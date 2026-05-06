using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Logging4net
{
    /// <summary>
    /// .NET Remoting�̃��\�b�h�Ăяo���A���\�b�h��t���O�o�̓��\�b�h��܂Ƃ߂��N���X�ł��B
    /// </summary>
	public static partial class Log
	{


        //__________________________________________________________________________________________
        #region ��REM CONF READ


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM CONF READ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_CONF_READ(object instance, string configText)
        {
            REM_CONF_READ(GetStackFrame(), instance, configText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM CONF READ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_CONF_READ(
            object instance, string configText, params ParameterInfo[] logParams)
        {
            REM_CONF_READ(GetStackFrame(), instance, configText, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM CONF READ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_CONF_READ(StackFrame sf, object instance, string configText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "REM CONF READ", configText, logParams);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��REM CALL


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM CALL���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_CALL(object instance, string methodName)
        {
            REM_CALL(GetStackFrame(), instance, methodName, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM CALL���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_CALL(object instance, params ParameterInfo[] logParams)
        {
            REM_CALL(GetStackFrame(), instance, null, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM CALL���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_CALL(object instance, string methodName, 
            params ParameterInfo[] logParams)
        {
            REM_CALL(GetStackFrame(), instance, methodName, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM CALL���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_CALL(StackFrame sf, object instance, string methodName,
            params ParameterInfo[] logParams)
        {
            if (methodName != null)
            {
                ParameterInfo methodNameParam = CP("FUNC", methodName);
                if (logParams == null)
                {
                    logParams = new ParameterInfo[] { methodNameParam };
                }
                else
                {
                    List<ParameterInfo> paramList = new List<ParameterInfo>();
                    paramList.Add(methodNameParam);
                    paramList.AddRange(logParams);
                    logParams = paramList.ToArray();
                }
            }

            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "REM CALL", null, logParams);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��REM RET


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM RET���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_RET(object instance)
        {
            REM_RET(GetStackFrame(), instance, null, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM RET���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_RET(object instance, params ParameterInfo[] logParams)
        {
            REM_RET(GetStackFrame(), instance, null, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM RET���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_RET(object instance, string logText)
        {
            REM_RET(GetStackFrame(), instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM RET���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_RET(object instance, string logText
            , params ParameterInfo[] logParams)
        {
            REM_RET(GetStackFrame(), instance, logText, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM RET���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_RET(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "REM RET", logText, logParams);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��REM IN


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM IN���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_IN(object instance)
        {
            REM_IN(GetStackFrame(), instance, null, null, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM IN���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_IN(object instance, params ParameterInfo[] logParams)
        {
            REM_IN(GetStackFrame(), instance, null, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM IN���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_IN(object instance, string logText)
        {
            REM_IN(GetStackFrame(), instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM IN���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_IN(object instance, string logText,
            params ParameterInfo[] logParams)
        {
            REM_IN(GetStackFrame(), instance, logText, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM IN���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_IN(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "REM IN", logText, logParams);
        }


        #endregion



        //__________________________________________________________________________________________
        #region ��REM OUT


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM OUT���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_OUT(object instance)
        {
            REM_OUT(GetStackFrame(), instance, null, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM OUT���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_OUT(object instance, params ParameterInfo[] logParams)
        {
            REM_OUT(GetStackFrame(), instance, null, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM OUT���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_OUT(object instance, string logText)
        {
            REM_OUT(GetStackFrame(), instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM OUT���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_OUT(object instance, string logText
            , params ParameterInfo[] logParams)
        {
            REM_OUT(GetStackFrame(), instance, logText, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM OUT���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_OUT(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "REM OUT", logText, logParams);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��REM ERR


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_ERR(object instance, Exception exception)
        {
            REM_ERR(GetStackFrame(), instance, exception, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_ERR(object instance, Exception exception,
            params ParameterInfo[] actionParams)
        {
            REM_ERR(GetStackFrame(), instance, exception, actionParams);
        }



        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REM ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REM_ERR(StackFrame sf, object instance, 
            Exception exception, params ParameterInfo[] actionParams)
        {
			ExceptionWrite(sf, EventCode.Trace, instance,
				"REM ERR", exception, actionParams);
        }


        #endregion
    }

}
