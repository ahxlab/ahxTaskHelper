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
        #region ��APP START/EXIT


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// APP START���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void APP_START()
        {
			BaseLog.Write(GetStackFrame(), null, EventCode.Trace, ErrorLevel.Normal, 0, "APP START", null
				, CP("CommandLine", Environment.CommandLine));
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// APP EXIT���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void APP_EXIT()
        {
			BaseLog.Write(GetStackFrame(), null, EventCode.Trace, ErrorLevel.Normal, 0,
                "APP EXIT", null, null);
		}


        #endregion


        //__________________________________________________________________________________________
        #region ��APP CONF READ/ERR


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
		/// <summary>
		/// APP CONF READ���O�o�̓��\�b�h�ł��B
		/// </summary>
		public static void APP_CONF_READ(object instance, string configXml)
        {
            BaseLog.Write(GetStackFrame(), null, EventCode.Trace, ErrorLevel.Normal, 0,
                "APP CONF READ", configXml, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// APP CONF READ ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
		public static void APP_CONF_READ_ERR(object instance, Exception exception)
        {
            ExceptionWrite(GetStackFrame(), EventCode.Trace, instance,
                "APP CONF READ ERR", exception, null);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��APP REMCONF READ/ERR


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// APP REMCONF READ���O�o�̓��\�b�h�ł��B
        /// </summary>
		public static void APP_REMCONF_READ(object instance, string configXml)
        {
            BaseLog.Write(GetStackFrame(), null, EventCode.Trace, ErrorLevel.Normal, 0,
                "APP REMCONF READ", configXml, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// APP REMCONF READ ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
		public static void APP_REMCONF_READ_ERR(object instance, Exception exception)
        {
            ExceptionWrite(GetStackFrame(), EventCode.Trace, instance,
                "APP REMCONF READ ERR", exception, null);
        }


        #endregion

    }

}
