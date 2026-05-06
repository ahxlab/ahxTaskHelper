using System;
using System.Diagnostics;
using System.Text;

namespace Logging4net
{
    /// <summary>
    /// ���O�o�̓��\�b�h��܂Ƃ߂��N���X�ł��B
    /// </summary>
    public static partial class Log
    {
        //__________________________________________________________________________________________
        #region ��SOC LISTEN START


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC LISTEN START ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_LISTEN_START(object instance, object localEndPoint)
        {
            SOC_LISTEN_START(GetStackFrame(), instance, localEndPoint);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC LISTEN START ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_LISTEN_START(StackFrame sf, object instance, object localEndPoint)
        {
            ParameterInfo[] values = new ParameterInfo[]
			{
				CP("LPT", ObjectToString(localEndPoint, string.Empty))
            };

            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "SOC LISTEN START", null, values);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC LISTEN ERR


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC LISTEN ERR ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_LISTEN_ERR(object instance, Exception exception)
        {
            SOC_LISTEN_ERR(GetStackFrame(), instance, exception);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC LISTEN ERR ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_LISTEN_ERR(StackFrame sf, object instance, Exception exception)
        {
            ExceptionWrite(sf, EventCode.Communication, instance,
                "SOC LISTEN ERR", exception, null);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC LISTEN END


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC LISTEN END ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_LISTEN_END(object instance)
        {
            SOC_LISTEN_END(GetStackFrame(), instance);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC LISTEN END ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_LISTEN_END(StackFrame sf, object instance)
        {
            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "SOC LISTEN END", null, null);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC ACCEPT


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC ACCEPT ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_ACCEPT(object instance, object remoteEndPoint, object localEndPoint)
        {
            SOC_ACCEPT(GetStackFrame(), instance, remoteEndPoint, localEndPoint);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC ACCEPT ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_ACCEPT(StackFrame sf,
            object instance, object remoteEndPoint, object localEndPoint)
        {
            ParameterInfo[] values = new ParameterInfo[]
            {
				CP("RPT", ObjectToString(remoteEndPoint, string.Empty)),
                CP("LPT", ObjectToString(localEndPoint, string.Empty))
            };

            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "SOC ACCEPT", null, values);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC ACCEPT CANCEL


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC ACCEPT CANCEL ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_ACCEPT_CANCEL(object instance,
            object remoteEndPoint, object localEndPoint)
        {
            SOC_ACCEPT_CANCEL(GetStackFrame(), instance, remoteEndPoint, localEndPoint);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC ACCEPT ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_ACCEPT_CANCEL(StackFrame sf,
            object instance, object remoteEndPoint, object localEndPoint)
        {
            ParameterInfo[] values = new ParameterInfo[]
            {
                CP("RPT", ObjectToString(remoteEndPoint, string.Empty)),
                CP("LPT", ObjectToString(localEndPoint, string.Empty))
            };

            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "SOC ACCEPT CANCEL", null, values);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC ACCEPT ERR


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC ACCEPT ERR ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_ACCEPT_ERR(object instance, Exception exception)
        {
            SOC_ACCEPT_ERR(GetStackFrame(), instance, exception);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC ACCEPT ERR ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_ACCEPT_ERR(StackFrame sf, object instance, Exception exception)
        {
            ExceptionWrite(sf, EventCode.Communication, instance,
                "SOC ACCEPT ERR", exception, null);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC CONNECT


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC CONNECT ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_CONNECT(object instance, object remoteEndPoint, object localEndPoint)
        {
            SOC_CONNECT(GetStackFrame(), instance, remoteEndPoint, localEndPoint);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC CONNECT ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_CONNECT(StackFrame sf,
            object instance, object remoteEndPoint, object localEndPoint)
        {
            ParameterInfo[] values = new ParameterInfo[]
            {
                CP("RPT", ObjectToString(remoteEndPoint, string.Empty)),
                CP("LPT", ObjectToString(localEndPoint, string.Empty))
            };

            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "SOC CONNECT", null, values);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC CONNECT ERR


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC CONNECT ERR ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_CONNECT_ERR(object instance, Exception exception)
        {
            SOC_CONNECT_ERR(GetStackFrame(), instance, exception);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC CONNECT ERR ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_CONNECT_ERR(StackFrame sf, object instance, Exception exception)
        {
            ExceptionWrite(sf, EventCode.Communication, instance,
                "SOC CONNECT ERR", exception, null);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC DISCONNECT


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC DISCONNECT ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_DISCONNECT(object instance)
        {
            SOC_DISCONNECT(GetStackFrame(), instance);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC DISCONNECT ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_DISCONNECT(StackFrame sf, object instance)
        {
            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "SOC DISCONNECT", null, null);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC OPEN


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC OPEN ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_OPEN(object instance, object remoteEndPoint, object localEndPoint)
        {
            SOC_OPEN(GetStackFrame(), instance, remoteEndPoint, localEndPoint);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC OPEN ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_OPEN(StackFrame sf,
            object instance, object remoteEndPoint, object localEndPoint)
        {
            ParameterInfo[] values = new ParameterInfo[]
            {
                CP("RPT", ObjectToString(remoteEndPoint, string.Empty)),
                CP("LPT", ObjectToString(localEndPoint, string.Empty))
            };

            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "SOC OPEN", null, values);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC CLOSE


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC CLOSE ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_CLOSE(object instance, string closeFactor)
        {
            SOC_CLOSE(GetStackFrame(), instance, closeFactor);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC CLOSE ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_CLOSE(StackFrame sf, object instance, string closeFactor)
        {
            ParameterInfo[] values = new ParameterInfo[]
            {
                CP("FCT", closeFactor)
            };

            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "SOC CLOSE", null, values);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC RECV ERR


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC RECV ERR ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_RECV_ERR(object instance, Exception exception)
        {
            SOC_RECV_ERR(GetStackFrame(), instance, exception, null, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC RECV ERR ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_RECV_ERR(object instance, Exception exception,
            string recvBufHexString, string recvBufCharaString)
        {
            SOC_RECV_ERR(GetStackFrame(), instance, exception, recvBufHexString, recvBufCharaString);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC RECV ERR ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_RECV_ERR(StackFrame sf, object instance, Exception exception)
        {
            SOC_RECV_ERR(sf, instance, exception, null, null);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC RECV ERR ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_RECV_ERR(StackFrame sf, object instance, Exception exception,
            string recvBufHexString, string recvBufCharaString)
        {
            int valueLength = 0;
            if (recvBufHexString != null)
            {
                valueLength++;
            }
            if (recvBufCharaString != null)
            {
                valueLength++;
            }

            ParameterInfo[] values = new ParameterInfo[valueLength];
            valueLength = 0;

            if (recvBufHexString != null)
            {
                values[valueLength] = CP("BUF(HEX)", recvBufHexString);
                valueLength++;
            }

            if (recvBufCharaString != null)
            {
                values[valueLength] = CP("BUF(TXT)", recvBufCharaString);
                valueLength++;
            }

            ExceptionWrite(sf, EventCode.Communication, instance,
                "SOC RECV ERR", exception, values);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC SEND ERR


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC SEND ERR ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_SEND_ERR(object instance, Exception exception)
        {
            SOC_SEND_ERR(GetStackFrame(), instance, exception);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC SEND ERR ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_SEND_ERR(StackFrame sf, object instance, Exception exception)
        {
            ExceptionWrite(sf, EventCode.Communication, instance,
                "SOC SEND ERR", exception, null);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC SEND INFO


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC SEND INFO ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_SEND_INFO(object instance, object info)
        {
            SOC_SEND_INFO(GetStackFrame(), instance, info);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC SEND INFO ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_SEND_INFO(StackFrame sf, object instance, object info)
        {
            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "SOC SEND INFO", ObjectToString(info, string.Empty), null);
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC SEND


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC SEND ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_SEND(object instance, object tcpClient, int size, string hexString, string charaString)
        {
            SOC_SEND(GetStackFrame(), instance, size, hexString, charaString, null);
        }
        /// <summary>
        /// SOC SEND ���O�o�̓��\�b�h�ł��B
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="tcpClient"></param>
        /// <param name="size"></param>
        /// <param name="hexString"></param>
        /// <param name="charaString"></param>
        /// <param name="tcpC"></param>
        public static void SOC_SEND(object instance, object tcpClient, int size, string hexString, string charaString, System.Net.Sockets.TcpClient tcpC)
        {
            SOC_SEND(GetStackFrame(), instance, size, hexString, charaString, tcpC);
        }

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC SEND ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_SEND(StackFrame sf, object instance, int size,
            string hexString, string charaString, System.Net.Sockets.TcpClient tcpClient)
        {
            string socInfo = "";

            if (tcpClient != null && tcpClient.Client != null)
            {
                socInfo = tcpClient.Client.RemoteEndPoint.ToString();
            }

            if (hexString != null)
            {
                BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                    "SOC SEND", hexString,
                    CP("SIZE", size), CP("soc", socInfo));

                if (charaString != null)
                {
                    BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                        "SOC SEND", string.Format("<{0}>", charaString),
                        CP("LEN", charaString.Length), CP("soc", socInfo));
                }
            }
            else if (charaString != null)
            {
                BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                        "SOC SEND", string.Format("<{0}>", charaString),
                        CP("SIZE", size),
                        CP("LEN", charaString.Length), CP("soc", socInfo));
            }
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC RECV / SOC RECV PROCESSED


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC RECV ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_RECV(object instance, int size,
            string hexString, string charaString, bool isProcessed)
        {
            SOC_RECV(GetStackFrame(), instance, size, hexString, charaString, isProcessed, null);
        }

        /// <summary>
        /// SOC RECV ���O�o�̓��\�b�h���̂Q
        /// </summary>
        public static void SOC_RECV(object instance, int size,
            string hexString, string charaString, bool isProcessed, System.Net.Sockets.TcpClient tcpClient)
        {
            SOC_RECV(GetStackFrame(), instance, size, hexString, charaString, isProcessed, tcpClient);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC RECV ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_RECV(StackFrame sf, object instance, int size,
            string hexString, string charaString, bool isProcessed, System.Net.Sockets.TcpClient tcpClient)
        {
            string socInfo = "";

            if (tcpClient != null && tcpClient.Client != null)
            {
                socInfo = tcpClient.Client.RemoteEndPoint.ToString();
            }

            string logCode;
            if (isProcessed)
            {
                logCode = "SOC RECV";
            }
            else
            {
                logCode = "SOC RECV";
            }

            string charaString2 = null;
            if (charaString != null)
            {
                string inData = charaString;
                charaString2 = System.Text.RegularExpressions.Regex.Replace(inData, @"\p{Cc}", str => string.Format("[{0:X2}]", (byte)str.Value[0]));
            } 
            if (hexString != null)
            {
                BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                    logCode, hexString,
                    CP("SIZE", size), CP("soc", socInfo));

                if (charaString2 != null && charaString != null)
                {
                    BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                        logCode, string.Format("<{0}>", charaString2),
                        CP("LEN", charaString.Length), CP("soc", socInfo));
                }
            }
            else if (charaString2 != null && charaString != null)
            {
                BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                        logCode, string.Format("<{0}>", charaString2),
                        CP("SIZE", size),
                        CP("LEN", charaString.Length), CP("soc", socInfo));
            }
        }


        #endregion


        //__________________________________________________________________________________________
        #region ��SOC RECV INFO
        /// <summary>
        /// ���O���b�Z�[�W�ҏW�p�̕�����
        /// </summary>
        public static StringBuilder LoggingSB = new StringBuilder();

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC RECV INFO ���O�o�̓��\�b�h�ł��B
        /// </summary>
        public static void SOC_RECV_INFO(object instance, object info)
        {
            SOC_RECV_INFO(GetStackFrame(), instance, info);
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// SOC RECV INFO ���O�o�̓��\�b�h�ł��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        public static void SOC_RECV_INFO(StackFrame sf, object instance, object info)
        {
            BaseLog.Write(sf, instance, EventCode.Communication, ErrorLevel.Normal, 0,
                "SOC RECV INFO", ObjectToString(info, string.Empty), null);
        }



        /// <summary>
        /// ���O�o�̓��[�h�̗񋓂ł��B
        /// </summary>
        [Flags]
        public enum LogOutputModeEnum
        {
            /// <summary>
            /// ���O�o�͂��܂���B
            /// </summary>
            None = 0x00,

            /// <summary>
            /// SEND INFO/RECV INFO��o�͂��܂��B
            /// </summary>
            Info = 0x01,

            /// <summary>
            /// �o�C�g�V�[�P���X��HEX�ŏo�͂��܂��B
            /// </summary>
            Hex = 0x02,

            /// <summary>
            /// �o�C�g�V�[�P���X��LogEncoding�v���p�e�B�Ŏw�肵��Encoding��g�p���A������ɕϊ����ďo�͂��܂��B
            /// </summary>
            Chara = 0x04,

            /// <summary>
            /// ���ׂẴ��O��o�͂��܂��B
            /// </summary>
            All = Info | Hex | Chara
        }


        /// <summary>
        /// ���O�o�̓��[�h��i�[���܂��B
        /// </summary>
        //private static LogOutputModeEnum _logOutputMode = LogOutputModeEnum.Hex | LogOutputModeEnum.Info;
        private static LogOutputModeEnum _logOutputMode = LogOutputModeEnum.Hex | LogOutputModeEnum.Chara | LogOutputModeEnum.Info;

        /// <summary>
        /// ���O�o�̓��[�hLogOutputMode.Chara�Ŏg�p����Encoding��i�[���܂��B
        /// </summary>
        private static Encoding _logOutputEncoding = Encoding.Default;

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// ���O�o�̓��[�h��擾�܂��͐ݒ肵�܂��B
        /// ����l��LogOutputMode.Hex | LogOutputMode.SendInfo�ł��B
        /// </summary>
        public static LogOutputModeEnum LogOutputMode
        {
            get
            {
                return _logOutputMode;
            }
            set
            {
                _logOutputMode = value;
            }
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// ���O�o�̓��[�hLogOutputMode.Chara�Ŏg�p����Encoding��i�[���܂��B
        /// ����l��Encoding.Default�ł��B
        /// </summary>
        public static Encoding LogOutputEncoding
        {
            get
            {
                return _logOutputEncoding;
            }
            set
            {
                _logOutputEncoding = value;
            }
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// ����M���O��o�͂��܂��B(�A�v���P�[�V��������Ăяo���Ȃ���)
        /// </summary>
        internal static void SOCCOMLOG(StackFrame sf, object obj, System.Net.Sockets.TcpClient tcpClient, bool isProcessed, byte[] buf, int offset, int length, bool isRecvLog)
        {
            string binaryText = null;
            string charaText = null;

            if ((_logOutputMode & LogOutputModeEnum.Hex) != 0)
            {

                StringBuilder stringBuilder = LoggingSB;
                lock (stringBuilder)
                {
                    stringBuilder.Length = 0;
                    stringBuilder.Append(BitConverter.ToString(buf, offset, length));
                    stringBuilder.Replace('-', ' ');
                    binaryText = stringBuilder.ToString();
                }
            }

            if ((_logOutputMode & LogOutputModeEnum.Chara) != 0)
            {
                StringBuilder stringBuilder = LoggingSB;
                lock (stringBuilder)
                {
                    stringBuilder.Length = 0;
                    stringBuilder.Append(
                        _logOutputEncoding.GetString(buf, offset, length));
                    charaText = stringBuilder.ToString();
                }
            }

            if (isRecvLog)
            {
                Log.SOC_RECV(GetStackFrame(), obj, length, binaryText, charaText, isProcessed, tcpClient);
            }
            else
            {
                Log.SOC_SEND(GetStackFrame(), obj, length, binaryText, charaText, tcpClient);
            }
        }

        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// ��M���O��o�͂��܂��B
        /// </summary>
        public static void SOCRCVLOG(object obj, System.Net.Sockets.TcpClient tcpClient, string strText, string strBinary)
        {
            if (strText != null)
            {
                Log.SOC_RECV(GetStackFrame(), obj, strText.Length, null, string.Format("<{0}>", strText), false, tcpClient);
            }

            if (strBinary != null)
            {
                Byte[] ba = System.Text.Encoding.UTF8.GetBytes(strBinary);
                Log.SOCCOMLOG(GetStackFrame(), obj, tcpClient, true, ba, 0, ba.Length, true);
            }
        }


        //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
        /// <summary>
        /// ���M���O��o�͂��܂��B
        /// </summary>
        public static void SOCSNDLOG(object obj, System.Net.Sockets.TcpClient tcpClient, string strText, string strBinary)
        {
            if (strText != null)
            {
                string inData = strText;
                string outStr = System.Text.RegularExpressions.Regex.Replace(inData, @"\p{Cc}", str => string.Format("[{0:X2}]", (byte)str.Value[0]));
                Log.SOC_SEND(GetStackFrame(), obj, strText.Length, null, outStr, tcpClient);
            }

            if (strBinary != null)
            {
                Byte[] ba = System.Text.Encoding.UTF8.GetBytes(strBinary);
                Log.SOCCOMLOG(GetStackFrame(), obj, tcpClient, true, ba, 0, ba.Length, false);
            }
        }


        /// <summary>
        /// �o�C�i���ϊ��iByte->String�j
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string ba2String(byte[] byteArray, int size)
        {

            string hexString = "";

            if (byteArray == null)
            {
                return "";
            }

            if (byteArray.Length == 0)
            {
                return "";
            }

            for (int ix = 0; ix <= size - 1; ix++)
            {
                hexString = hexString + string.Format("{0:X2}", byteArray[ix]);
            }

            return hexString;

        }


        #endregion
    }
}


