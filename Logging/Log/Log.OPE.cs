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
        #region ��OPERATION


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// OPR ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void OPE(object instance, string actionName)
        {
			OPE_GUI(GetStackFrame(), instance, actionName, null);
        }

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// OPE GUI���O�o�̓��\�b�h�ł��B
        /// </summary>
		public static void OPE_GUI(object instance, string actionName)
        {
			OPE_GUI(GetStackFrame(), instance, actionName, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// OPE GUI���O�o�̓��\�b�h�ł��B
        /// </summary>
		public static void OPE_GUI(object instance, string actionName, string actionParam)
        {
			OPE_GUI(GetStackFrame(), instance, actionName, actionParam);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// OPE GUI���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void OPE_GUI
			(StackFrame sf, object instance, string actionName, string actionParam)
        {
			BaseLog.Write(sf, instance, EventCode.Operation, ErrorLevel.Normal, 0,
                "OPE GUI", actionParam, CP("ACT", actionName));
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��OPE ERR


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// OPE ERR���O�o�̓��\�b�h�ł��B
        /// </summary>
		public static void OPE_ERR(object instance, string actionName, Exception exception)
        {
            OPE_ERR(GetStackFrame(), instance, actionName, null, exception);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// OPE ERR���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
		public static void OPE_ERR(StackFrame sf, object instance,
            string actionName, Exception exception)
        {
			OPE_ERR(sf, instance, actionName, null, exception);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// OPE ERR���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
		public static void OPE_ERR(StackFrame sf, object instance,
            string actionName, string actionParam, Exception exception)
        {
			ExceptionWrite(sf, EventCode.Operation, instance,
                "OPE ERR", exception,
                CP("ACT", actionName),
				CP("VAL", actionParam));
        }


        #endregion

    }
}
