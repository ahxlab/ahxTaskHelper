using System;
using System.Diagnostics;

namespace Logging4net
{
	/// <summary>
	/// ���O�o�̓��\�b�h��܂Ƃ߂��N���X�ł��B
	/// </summary>
	public static partial class Log
	{
        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// NOTI SEND���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void NOTI_SEND(object instance, params ParameterInfo[] parameters)
        {
            BaseLog.Write(GetStackFrame(), instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "NOTI SEND", null, parameters);
        }

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// NOTI RECV���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void NOTI_RECV(
            object instance, params ParameterInfo[] parameters)
        {
            BaseLog.Write(GetStackFrame(), instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "NOTI RECV",
                null,
                parameters);
        }

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// NOTI ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void NOTI_ERR(
            object instance, Exception exception)
        {
            ExceptionWrite(GetStackFrame(),
                EventCode.Communication,
                instance,
                "NOTI ERR",
                exception,
                null);
        }
    }
}
