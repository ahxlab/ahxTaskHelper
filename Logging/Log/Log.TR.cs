using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;


namespace Logging4net
{
	/// <summary>
	/// ���O�o�̓��\�b�h��܂Ƃ߂��N���X�ł��B
	/// </summary>
	public static partial class Log
	{


        //__________________________________________________________________________________________
        #region �� TR_IN


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR IN���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR_IN(object instance)
        {
            TR_IN(GetStackFrame(), instance, null, null);
        }

		//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
		/// <summary>
		/// TR IN���O�o�̓��\�b�h�ł��B
		/// </summary>
		public static void TR_IN(object instance, params ParameterInfo[] logParams)
		{
            TR_IN(GetStackFrame(), instance, null, logParams);
		}

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR IN���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR_IN(object instance, string logText)
        {
            TR_IN(GetStackFrame(), instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR IN���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void TR_IN(StackFrame sf, object instance, string logText)
        {
            TR_IN(sf, instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR IN���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void TR_IN(
            StackFrame sf,
            object instance,
            string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.Trace, ErrorLevel.Normal, 0, "TR IN", logText, logParams);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��TR


		//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
		/// <summary>
		/// TR���O�o�̓��\�b�h�ł��B
		/// </summary>
        public static void TR(object instance, params ParameterInfo[] logParams)
		{
            TR(GetStackFrame(), instance, null, null, logParams);
		}

		//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
		/// <summary>
		/// TR���O�o�̓��\�b�h�ł��B
		/// </summary>
		public static void TR(object instance, string logText, params ParameterInfo[] logParams)
		{
			TR(GetStackFrame(), instance, null, logText, logParams);
		}
		
		//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR(object instance, string actionName)
        {
            TR(GetStackFrame(), instance, actionName, null, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR(object instance, string actionName, string logText)
        {
            TR(GetStackFrame(), instance, actionName, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR(object instance, string actionName, string logText,
            params ParameterInfo[] logParams)
        {
            TR(GetStackFrame(), instance, actionName, logText, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void TR(StackFrame sf, object instance, string actionName, string logText,
            params ParameterInfo[] logParams)
        {
            if (actionName != null)
            {
                ParameterInfo actionNameParam = CP("ACT", actionName);
                if (logParams == null)
                {
                    logParams = new ParameterInfo[] { actionNameParam };
                }
                else
                {
                    List<ParameterInfo> paramList = new List<ParameterInfo>();
                    paramList.Add(actionNameParam);
                    paramList.AddRange(logParams);
                    logParams = paramList.ToArray();
                }
            }

            BaseLog.Write(sf, instance, EventCode.Trace, ErrorLevel.Normal, 0,
                "TR", logText, logParams);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��TR OUT


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR OUT���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR_OUT(object instance)
        {
            TR_OUT(GetStackFrame(), instance, null, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR OUT���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR_OUT(object instance, string logText)
        {
            TR_OUT(GetStackFrame(), instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR OUT���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR_OUT(object instance, params ParameterInfo[] logParams)
        {
            TR_OUT(GetStackFrame(), instance, null, logParams);
        }

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR OUT���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void TR_OUT(int frameCount, object instance, string logText)
        {
            TR_OUT(GetStackFrame(), instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR OUT���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void TR_OUT(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.Trace, ErrorLevel.Normal, 0,
                "TR OUT", logText, logParams);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��TR ERR

        class ErrCodeException : ApplicationException
        {
            public ErrCodeException(int error)
            {
                errCode = error;
            }
            public int errCode = 0;
        }

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR_ERR(string actionName, int errcode)
        {
            ErrCodeException exception = new ErrCodeException(errcode);
            TR_ERR(GetStackFrame(), null, actionName, exception, null);
        }

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR_ERR(string actionName, int errcode,
            params ParameterInfo[] logParams)
        {
            ErrCodeException exception = new ErrCodeException(errcode);
            TR_ERR(GetStackFrame(), null, actionName, exception, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR_ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR_ERR(object instance, string logText, Exception ex, params ParameterInfo[] logParams)
        {
            TR_ERR(GetStackFrame(), instance, logText, null, logParams);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR_ERR(object instance, Exception exception)
        {
            TR_ERR(GetStackFrame(), instance, null, exception, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR_ERR(object instance, Exception exception,
            params ParameterInfo[] logParams)
        {
            TR_ERR(GetStackFrame(), instance, null, exception, logParams);
        }

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR ERR���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void TR_ERR(StackFrame sf, object instance, Exception exception)
        {
            TR_ERR(sf, instance, null, exception, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR_ERR(object instance, string actionName, Exception exception)
        {
            TR_ERR(GetStackFrame(), instance, actionName, exception, null);
        }

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void TR_ERR(StackFrame sf, object instance, string actionName, 
            Exception exception)
        {
            TR_ERR(sf, instance, actionName, exception, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// TR ERR���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void TR_ERR(StackFrame sf, object instance, string actionName,
            Exception exception, params ParameterInfo[] logParams)
        {
            if (actionName != null)
            {
                ParameterInfo actionNameParam = CP("ACT", actionName);
                if (logParams == null)
                {
                    logParams = new ParameterInfo[] { actionNameParam };
                }
                else
                {
                    List<ParameterInfo> paramList = new List<ParameterInfo>();
                    paramList.Add(actionNameParam);
                    paramList.AddRange(logParams);
                    logParams = paramList.ToArray();
                }
            }

            ExceptionWrite(sf, EventCode.Trace, instance,
                "TR ERR", exception, logParams);
		}

		#endregion



		//__________________________________________________________________________________________
		#region ��TR WARNING


		//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
		/// <summary>
		/// TR_WARN���O�o�̓��\�b�h�ł��B
		/// </summary>
		public static void TR_WARN(object instance, params ParameterInfo[] logParams)
		{
            TR_WARN(GetStackFrame(), instance, null, null, logParams);
		}

		//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
		/// <summary>
		/// TR_WARN���O�o�̓��\�b�h�ł��B
		/// </summary>
		public static void TR_WARN(object instance, string logText, params ParameterInfo[] logParams)
		{
            TR_WARN(GetStackFrame(), instance, null, logText, logParams);
		}

		//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
		/// <summary>
		/// TR_WARN���O�o�̓��\�b�h�ł��B
		/// </summary>
		public static void TR_WARN(object instance, string actionName, string logText)
		{
            TR_WARN(GetStackFrame(), instance, actionName, logText, null);
		}

		//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
		/// <summary>
		/// TR_WARN���O�o�̓��\�b�h�ł��B
		/// </summary>
		public static void TR_WARN(object instance, string actionName, string logText,
			params ParameterInfo[] logParams)
		{
            TR_WARN(GetStackFrame(), instance, actionName, logText, logParams);
		}

		//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
		/// <summary>
        /// TR_WARN���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
		/// </summary>
		public static void TR_WARN(StackFrame sf, object instance, string actionName, string logText,
			params ParameterInfo[] logParams)
		{
			if (actionName != null)
			{
				ParameterInfo actionNameParam = CP("ACT", actionName);
				if (logParams == null)
				{
					logParams = new ParameterInfo[] { actionNameParam };
				}
				else
				{
					List<ParameterInfo> paramList = new List<ParameterInfo>();
					paramList.Add(actionNameParam);
					paramList.AddRange(logParams);
					logParams = paramList.ToArray();
				}
			}

			BaseLog.Write(sf, instance, EventCode.Trace | EventCode.Error
				, ErrorLevel.Warning, 0, "TR WARN", logText, logParams);
		}

		#endregion

    }
}
