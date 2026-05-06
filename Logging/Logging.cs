using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Reflection;

namespace Logging4net
{
    /// <summary>
    /// 物理ログファイルに書き出す仕組みを提供します。
    /// </summary>
    internal class Logging : IDisposable
    {
        // logfile parameters
        private int _keepDays;
        private int _limitFileLength;
        private string _basePath;
        private string _baseFileName;
        private bool _autoFlush;
        private bool _logFilenameWithPid;

        private EventCode _eventCode;
        private Queue _queue;
        private int _queueCount;
        private Thread _writeThread;
        private AutoResetEvent _resetEvent;

        // local terminal information
        private string _osVersion;
        private int _processID;
        private string _hostname;
        private string _logStartTime;
        private string _ipAddress;
        private string _loggerAssemblyName;

        // event log parameters
        private EventLog _eventLog;

        /// <summary>
        /// <see cref="Logging"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="eventCode"><see cref="EventCode"/> で定義されたログ種別を指定します</param>
        public Logging(EventCode eventCode)
        {
            _eventCode = eventCode;
        }

        #region IDisposable Members
        /// <summary>
        /// この <see cref="Logging"/> オブジェクトによって使用されている全てのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// この <see cref="Logging"/> オブジェクトによって使用されているすべてのリソースを解放します。
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            Interlocked.Increment(ref _queueCount);
            _resetEvent.Set();

            _writeThread.Join();
        }

        /// <summary>
        /// コンストラクターで指定されたログ種別に対する物理ログファイルの拡張子を返します
        /// </summary>
        /// <returns></returns>
        private string GetEventCodeExtension()
        {
            string ext;
            switch (_eventCode)
            {
                case EventCode.Trace:
                    ext = ".tr.log";
                    break;
                case EventCode.Operation:
                    ext = ".op.log";
                    break;
                case EventCode.Communication:
                    ext = ".cn.log";
                    break;
                case EventCode.DBAccess:
                    ext = ".db.log";
                    break;
                case EventCode.Error:
                    ext = ".er.log";
                    break;
                default:
                    ext = "";
                    break;
            }
            return ext;
        }

        /// <summary>
        /// 物理ログファイルに書き出すスレッドを起動します。
        /// </summary>
        internal void StartLogging()
        {
            _resetEvent = new AutoResetEvent(false);
            _queue = Queue.Synchronized(new Queue());
            _queueCount = 0;

            _writeThread = new Thread(new ThreadStart(LoggingLoop));
            _writeThread.IsBackground = true;
            _writeThread.Name = "Logger" + _eventCode.ToString();
            _writeThread.Start();
        }

        /// <summary>
        /// 物理ログファイルへの指定ログの書き出し要求を行います
        /// </summary>
        /// <param name="log"></param>
        internal void Write(string log)
        {
            try
            {
                lock (_queue.SyncRoot)
                {
                    _queue.Enqueue(log);
                    Interlocked.Increment(ref _queueCount);
                }
                _resetEvent.Set();
            }
            catch (Exception ex)
            {
                WriteAnotherLog(
                    string.Format("Logging::Write ThreadName:[{0}], ExMsg:[{1}]",
                    _writeThread.Name, ex.ToString()));
            }
        }

        /// <summary>
        /// 物理ログへの書き出しスレッドループ
        /// </summary>
        private void LoggingLoop()
        {
            StreamWriter fsw = null;
            int writtenBytes = 0;
            DateTime lastLogDate = DateTime.MinValue;

            try
            {
                WriteAnotherLog(
                    string.Format("!!! Logging Start !!! ThreadName[{0}]", _writeThread.Name));

                while (true)
                {
                    _resetEvent.WaitOne();

                    while (_queueCount > 0)
                    {
                        string log;

                        lock (_queue.SyncRoot)
                        {
                            log = (string)_queue.Dequeue();
                            Interlocked.Decrement(ref _queueCount);
                        }

                        try
                        {
                            int logBytes = Encoding.UTF8.GetByteCount(log);

                            if (fsw == null)
                            {
                                fsw = OpenNextLogFile(true);
                            }
                            else
                            {
                                if (writtenBytes + logBytes > this._limitFileLength
                                    || lastLogDate != DateTime.Now.Date)
                                {
                                    fsw.Close();
                                    fsw = null;
                                    writtenBytes = 0;
                                    fsw = OpenNextLogFile(false);
                                }
                            }

                            fsw.Write(log);
                            writtenBytes += logBytes;
                            lastLogDate = DateTime.Now.Date;
                        }
                        catch (Exception ex)
                        {
                            WriteAnotherLog(
                                string.Format("Logging::LoggingLoop ThreadName[{0}] In while loop : {1}",
                                    _writeThread.Name, ex.ToString()));
                            WriteAnotherLog(log);
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                WriteAnotherLog(
                    string.Format("Logging::LoggingLoop ThreadName[{0}] Logging Finalizing...",
                        _writeThread.Name));
            }
            catch (Exception ex)
            {
                WriteAnotherLog(
                    string.Format("Logging::LoggingLoop ThreadName[{0}] In catch(Exception) : {1}",
                        _writeThread.Name, ex.ToString()));
            }
            finally
            {
                WriteAnotherLog(
                    string.Format("!!! Logging End !!! ThreadName[{0}]", _writeThread.Name));

                if (fsw != null)
                {
                    fsw.Close();
                    fsw = null;
                }
            }
        }

        /// <summary>
        /// 引数で渡されたログ保持期間を過ぎたログを日付フォルダー毎削除します
        /// </summary>
        /// <param name="keepDays">ログの保持期間を日数単位で指定します</param>
        private void CleanUpOldFiles(int keepDays)
        {
            DateTime limitDate = DateTime.Now - new TimeSpan(keepDays, 0, 0, 0);
            string compareString = string.Format("{0:d04}{1:d02}{2:d02}",
                limitDate.Year, limitDate.Month, limitDate.Day);
            string mutexName = "Logger" + compareString;
            bool IsMutexCreate = true;
            Mutex mutex = null;

            try
            {
                mutex = new Mutex(true, mutexName, out IsMutexCreate);

                if (IsMutexCreate == false)
                {
                    mutex.Close();
                    mutex = null;
                }
                else
                {
                    try
                    {
                        string[] subDirectories = Directory.GetDirectories(_basePath);

                        foreach (string subDirectory in subDirectories)
                        {
                            if (string.Compare(Path.GetFileName(subDirectory), compareString) < 0)
                            {
                                try
                                {
                                    Directory.Delete(subDirectory, true);
                                }
                                catch (Exception ex)
                                {
                                    WriteAnotherLog(
                                        string.Format("Logging::CleanUpOldFiles ThreadName[{0}],  Directory.Delete:[{1}], ExMsg:[{2}]",
                                        _writeThread.Name, subDirectory, ex.ToString()));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteAnotherLog(
                            string.Format("Logging::CleanUpOldFiles ThreadName[{0}], Directory.GetDirectories:[{1}], ExMsg:[{2}]",
                            _writeThread.Name, _basePath, ex.ToString()));
                    }
                    finally
                    {
                        if (mutex != null)
                        {
                            mutex.ReleaseMutex();
                            mutex.Close();
                            mutex = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteAnotherLog(
                    string.Format("Logging::CleanUpOldFiles ThreadName[{0}], ExMsg:[{1}]",
                    _writeThread.Name, ex.ToString()));
            }
        }

        /// <summary>
        /// ログサイズ越え、または、日付替わりが発生した際の次ログファイルの準備を行います。
        /// </summary>
        /// <param name="IsTheFirstFile">アプリケーション起動後最初のログファイルかどうかを指定します</param>
        /// <returns>次の新しいログファイルに対する StreamWriter object を返します</returns>
        private StreamWriter OpenNextLogFile(bool IsTheFirstFile)
        {
            if (_keepDays > 0)
            {
                CleanUpOldFiles(_keepDays);
            }
            StreamWriter fsw = OpenLogFile(IsTheFirstFile);
            return fsw;
        }

        /// <summary>
        /// 新しいログファイルを作成します。
        /// </summary>
        /// <remarks>
        /// 新しいログファイルを作成する際、該当日付フォルダーが存在しなければ該当日付フォルダーも新規に作成します。
        /// </remarks>
        /// <param name="IsTheFirstFile">アプリケーション起動後最初のログファイルかどうかを指定します</param>
        /// <returns>新しいログファイルに対する StreamWriter object を返します</returns>
        private StreamWriter OpenLogFile(bool IsTheFirstFile)
        {
            DateTime now = DateTime.Now;
            string path = string.Format("{0}{1}{2:d04}{3:d02}{4:d02}",
                this._basePath, Path.DirectorySeparatorChar,
                now.Year, now.Month, now.Day);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += string.Format("{0}{1}{2}{3:d04}{4:d02}{5:d02}{6:d02}{7:d02}{8:d02}",
                Path.DirectorySeparatorChar, this._baseFileName,
                (this._logFilenameWithPid ? Process.GetCurrentProcess().Id.ToString() + "_" : ""),
                now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

            if (IsTheFirstFile)
            {
                path += "_S" + GetEventCodeExtension();

                // Cache Header Information
                this._hostname = Environment.MachineName;
                this._osVersion = Environment.OSVersion.VersionString;
                this._processID = Process.GetCurrentProcess().Id;

                try
                {
                    this._ipAddress = " ";
                    NetworkInterface[] _adapters = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (NetworkInterface adapter in _adapters)
                    {
                        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                        {
                            continue;
                        }

                        IPInterfaceProperties props = adapter.GetIPProperties();
                        UnicastIPAddressInformationCollection unis = props.UnicastAddresses;
                        foreach (UnicastIPAddressInformation uni in unis)
                        {
                            this._ipAddress += uni.Address.ToString() + " ";
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteAnotherLog(
                        string.Format("Logging::OpenLogFile ThreadName[{0}], ExMsg:[{1}]",
                        _writeThread.Name, ex.ToString()));
                }

                this._loggerAssemblyName = "-----";
                try
                {
                    Assembly asm = Assembly.GetAssembly(this.GetType());
                    FileInfo fileInfo = new FileInfo(asm.Location);
                    this._loggerAssemblyName = fileInfo.Name;
                }
                catch (Exception ex)
                {
                    WriteAnotherLog(
                        string.Format("Logging::OpenLogFile ThreadName[{0}], ExMsg:[{1}]",
                        _writeThread.Name, ex.ToString()));
                }

                _logStartTime = string.Format(
                    "{0:d04}/{1:d02}/{2:d02} {3:d02}:{4:d02}:{5:d02}.{6:d03}",
                    now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
            }
            else
            {
                path += "_C" + GetEventCodeExtension();
            }

            StreamWriter fsw = new StreamWriter(path, true);
            fsw.AutoFlush = this._autoFlush;

            // Write Header Information
            string fixedlog = "";
            string log;
            log = string.Format(
                "{0}_/_/_/_/_/ MachineName[{1}], OS[{2}]\x0d\x0a",
                fixedlog, this._hostname, this._osVersion);
            fsw.Write(log);

            log = string.Format(
                "{0}_/_/_/_/_/ LogStart DateTime[{1}], ProcessID[{2}], IP Address[{3}]\x0d\x0a",
                fixedlog, _logStartTime, this._processID, this._ipAddress);
            fsw.Write(log);

            return fsw;
        }

        #region Anothor Logging
        /// <summary>
        /// 標準サポートするログファイルとは別のログファイルなどに指定ログ内容を書き出します
        /// </summary>
        /// <remarks>
        /// Logger 内部で発生した障害調査のために、別のログファイルなどに指定ログ内容を書き出します。
        /// </remarks>
        /// <param name="log">他のログに書き出すログ内容を指定します</param>
        private void WriteAnotherLog(string log)
        {
            WriteEventLog(log);
        }

        /// <summary>
        /// 標準サポートするログファイルとは別のログファイルに指定ログ内容を書き出します
        /// </summary>
        /// <param name="log">ログとして書き出すログ内容を指定します</param>
        private void WriteFileLog(string log)
        {
            StreamWriter streamWriter = null;

            try
            {
                DateTime now = DateTime.Now;
                string filePath = string.Format("{0}{1}{2:d04}{3:d02}{4:d02}",
                    this._basePath, Path.DirectorySeparatorChar,
                    now.Year, now.Month, now.Day);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                filePath += string.Format("{0}{1}{2}_logger.log",
                    Path.DirectorySeparatorChar, this._baseFileName,
                    (_logFilenameWithPid ? Process.GetCurrentProcess().Id.ToString() : ""));

                streamWriter = new StreamWriter(filePath, true);
                streamWriter.AutoFlush = true;

                string writeLog = string.Format(
                    "{0:d04}/{1:d02}/{2:d02} {3:d02}:{4:d02}:{5:d02}.{6:d03} 0x{7:X08} {8}\x0d\x0a",
                    now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond,
                    Thread.CurrentThread.ManagedThreadId, log
                    );

                streamWriter.Write(writeLog);
            }
            catch (Exception ex)
            {
                WriteAnotherLog(
                    string.Format("Logging::WriteFileLog ThreadName:[{0}], ExMsg:[{1}]",
                    _writeThread.Name, ex.ToString()));
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter = null;
                }
            }
        }

        /// <summary>
        /// Windowsイベントログに、指定ログ内容を書き出します。
        /// </summary>
        /// <param name="log">Windowsイベントログに書き出すログ内容を指定します</param>
        private void WriteEventLog(string log)
        {
            DateTime now = DateTime.Now;

            if (this._eventLog != null)
            {
                lock (this._eventLog)
                {
                    try
                    {
                        string dateTime = string.Format(
                            "DateTime  : {0:d04}/{1:d02}/{2:d02} {3:d02}:{4:d02}:{5:d02}.{6:d03}\r\n",
                            now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);
                        string processId = string.Format(
                            "ProcessID : {0}\r\n", Process.GetCurrentProcess().Id.ToString());
                        string threadId = string.Format(
                            "ThreadID  : 0x{0:X08}\r\n", Thread.CurrentThread.ManagedThreadId);
                        string eventCode = string.Format(
                            "EventCode : {0}\r\n", _eventCode.ToString());

                        string writeLog = string.Format(
                            "{0}{1}{2}{3}LogMessage: {4}\r\n", dateTime, processId, threadId, eventCode, log);

                        this._eventLog.WriteEntry(writeLog);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        #endregion

        #region Logger Parameter Properties
        /// <summary>
        /// ログファイルの保存日数を取り、または設定します。
        /// </summary>
        /// <remarks>
        /// 規定値は、レジストリから取得します。取得できない場合のデフォルト値は１４です。
        /// </remarks>
        public int KeepDays
        {
            set
            {
                this._keepDays = value;
            }
            get
            {
                return this._keepDays;
            }
        }

        /// <summary>
        /// １つのログファイルの最大サイズを取得し、または設定します。
        /// </summary>
        /// <remarks>
        /// 規定値は、レジストリから取得します。取得できない場合のデフォルト値は 1048576 (1MBytes) です。
        /// </remarks>
        public int LimitFileLength
        {
            set
            {
                this._limitFileLength = value;
            }
            get
            {
                return this._limitFileLength;
            }
        }

        /// <summary>
        /// ログを書き出すベースパス名を取得し、または設定します。
        /// </summary>
        /// <remarks>
        /// 規定値は、レジストリから取得します。取得できない場合のデフォルト値はカレントフォルダーです。
        /// </remarks>
        public string BasePath
        {
            set
            {
                this._basePath = value;
            }
            get
            {
                return this._basePath;
            }
        }

        /// <summary>
        /// ログのベースファイル名を取得し、または設定します。
        /// </summary>
        /// <remarks>
        /// 規定値は、拡張子を除いた実行ファイル名です。
        /// </remarks>
        public string BaseFileName
        {
            set
            {
                this._baseFileName = value;
            }
            get
            {
                return this._baseFileName;
            }
        }

        /// <summary>
        /// ログを書き出す度に、出力バッファーをフラッシュするかどうかを示す値を取得し、または設定します。
        /// </summary>
        /// <remarks>
        /// 規定値は、レジストリから取得します。取得できない場合の規定値は false です。
        /// </remarks>
        public bool AutoFlush
        {
            set
            {
                this._autoFlush = value;
            }
            get
            {
                return this._autoFlush;
            }
        }

        /// <summary>
        /// ログファイル名にプロセスＩＤを付加するかどうかを示す値を取得し、または設定します。<br/>
        /// 同一アプリケーションが複数起動される場合に、ログファイル名の重複を避けることができます。
        /// </summary>
        /// <remarks>
        /// 規定値は false です。
        /// </remarks>
        public bool LogFilenameWithPid
        {
            set
            {
                this._logFilenameWithPid = value;
            }
            get
            {
                return this._logFilenameWithPid;
            }
        }

        /// <summary>
        /// LogManager で生成したイベントログハンドラを設定します。
        /// </summary>
        /// <remarks>
        /// 規定値は null です。
        /// </remarks>
        public EventLog EventLogHandler
        {
            set
            {
                if (System.Environment.OSVersion.Version.Major < 6)
                {
                    this._eventLog = value;
                }
            }
        }

        #endregion
    }
}
