using System;
using System.Net.Sockets;
using System.Net;

namespace YQ.Tool.UDP
{
    public class LeafUDPClient
    {
        /// <summary>
        /// 名称
        /// </summary>
        private string _Name = "未定义";
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        IPEndPoint _ipLocalEndPoint;
        /// <summary>
        /// 本地终结点
        /// </summary>
        public IPEndPoint ipLocalEndPoint
        {
            get
            {
                return _ipLocalEndPoint;
            }
            set
            {
                _ipLocalEndPoint = value;
            }
        }

        /// <summary>
        /// UDP 客户端
        /// </summary>
        private UdpClient _NetWork = null;
        public UdpClient NetWork
        {
            get
            {
                return _NetWork;
            }
            set
            {
                _NetWork = value;
            }
        }

        /// <summary>
        /// 断开客户端连接
        /// </summary>
        public void Close()
        {
            try
            {
                if (_NetWork != null && _NetWork.Client.Connected)
                {
                    _NetWork.Close();
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("[LOG]UDPClient Error！", ex);
            }
        }
    }
}
