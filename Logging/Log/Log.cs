using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Security;

namespace Logging4net
{
	/// <summary>
	/// Log�o�͎��̃p�����[�^���[ParameterID��Value]��ێ�����N���X
	/// </summary>
	public class ParameterInfo
	{
		/// <summary>
		/// �p�����[�^ID
		/// </summary>
		private object _parameterID = null;

		/// <summary>
		/// �p�����[�^ID�ɑ΂����̓I�ȏ��
		/// </summary>
		protected List<object> _valueList = new List<object>();


        /// <summary>
        /// �N���X�̐V�����C���X�^���X����������܂��B
        /// �p�����[�^ID�ƃp�����[�^�̓v���p�e�B�Őݒ肵�Ă��������B
        /// </summary>
        public ParameterInfo()
        {
        }

        /// <summary>
        /// �p�����[�^ID�ƃp�����[�^��w�肵�āA
        /// �N���X�̐V�����C���X�^���X����������܂��B
        /// </summary>
        /// <param name="parameterID">
        /// �p�����[�^ID(���O)��w�肵�܂��B
        /// ������ȊO��w�肷���ToString()�ŕ�����ɕϊ����܂��B
        /// null��w�肷��ƁA���O�t�@�C���ɂ�"*"�ŏo�͂��܂��B</param>
        /// <param name="values">�p�����[�^��w�肵�܂��B
        /// null�ł�悢�ł��B�z�񒆂�null��w�肷�邱�Ƃ�\�ł��B
        /// null��w�肷��ƁA���O�t�@�C���ɂ�string.Empty��o�͂��܂��B</param>
        public ParameterInfo(object parameterID, params object[] values)
        {
            _parameterID = parameterID;
            if (values != null)
            {
                _valueList.AddRange(values);
            }
        }

		/// <summary>
		/// �p�����[�^ID
		/// </summary>
		public object ParameterID
		{
			get
			{
				return _parameterID;
			}
			set
			{
				_parameterID = value;
			}
		}

		/// <summary>
		/// �p�����[�^ID�ɑ΂����̓I�ȏ��
		/// </summary>
		public List<object> ValueList
		{
			get
			{
				return _valueList;
			}
			set
			{
				_valueList = value;
			}
		}
	}
	
	/// <summary>
    /// ���O�o�̓��\�b�h��܂Ƃ߂��N���X�ł��B
    /// </summary>
	public static partial class Log
    {

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
		/// ���O�o�͎��ɐݒ肷��ParameterInfo�N���X�̊ȈՐ������\�b�h
		/// </summary>
        /// <param name="parameterID">
        /// �p�����[�^ID(���O)��w�肵�܂��B
        /// ������ȊO��w�肷���ToString()�ŕ�����ɕϊ����܂��B
        /// null��w�肷��ƁA���O�t�@�C���ɂ�"*"�ŏo�͂��܂��B</param>
        /// <param name="values">�p�����[�^��w�肵�܂��B
        /// null�ł�悢�ł��B�z�񒆂�null��w�肷�邱�Ƃ�\�ł��B
        /// null��w�肷��ƁA���O�t�@�C���ɂ�string.Empty��o�͂��܂��B</param>
        /// <returns>ParameterInfo�N���X</returns>
        public static ParameterInfo CP(object parameterID, params object[] values)
		{
            return new ParameterInfo(parameterID, values);
		}


		//__________________________________________________________________________________________
        #region ���ėp(����𒼐ڎg�p���邱�Ƃ͐������Ȃ��B���O�o�̓��\�b�h��ǉ����鎖�𐄏�����B)


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
		/// �ꎞ�I�Ƀ��\�b�h�쐬�B�S�A�v�����\�b�h�Ή���폜�\��B���̃��\�b�h�͎g�p���Ȃ��ł��������B
        /// </summary>
        /// <param name="eventCode"></param>
        /// <param name="errorLevel"></param>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
		public static void Write(EventCode   eventCode
									,ErrorLevel  errorLevel
									,int         errorCode
									,string      message )
		{
			BaseLog.Write(eventCode, errorLevel, errorCode, message, 3);
		}


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
		/// �ꎞ�I�Ƀ��\�b�h�쐬�B�S�A�v�����\�b�h�Ή���폜�\��B���̃��\�b�h�͎g�p���Ȃ��ł��������B
		/// </summary>
		/// <param name="eventCode"></param>
		/// <param name="errorLevel"></param>
		/// <param name="errorCode"></param>
		/// <param name="message"></param>
		/// <param name="functionFrame"></param>
		public static void Write(EventCode eventCode
								,ErrorLevel errorLevel
								,int errorCode
								,string message
								,int functionFrame)
		{
			BaseLog.Write(eventCode, errorLevel, errorCode, message, functionFrame + 1);
		}


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// ���O��o�͂��܂��B
        /// ����𒼐ڎg�p���邱�Ƃ͐������Ȃ��B���O�o�̓��\�b�h��ǉ����鎖�𐄏�����B
        /// </summary>
        /// <param name="frameCount"></param>
        /// <param name="instance">�C���X�^���X���ʎq</param>
        /// <param name="eventCode"></param>
        /// <param name="errorLevel"></param>
        /// <param name="errorCode"></param>
        /// <param name="logCode"></param>
        /// <param name="logText"></param>
        /// <param name="logParams"></param>
		public static void Write(int frameCount, object instance,
            EventCode eventCode, ErrorLevel errorLevel, int errorCode, string logCode,
			string logText, params ParameterInfo[] logParams)
        {
            BaseLog.Write(
                frameCount + 1,
                instance,
                eventCode, 
                errorLevel, 
                errorCode, 
                logCode, 
                logText, 
                logParams);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ����������o

        
        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// �w���Exception����O�o�͂������ǂ�����Ԃ��܂��B
        /// �o�͂��Ă��Ȃ��ꍇ��true��Ԃ��܂��B�o�͍ς݂̏ꍇ��false��Ԃ��܂��B
        /// </summary>
        /// <param name="exception">���O�o�͑Ώۂ̗�O��w�肵�܂��B</param>
        /// <returns>���ɓo�^�ς݂̏ꍇ��false��Ԃ��܂��B</returns>
        private static bool ExceptionLogWroteEntry(Exception exception)
        {
            lock (_logWroteExceptionQueue)
            {
                if (!_logWroteExceptionQueue.Contains(exception))
                {
                    if (_logWroteExceptionQueue.Count > 5)
                    {
                        _logWroteExceptionQueue.Dequeue();
                    }
                    _logWroteExceptionQueue.Enqueue(exception);

                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// ��O���O�o�̓��\�b�h�ł��B
        /// </summary>
		private static void ExceptionWrite(StackFrame sf, EventCode eventCode,
            object instance, string logCode, Exception exception, params ParameterInfo[] logParams)
        {
            string logText;

            if (exception != null)
            {
                if (ExceptionLogWroteEntry(exception))
                {
                    logText = exception.GetType().FullName;
                }
                else
                {
                    ErrCodeException ecexception = exception as ErrCodeException;
                    if (ecexception != null)
                    {
                        logText = string.Format("[errCode]=[0x{0:X08}, {1}]", ecexception.errCode, ecexception.errCode);
                    }
                    else
                    {
                        logText = exception.ToString();
                    }
                }
            }
            else
            {
                logText = null;
            }

            eventCode |= EventCode.Error;

            BaseLog.Write(sf, instance, eventCode, ErrorLevel.Error, 0,
                logCode, logText, logParams);
        }



        /// <summary>
        /// ���߂Ƀ��O�o�͂�����O��ێ�����L���[�ł��B
        /// </summary>
        private static Queue<Exception> _logWroteExceptionQueue = new Queue<Exception>();


        #endregion


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// �w��̃I�u�W�F�N�g�𕶎���ɕϊ����܂��B
        /// </summary>
        /// <param name="value">�ϊ����̃I�u�W�F�N�g��w�肵�܂��B</param>
        /// <param name="nullText">�I�u�W�F�N�g��null�̏ꍇ�ɕԋp���镶�����w�肵�܂��B</param>
        /// <returns>value.ToString()�œ����������Ԃ��܂��Bnull�̏ꍇ��nullText��Ԃ��܂��B</returns>
        public static string ObjectToString(object value, string nullText)
        {
            if (value == null || value is DBNull)
            {
                return nullText;
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// StackFrame ����擾���܂��BTR*/DB*/REG*/SOCK* �n���̃��x������� StackFrame ��p�ł��B
        /// </summary>
        /// <returns>StackFrame �̃C���X�^���X��Ԃ��܂��B���݂��Ȃ��ꍇ�� null ���Ԃ�܂�</returns>
        [DynamicSecurityMethod]
        internal static StackFrame GetStackFrame()
        {
            return new StackFrame(2, true);
        }
    }



}
