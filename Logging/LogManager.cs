using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;

namespace Logging4net
{
    /// <summary>
    /// アプリケーションログを管理するロガーを表します。
    /// </summary>
    /// <remarks>
    /// このクラス及びその派生クラスを、１つのアプリケーションドメイン内に複数生成することはできません。
    /// 生成は、実行可能ファイル(exeモジュール)内で行ってください。
    /// </remarks>
    /// <example>
    /// Logging クラスの使用例を次に示します。
    /// <code>
    /// using (LogManager logmng = new LogManager())
    /// {
    ///    logmng.BaseFileName = "LogTest";
    ///
    ///    Log.Write(EventCode.Debug, ErrorLevel.Normal, 0, "Test");
    /// }
    /// </code>
    /// </example>
    public class LogManager : IDisposable
    {
        internal int _keepDays;
        internal int _limitFileLength;
        internal string _basePath;
        internal string _baseFileName;
        internal bool _autoFlush;
        internal bool _logFilenameWithPid;

        internal EventLog _eventLog;
        internal string _eventLogName = string.Empty;
        internal string _eventSourceName = string.Empty;

        /// <summary>
        /// <see cref="LogManager"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public LogManager()
        {
            // Initialize logger parameters
            _keepDays = 7 * 2;              // 2 weeks
            _limitFileLength = 1024 * 1024; // 1Mbytes size per file.
            _basePath = ".";                // current folder
            _basePath = System.Environment.GetEnvironmentVariable("LOCALAPPDATA") + @"\Logging4net\.";
            // execution file
            _baseFileName = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]);
            _autoFlush = true;              // auto flush true
            _logFilenameWithPid = false;    // ProcessId no add

            GetRegistryParameterForLogManager();

            // Initialize EventLog parameters => not use.
            //InitialEventLogSettings();

        }

        /// <summary>
        /// この <see cref="LogManager"/> オブジェクトによって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// この <see cref="LogManager"/> オブジェクトによって使用されているすべてのリソースを解放します。
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            TerminateEventLog();
        }

        /// <summary>
        /// ロガーで使用するパラメーターをレジストリから読み込みます
        /// </summary>
        private void GetRegistryParameterForLogManager()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\_myorg\_log");
            if (key != null)
            {
                object obj = null;
                try
                {
                    obj = key.GetValue("KeepDays");
                    if (obj != null && (int)obj > 0)
                    {
                        this._keepDays = (int)obj;
                    }
                }
                catch {}

                try
                {
                    obj = key.GetValue("LimitFileLength");
                    if (obj != null && (int)obj > 0)
                    {
                        this._limitFileLength = (int)obj;
                    }
                }
                catch {}

                try
                {
                    obj = key.GetValue("BasePath");
                    if (obj != null && ((string)obj).Length > 0)
                    {
                        this._basePath = (string)obj;
                    }
                }
                catch {}

                try
                {
                    obj = key.GetValue("AutoFlush");
                    if (obj != null && ((string)obj).Length > 0)
                    {
                        this._autoFlush = bool.Parse((string)obj);
                    }
                }
                catch {}

                key.Close();
                key = null;
            }
        }

        /// <summary>
        /// ロガーで使用するイベントログの設定を行います
        /// </summary>
        private void InitialEventLogSettings()
        {
            try
            {
                while (true)
                {
                    if (EventLog.SourceExists(this._eventSourceName))
                    {
                        EventLog.DeleteEventSource(this._eventSourceName);
                    }
                    else
                    {
                        break;
                    }
                }

                EventLog.CreateEventSource(this._eventSourceName, this._eventLogName);

                this._eventLog = new EventLog(this._eventLogName, ".", this._eventSourceName);
                this._eventLog.MaximumKilobytes = 512;
                this._eventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
            }
            catch (Exception ex)
            {
                string errlog = ex.ToString();

                if (this._eventLog != null)
                {
                    this._eventLog.Close();
                    this._eventLog = null;
                }
            }
        }

        /// <summary>
        /// ロガーで使用したイベントログの後処理を行います
        /// </summary>
        private void TerminateEventLog()
        {
            try
            {
                if (this._eventLog != null)
                {
                    this._eventLog.Close();
                    this._eventLog = null;
                }
            }
            catch (Exception ex)
            {
                string errlog = ex.ToString();
                this._eventLog = null;
            }
        }

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

        #endregion

    }

}
