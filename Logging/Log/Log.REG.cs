using System.Diagnostics;

namespace Logging4net
{
	/// <summary>
	/// ���O�o�̓��\�b�h��܂Ƃ߂��N���X�ł��B
	/// </summary>
	public static partial class Log
	{
        //__________________________________________________________________________________________
        #region ��REG


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REG���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REG(object instance)
        {
            REG(GetStackFrame(), instance, null, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REG���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REG(object instance, string logText)
        {
            REG(GetStackFrame(), instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REG���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void REG(StackFrame sf, object instance, string logText)
        {
            REG(sf, instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REG���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void REG(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.Trace, ErrorLevel.Normal,
                0, "REG", logText, logParams);
        }


        #endregion

        //__________________________________________________________________________________________
        #region ��REG ERR


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REG ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REG_ERR(object instance)
        {
            REG_ERR(GetStackFrame(), instance, null, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REG ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void REG_ERR(object instance, string logText)
        {
            REG_ERR(GetStackFrame(), instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REG ERR���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void REG_ERR(StackFrame sf, object instance, string logText)
        {
            REG_ERR(sf, instance, logText, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// REG ERR���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void REG_ERR(StackFrame sf, object instance, string logText,
            params ParameterInfo[] logParams)
        {
            BaseLog.Write(sf, instance, EventCode.Trace | EventCode.Error,
                ErrorLevel.Normal, 0, "REG ERR", logText, logParams);
        }


        #endregion
    }
}
