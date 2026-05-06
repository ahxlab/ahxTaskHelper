using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Security;
using log4net;

namespace Logging4net
{
    /// <summary>
    /// ログ書き出し時のイベントコード
    /// </summary>
    /// <remarks>
    /// イベントコードの指定に従ったログファイルにログ内容を書き出します。
    /// ビットフラグで定義しているため論理和指定が可能。
    /// </remarks>
    [Flags]
    public enum EventCode
    {
        /// <summary>
        /// ログによる処理解析のために必要不可欠なログ
        /// </summary>
        Trace = 0x01,
        /// <summary>
        /// オペレーター、および、自動サービスアプリケーションが行った
        /// 操作履歴と実行結果(成功／失敗)を示すログ
        /// </summary>
        Operation = 0x02,
        /// <summary>
        /// 他システムとのインターフェース、または、通信に関するログ
        /// </summary>
        Communication = 0x04,
        /// <summary>
        /// データベースアクセスに関するログ
        /// </summary>
        DBAccess = 0x08,
        /// <summary>
        /// エラーが発生した際に書き出すログ
        /// </summary>
        Error = 0x10,
    }

    /// <summary>
    /// ログ書き出し時のエラーレベル
    /// </summary>
    public enum ErrorLevel
    {
        /// <summary>
        /// 正常処理
        /// </summary>
        Normal,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// エラー
        /// </summary>
        Error,
        /// <summary>
        /// 致命的なエラー
        /// </summary>
        Fatal,
    }

    /// <summary>
    /// アプリケーションからのログ書き出しインターフェースを提供します。
    /// このクラスは継承できません。
    /// </summary>
    /// <remarks>
    /// この型の全てのメンバはマルチスレッド操作で安全に使用できます。
    /// </remarks>
    public sealed class BaseLog
    {
        private static readonly ILog Logger = log4net.LogManager.GetLogger(typeof(BaseLog));

        static BaseLog()
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            if (File.Exists(configPath))
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(configPath));
            }
        }

        /// <summary>
        /// ログ出力後に発生します。
        /// このイベントは、ビルド構成をデバッグ構成（「DEBUG」シンボル有効）にした時のみ発生します。
        /// リリース構成では発生しません。
        /// ※注意：本イベントのイベントハンドラ内では、Log出力メソッドを実行しないでください。
        /// </summary>
        public static event LogWriteEventHandler LogWroteEvent;


        internal static EventCode[] _eventCodes = new EventCode[] {
            EventCode.Trace,
            EventCode.Operation,
            EventCode.Communication,
            EventCode.DBAccess,
            EventCode.Error };
        // internal static Hashtable _logger = new Hashtable();

		private BaseLog()
        {
        }

        /// <summary>
        /// 作業用のStringBuilderです。
        /// </summary>
        private static StringBuilder LoggingSB = new StringBuilder();


        internal static void Write(
            StackFrame sf,
            object instance,
            EventCode eventCode,
            ErrorLevel errorLevel,
            int errorCode,
            string logCode,
            string logText,
            params ParameterInfo[] logParams)
        {
            string message;

            try
            {
                lock (LoggingSB)
                {
                    LoggingSB.Length = 0;
                    LoggingSB.Append('[');

                    string instanceName = BaseLog.GetInstanceName(instance);
                    instanceName = null;        // TODO : スタックから取得できるので削除

                    if (instanceName == null || instanceName.Length == 0)
                    {
                        LoggingSB.Append('*');
                    }
                    else
                    {
                        LoggingSB.Append(instanceName);
                    }
                    LoggingSB.Append("] ");
                    LoggingSB.Append('[');
                    LoggingSB.Append(logCode);
                    LoggingSB.Append("] ");

                    if (logParams != null && logParams.Length != 0)
                    {
                        foreach (ParameterInfo pi in logParams)
                        {
                            if (pi != null)
                            {
                                LoggingSB.Append('[');
                                LoggingSB.Append(Log.ObjectToString(pi.ParameterID, "*"));
                                LoggingSB.Append('=');

                                for (int i = 0; i < pi.ValueList.Count; i++)
                                {
                                    if (i > 0)
                                        LoggingSB.Append(", ");
                                    if (pi.ValueList.Count > 1)
                                        LoggingSB.AppendFormat("{0}:", i);
                                    LoggingSB.Append(Log.ObjectToString(pi.ValueList[i], string.Empty));
                                }

                                LoggingSB.Append("] ");
                            }
                        }
                    }

                    if (logText != null && logText.Length != 0)
                    {
                        LoggingSB.Append(logText);
                    }
                    else
                    {
                        LoggingSB.Length--;
                    }

                    message = LoggingSB.ToString();
                }

                eventCode |= EventCode.Trace;
                if (errorLevel != ErrorLevel.Normal)
                {
                    eventCode |= EventCode.Error;
                }

                DateTime now = DateTime.Now;

                string datetime = string.Format(
                    "{0:d04}/{1:d02}/{2:d02} {3:d02}:{4:d02}:{5:d02}.{6:d03}",
                    now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);

                string assemblyName = GetAssemblyName(sf);
                string methodName = GetFunctionName(sf);
                int threadID = Thread.CurrentThread.ManagedThreadId;

                string log = string.Format(
                    "{0} {1:d03} {2,-5} 0x{4:X08} {5} {6} {7}\x0d\x0a",
                    datetime, (int)eventCode, GetErrorLevelString(errorLevel), errorCode,
                    threadID, assemblyName, methodName, message);

                ILog callerLogger = log4net.LogManager.GetLogger(assemblyName);

                foreach (EventCode ec in _eventCodes)
                {
                    if ((eventCode & ec) != 0)
                    {
                        string log4netMessage = string.Format("{0} {1}", methodName, message);
                        switch (errorLevel)
                        {
                            case ErrorLevel.Normal: callerLogger.Info(log4netMessage); break;
                            case ErrorLevel.Warning: callerLogger.Warn(log4netMessage); break;
                            case ErrorLevel.Error: callerLogger.Error(log4netMessage); break;
                            case ErrorLevel.Fatal: callerLogger.Fatal(log4netMessage); break;
                            default: callerLogger.Info(log4netMessage); break;
                        }
                    }
                }

                if (LogWroteEvent != null)
                {
                    LogWriteEventArgs eventArgs = new LogWriteEventArgs(
                        now,
                        eventCode,
                        errorLevel,
                        errorCode,
                        threadID,
                        assemblyName,
                        methodName,
                        log,
                        message,
                        instance,
                        logCode,
                        logText,
                        logParams);

                    LogWroteEvent(eventArgs);
                }
            }
            catch
            {
            }
        }
        
        internal static void Write(int functionFrame, object instance,
            EventCode eventCode, ErrorLevel errorLevel, int errorCode, string logCode,
            string logText, params ParameterInfo[] logParams)
        {
            functionFrame += 2;
            string message;

            try
            {
                lock (LoggingSB)
                {
                    LoggingSB.Length = 0;
                    LoggingSB.Append('[');

                    string instanceName = BaseLog.GetInstanceName(instance);
                    instanceName = null;        // TODO : スタックから取得できるので削除

                    if (instanceName == null || instanceName.Length == 0)
                    {
                        LoggingSB.Append('*');
                    }
                    else
                    {
                        LoggingSB.Append(instanceName);
                    }
                    LoggingSB.Append("] ");
                    LoggingSB.Append('[');
                    LoggingSB.Append(logCode);
                    LoggingSB.Append("] ");

                    if (logParams != null && logParams.Length != 0)
                    {
                        foreach (ParameterInfo pi in logParams)
                        {
                            if (pi != null)
                            {
                                LoggingSB.Append('[');
                                LoggingSB.Append(Log.ObjectToString(pi.ParameterID, "*"));
                                LoggingSB.Append('=');

                                for (int i = 0; i < pi.ValueList.Count; i++)
                                {
                                    if (i > 0)
                                        LoggingSB.Append(", ");
                                    if (pi.ValueList.Count > 1)
                                        LoggingSB.AppendFormat("{0}:", i);
                                    LoggingSB.Append(Log.ObjectToString(pi.ValueList[i], string.Empty));
                                }

                                LoggingSB.Append("] ");
                            }
                        }
                    }

                    if (logText != null && logText.Length != 0)
                    {
                        LoggingSB.Append(logText);
                    }
                    else
                    {
                        LoggingSB.Length--;
                    }

                    message = LoggingSB.ToString();
                }

                eventCode |= EventCode.Trace;
                if (errorLevel != ErrorLevel.Normal)
                {
                    eventCode |= EventCode.Error;
                }

                DateTime now = DateTime.Now;

                string datetime = string.Format(
                    "{0:d04}/{1:d02}/{2:d02} {3:d02}:{4:d02}:{5:d02}.{6:d03}",
                    now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);

                string assemblyName = GetAssemblyName(functionFrame);
                string methodName = GetFunctionName(functionFrame);
                int threadID = Thread.CurrentThread.ManagedThreadId;

                string log = string.Format(
                    "{0} {1:d03} {2,-5} 0x{4:X08} {5} {6} {7}\x0d\x0a",
                    datetime, (int)eventCode, GetErrorLevelString(errorLevel), //errorCode,
                    threadID, assemblyName, methodName, message);

                ILog callerLogger = log4net.LogManager.GetLogger(assemblyName);

                foreach (EventCode ec in _eventCodes)
                {
                    if ((eventCode & ec) != 0)
                    {
                        string log4netMessage = string.Format("{0} {1}", methodName, message);
                        switch (errorLevel)
                        {
                            case ErrorLevel.Normal: callerLogger.Info(log4netMessage); break;
                            case ErrorLevel.Warning: callerLogger.Warn(log4netMessage); break;
                            case ErrorLevel.Error: callerLogger.Error(log4netMessage); break;
                            case ErrorLevel.Fatal: callerLogger.Fatal(log4netMessage); break;
                            default: callerLogger.Info(log4netMessage); break;
                        }
                    }
                }

				if (LogWroteEvent != null)
                {
                    LogWriteEventArgs eventArgs = new LogWriteEventArgs(
                        now,
                        eventCode,
                        errorLevel,
                        errorCode,
                        threadID,
                        assemblyName,
                        methodName,
                        log,
                        message,
                        instance,
                        logCode,
                        logText,
                        logParams);

                    LogWroteEvent(eventArgs);
                }
            }
            catch
            {
            }
        }

        internal static void Write(
            EventCode   eventCode,
            ErrorLevel  errorLevel,
            int         errorCode,
            string      message )
        {
            Write(eventCode, errorLevel, errorCode, message, 3);
        }

        internal static void Write(
            EventCode   eventCode,
            ErrorLevel  errorLevel,
            int         errorCode,
            string      message,
            int         functionFrame)
        {
			DateTime now = DateTime.Now;

			string datetime = string.Format(
                "{0:d04}/{1:d02}/{2:d02} {3:d02}:{4:d02}:{5:d02}.{6:d03}",
                now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, now.Millisecond);

            int threadID = Thread.CurrentThread.ManagedThreadId;
            string assemblyName = GetAssemblyName(functionFrame);
            string methodName = GetFunctionName(functionFrame);

			string log = string.Format(
                "{0} {1:d03} {2,-5} 0x{4:X08} {5} {6} {7}\x0d\x0a",
                datetime, (int)eventCode, GetErrorLevelString(errorLevel), errorCode,
                threadID, assemblyName, methodName, message);

            foreach (EventCode ec in _eventCodes)
            {
                if ((eventCode & ec) != 0)
                {
                    string log4netMessage = string.Format("{0} {1} {2}", assemblyName, methodName, message);
                    switch (errorLevel)
                    {
                        case ErrorLevel.Normal: Logger.Info(log4netMessage); break;
                        case ErrorLevel.Warning: Logger.Warn(log4netMessage); break;
                        case ErrorLevel.Error: Logger.Error(log4netMessage); break;
                        case ErrorLevel.Fatal: Logger.Fatal(log4netMessage); break;
                        default: Logger.Info(log4netMessage); break;
                    }
                }
            }

            if (LogWroteEvent != null)
            {
                LogWriteEventArgs eventArgs = new LogWriteEventArgs(
                    now,
                    eventCode,
                    errorLevel,
                    errorCode,
                    threadID,
                    assemblyName,
                    methodName,
                    log,
                    message,
                    null,
                    null,
                    null,
                    null);

                LogWroteEvent(eventArgs);
            }
        }

		/// <summary>
		/// ログ書き出しの呼び出し元に遡って指定スタックフレーム分をスキップしたモジュール名を取得します。
		/// </summary>
		/// <param name="skipStackFrames">スキップするスタックフレーム数を指定します。</param>
		/// <returns>スキップしたフレーム位置に該当するモジュール名の文字列を返します。</returns>
        [DynamicSecurityMethod]
		internal static string GetAssemblyName(int skipStackFrames)
		{
			string AssemblyName = string.Empty;

			try
			{
				StackFrame sf = new StackFrame(skipStackFrames);
				AssemblyName = sf.GetMethod().DeclaringType.Module.Name;
			}
			catch
			{
			}

			return AssemblyName;
		}

        /// <summary>
        /// 指定された StackFrame からモジュール名を取得します。
        /// </summary>
        /// <param name="sf">System.Diagnostics.StackFrame class. モジュール名を取得する StackFrame を指定します。</param>
        /// <returns>指定した StackFrame のモジュール名を返します。</returns>
        internal static string GetAssemblyName(StackFrame sf)
        {
            string assemblyName = string.Empty;

            try
            {
                if (sf != null)
                {
                    assemblyName = sf.GetMethod().DeclaringType.Module.Name;
                }
            }
            catch
            {
            }

            return assemblyName;
        }

        /// <summary>
        /// ログ書き出しの呼び出し元に遡って指定スタックフレーム分をスキップしたクラスと関数名を取得します。
        /// </summary>
        /// <param name="skipStackFrames">スキップするスタックフレーム数を指定します。</param>
        /// <returns>スキップしたフレーム位置に該当するクラスと関数名の文字列を返します。</returns>
        internal static string GetFunctionName(int skipStackFrames)
        {
            try
            {
                StackFrame sf = new StackFrame(skipStackFrames, false);
                return GetFunctionName(sf);
            }
            catch
            {
                return "";
            }
        }

        internal static string GetFunctionName(StackFrame sf)
        {
            try
            {
                MethodBase mb = sf.GetMethod();
                if (mb == null) return "";

                Type declaringType = mb.DeclaringType;
                string methodName = mb.Name;

                // Handle async state machine
                if (methodName == "MoveNext" && declaringType != null && declaringType.Name.Contains("<"))
                {
                    int start = declaringType.Name.IndexOf('<');
                    int end = declaringType.Name.IndexOf('>');
                    if (start >= 0 && end > start)
                    {
                        methodName = declaringType.Name.Substring(start + 1, end - start - 1);
                        // Also try to get the original class name
                        if (declaringType.DeclaringType != null)
                        {
                            declaringType = declaringType.DeclaringType;
                        }
                    }
                }

                StringBuilder sb = new StringBuilder();
                if (declaringType != null)
                {
                    sb.Append(declaringType.Name);
                }
                sb.Append("::");
                sb.Append(methodName);

                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// ErrorLevel のログ用文字列を返します。
        /// </summary>
        /// <param name="errorLevel">文字列化対象の ErrorLevel 列挙体の１つを指定します。</param>
        /// <returns>指定された ErrorLevel 列挙体に該当する文字列を返します。</returns>
        internal static string GetErrorLevelString(ErrorLevel errorLevel)
        {
            string errorLevelString = "";

            switch (errorLevel)
            {
                case ErrorLevel.Normal:
                    errorLevelString = "*";
                    break;
                case ErrorLevel.Warning:
                    errorLevelString = "Warn";
                    break;
                case ErrorLevel.Error:
                    errorLevelString = "Error";
                    break;
                case ErrorLevel.Fatal:
                    errorLevelString = "Fatal";
                    break;
                default:
                    errorLevelString = "";
                    break;
            }
            return errorLevelString;
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// 指定のオブジェクトのインスタンス名を返します。
        /// nullを指定するとstring.Emptyを返します。
        /// nullを返す可能性があることを考慮してください。
        /// </summary>
        /// <param name="instance">インスタンス識別子</param>
        internal static string GetInstanceName(object instance)
        {
            string instanceName;

            if (instance is ILogging)
            {
                ILogging writer = (ILogging)instance;
                instanceName = writer.InstanceName;
            }
            else
            {
                instanceName = Log.ObjectToString(instance, string.Empty);
            }

            return instanceName;
        }

    }

    /// <summary>
    /// LogWriteEventのイベントデリゲートです。
    /// </summary>
    /// <param name="e"></param>
    public delegate void LogWriteEventHandler(LogWriteEventArgs e);

    /// <summary>
    /// LogWriteEventのイベントパラメータクラスです。
    /// </summary>
    public class LogWriteEventArgs : EventArgs
    {
        /// <summary>
        /// ログ出力内容を指定して、
        /// クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="loggingTime">ログ出力日時を指定します。</param>
        /// <param name="eventCode">イベントコードを指定します。</param>
        /// <param name="errorLevel">エラーレベルを指定します。</param>
        /// <param name="errorCode">エラーコードを指定します。</param>
        /// <param name="threadID">スレッドIDを指定します。</param>
        /// <param name="assemblyName">アセンブリ名を指定します。</param>
        /// <param name="methodName">メソッド名を指定します。</param>
        /// <param name="log">
        /// ログヘッダーなどすべてを含むログ出力文字列を指定します。</param>
        /// <param name="appLog">
        /// インスタンス識別子以降のログ出力文字列を指定します。</param>
        /// <param name="instance">ログ出力を行ったインスタンスを指定します。
        /// 文字列を指定して、インスタンス名を直接指定することができます。</param>
        /// <param name="logCode">ログコードを指定します。</param>
        /// <param name="logText">ログテキストを指定します。</param>
        /// <param name="logParams">ログパラメーターを指定します。</param>
        public LogWriteEventArgs(
            DateTime loggingTime,
            EventCode eventCode,
            ErrorLevel errorLevel,
            int errorCode,
            int threadID,
            string assemblyName,
            string methodName,
            string log,
            string appLog,
            object instance,
            string logCode,
            string logText,
            params object[] logParams)
        {
            _loggingTime = loggingTime;
            _eventCode = eventCode;
            _errorLevel = errorLevel;
            _errorCode = errorCode;
            _threadID = threadID;
            _assemblyName = assemblyName;
            _methodName = methodName;

            _log = log;
            _appLog = appLog;
            _instance = instance;
            _logCode = logCode;
            _logText = logText;
            _logParams = logParams;
        }



        /// <summary>
        /// ログ出力日時を取得します。
        /// </summary>
        public DateTime LoggingTime
        {
            get
            {
                return _loggingTime;
            }
        }

        /// <summary>
        /// イベントコードを取得します。
        /// </summary>
        public EventCode EventCode
        {
            get
            {
                return _eventCode;
            }
        }

        /// <summary>
        /// エラーレベルを取得します。
        /// </summary>
        public ErrorLevel ErrorLevel
        {
            get
            {
                return _errorLevel;
            }
        }

        /// <summary>
        /// エラーコードを取得します。
        /// </summary>
        public int ErrorCode
        {
            get
            {
                return _errorCode;
            }
        }

        /// <summary>
        /// スレッドIDを取得します。
        /// </summary>
        public int ThreadID
        {
            get
            {
                return _threadID;
            }
        }

        /// <summary>
        /// アセンブリ名を取得します。
        /// </summary>
        public string AssemblyName
        {
            get
            {
                return _assemblyName;
            }
        }

        /// <summary>
        /// メソッド名を取得します。
        /// </summary>
        public string MethodName
        {
            get
            {
                return _methodName;
            }
        }


        /// <summary>
        /// ログヘッダーなどすべてを含むログ出力文字列を取得します。
        /// 行頭に日時があり、末尾に改行コードが含まれています。
        /// </summary>
        public string Log
        {
            get
            {
                return _log;
            }
        }

        /// <summary>
        /// インスタンス識別子以降のログ出力文字列を取得します。
        /// 日時および末尾の改行コードは含みません。日時を含めて文字列で取得する場合、
        /// AppLogWithDateTimeプロパティを参照してください。
        /// </summary>
        public string AppLog
        {
            get
            {
                return _appLog;
            }
        }

        /// <summary>
        /// 日時と、インスタンス識別子以降のログ出力文字列を合わせて取得します。
        /// 末尾に改行コードが含まれています。
        /// </summary>
        public string AppLogWithDateTime
        {
            get
            {
                return string.Format("{0} {1}\r\n",
                    _loggingTime.ToString("yyyy/MM/dd HH:mm:ss.fff"), _appLog);
            }
        }


        /// <summary>
        /// ログ出力を行ったオブジェクトを取得します。
        /// このオブジェクトの型が文字列(string)の場合は、
        /// インスタンス名を直接表しています。
        /// インスタンス名が指定されていない場合はnullを返します。
        /// </summary>
        public object Instance
        {
            get
            {
                return _instance;
            }
        }


        /// <summary>
        /// インスタンス名を取得します。
        /// インスタンス名が指定されていない場合はstring.Emptyを返します。
        /// </summary>
        public string InstanceName
        {
            get
            {
                return BaseLog.GetInstanceName(_instance);
            }
        }


        /// <summary>
        /// ログコードを取得します。
        /// </summary>
        public string LogCode
        {
            get
            {
                return _logCode;
            }
        }

        /// <summary>
        /// ログテキストを取得します。
        /// </summary>
        public string LogText
        {
            get
            {
                return _logText;
            }
        }

        /// <summary>
        /// ログパラメーターを取得します。
        /// </summary>
        public object[] LogParams
        {
            get
            {
                return _logParams;
            }
        }


        private DateTime _loggingTime;

        private EventCode _eventCode;

        private ErrorLevel _errorLevel;

        private int _errorCode;

        private int _threadID;

        private string _assemblyName;

        private string _methodName;

        private string _log;

        private string _appLog;

        private object _instance;

        private string _logCode;

        private string _logText;

        private object[] _logParams;

    }

    public interface ILogging
    {
        string InstanceName
        {
            get;
        }
    }
}

namespace System.Security
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    internal sealed class DynamicSecurityMethodAttribute : Attribute
    {
    }
}
