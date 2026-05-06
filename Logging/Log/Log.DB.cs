using System;
using System.Diagnostics;

namespace Logging4net
{
	/// <summary>
	/// ���O�o�̓��\�b�h��܂Ƃ߂��N���X�ł��B
	/// </summary>
	public static partial class Log
	{
        //__________________________________________________________________________________________
        #region ��DB INFO


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB INFO���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_INFO(object instance)
        {
            DB_INFO(GetStackFrame(), instance, null);
        }
        /// <summary>
        /// DB INFO���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB(object instance)
        {
            DB_INFO(GetStackFrame(), instance, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB INFO���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_INFO(object instance, string logText)
        {
            DB_INFO(GetStackFrame(), instance, logText);
        }

        /// <summary>
        /// DB INFO���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB(object instance, string logText)
        {
            DB_INFO(GetStackFrame(), instance, logText);
        }
        
		//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
		/// <summary>
		/// DB INFO���O�o�̓��\�b�h�ł��B
		/// </summary>
		public static void DB_INFO(object instance, params ParameterInfo[] logParams)
		{
			DB_INFO(GetStackFrame(), instance, null, logParams);
		}

        /// <summary>
        /// DB INFO���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB(object instance, params ParameterInfo[] logParams)
        {
            DB_INFO(GetStackFrame(), instance, null, logParams);
        }
        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB INFO���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_INFO(StackFrame sf, object instance, string logText)
        {
            DB_INFO(sf, instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB INFO���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_INFO(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.DBAccess | EventCode.Trace, ErrorLevel.Normal,
                0, "DB INFO", logText, logParams);
        }


        #endregion

        //__________________________________________________________________________________________
        #region ��DB CONNECT


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB CONNECT���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_CONNECT(object instance)
        {
            DB_CONNECT(GetStackFrame(), instance, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB CONNECT���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_CONNECT(object instance, string logText)
        {
            DB_CONNECT(GetStackFrame(), instance, logText);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB CONNECT���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_CONNECT(StackFrame sf, object instance, string logText)
        {
            DB_CONNECT(sf, instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB CONNECT���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_CONNECT(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.DBAccess | EventCode.Trace, ErrorLevel.Normal,
                0, "DB CONNECT", logText, logParams);
        }


        #endregion

        //__________________________________________________________________________________________
        #region ��DB OPEN


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB OPEN���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_OPEN(object instance)
        {
            DB_OPEN(GetStackFrame(), instance, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB OPEN���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_OPEN(object instance, string logText)
        {
            DB_OPEN(GetStackFrame(), instance, logText);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB OPEN���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_OPEN(StackFrame sf, object instance, string logText)
        {
            DB_OPEN(sf, instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB OPEN���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_OPEN(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.DBAccess | EventCode.Trace, ErrorLevel.Normal,
                0, "DB OPEN", logText, logParams);
        }


        #endregion

        //__________________________________________________________________________________________
        #region ��DB CLOSE


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB CLOSE���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_CLOSE(object instance)
        {
            DB_CLOSE(GetStackFrame(), instance, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB CLOSE���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_CLOSE(object instance, string logText)
        {
            DB_CLOSE(GetStackFrame(), instance, logText);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB CLOSE���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_CLOSE(StackFrame sf, object instance, string logText)
        {
            DB_CLOSE(sf, instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB CLOSE���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_CLOSE(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.DBAccess | EventCode.Trace, ErrorLevel.Normal,
                0, "DB CLOSE", logText, logParams);
        }


        #endregion

        //__________________________________________________________________________________________
        #region ��DB QUERY


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_QUERY(object instance)
        {
            DB_QUERY(GetStackFrame(), instance, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_QUERY(object instance, string logText)
        {
            DB_QUERY(GetStackFrame(), instance, logText);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_QUERY(StackFrame sf, object instance, string logText)
        {
            DB_QUERY(sf, instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_QUERY(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.DBAccess | EventCode.Trace, ErrorLevel.Normal,
                0, "DB QUERY", logText, logParams);
        }


        #endregion

        //__________________________________________________________________________________________
        #region ��DB QUERY RET


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY RET���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_QUERY_RET(object instance)
        {
            DB_QUERY_RET(GetStackFrame(), instance, null, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY RET���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_QUERY_RET(object instance, string logText)
        {
            DB_QUERY_RET(GetStackFrame(), instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY RET���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_QUERY_RET(object instance, string logText,
            params ParameterInfo[] logParams)
        {
            DB_QUERY_RET(GetStackFrame(), instance, logText, logParams);
        }

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY RET���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_QUERY_RET(StackFrame sf, object instance, string logText)
        {
            DB_QUERY_RET(sf, instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY RET���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_QUERY_RET(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.DBAccess | EventCode.Trace, ErrorLevel.Normal,
                0, "DB QUERY RET", logText, logParams);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��DB QUERY RESULTSET


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY RESULTSET���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_QUERY_RESULTSET(object instance)
        {
            DB_QUERY_RESULTSET(GetStackFrame(), instance, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY RESULTSET���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_QUERY_RESULTSET(object instance, string logText)
        {
            DB_QUERY_RESULTSET(GetStackFrame(), instance, logText);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY RESULTSET���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_QUERY_RESULTSET(object instance, params ParameterInfo[] logParams)
        {
            DB_QUERY_RESULTSET(GetStackFrame(), instance, null, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY RESULTSET���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_QUERY_RESULTSET(StackFrame sf, object instance, string logText)
        {
            DB_QUERY_RESULTSET(sf, instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB QUERY RESULTSET���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_QUERY_RESULTSET(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.DBAccess | EventCode.Trace, ErrorLevel.Normal,
                0, "DB QUERY RESULTSET", logText, logParams);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��DB ERR


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_ERR(object instance, Exception exception)
        {
            DB_ERR(GetStackFrame(), instance, exception, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_ERR(object instance, Exception exception,
            params ParameterInfo[] logParams)
        {
            DB_ERR(GetStackFrame(), instance, exception, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB ERR���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_ERR(StackFrame sf, object instance,
            Exception exception, params ParameterInfo[] logParams)
        {
            ExceptionWrite(sf, EventCode.Trace, instance,
                "DB ERR", exception, logParams);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��DB ERR


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_ERR(object instance)
        {
            DB_ERR(GetStackFrame(), instance, string.Empty);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void DB_ERR(object instance, string logText)
        {
            DB_ERR(GetStackFrame(), instance, logText);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB ERR���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_ERR(StackFrame sf, object instance, string logText)
        {
            DB_ERR(sf, instance, logText, (ParameterInfo[])null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// DB ERR���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void DB_ERR(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.DBAccess | EventCode.Trace | EventCode.Error,
                ErrorLevel.Fatal, 0, "DB ERR", logText, logParams);
        }


        #endregion
    }
}
