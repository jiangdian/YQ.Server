using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace YQ.Tool.UDP
{
    #region 自定义事件
    /// <summary>
    /// 数据接收事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="data"></param>
    public delegate void DataReceivedHandler(byte[] data, IPEndPoint remote);

    /// <summary>
    /// 发送数据事件
    /// </summary>
    /// <param name="data"></param>
    public delegate bool DataSendHandler(byte[] data);

    ///// <summary>
    ///// 捕获到IP数据包
    ///// 作者：Maximus Ye
    ///// 添加时间：2013年9月16日
    ///// </summary>
    ///// <param name="packet"></param>
    //public delegate void PacketReceived(IPPacket packet);
    #endregion

    public class UDPServer
    {
        /// <summary>
        /// UDP服务端监听
        /// </summary>
        LeafUDPClient udpserver = new LeafUDPClient();
        BindingList<string> lstClient = new BindingList<string>();

        public event DataReceivedHandler DataReceived;
        public event DataSendHandler DataSended;
        /// <summary>
        /// 监听状态
        /// </summary>
        bool isListen = false;

        /// <summary>
        /// 开启UDP监听
        /// </summary>
        /// <returns></returns>
        public void StartUDPServer()
        {
            try
            {
                IPEndPoint ipLocalEndPoint;
                ipLocalEndPoint = new IPEndPoint(IPAddress.Any, 20000);

                udpserver.NetWork = new UdpClient(ipLocalEndPoint);
                udpserver.ipLocalEndPoint = ipLocalEndPoint;
                udpserver.NetWork.BeginReceive(new AsyncCallback(ReceiveCallback), udpserver);
                isListen = true;
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("[LOG]UDPStart Error！", ex);
            }
        }

        public void SendData(byte[] data, IPEndPoint remote)
        {
        //    Console.WriteLine("333");
            udpserver.NetWork.Send(data, data.Length, remote);
          //  Console.WriteLine("444");
            DataSended?.BeginInvoke(data, null, null);
           // Console.WriteLine("555");
        }

        /// <summary>
        /// 接收到数据
        /// </summary>
        /// <param name="ar"></param>
        public void ReceiveCallback(IAsyncResult ar)
        {
            LeafUDPClient userver = (LeafUDPClient)ar.AsyncState;
            string ConnName = "";
            try
            {
                if (userver.NetWork.Client != null)
                {
                    IPEndPoint fclient = userver.ipLocalEndPoint;
                    Byte[] recdata = userver.NetWork.EndReceive(ar, ref fclient);
                    ConnName = userver.ipLocalEndPoint.Port + "->" + fclient.ToString();
                    if (DataReceived != null)
                    {
                        DataReceived.BeginInvoke(recdata, fclient, null, null);//异步输出数据
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("[LOG]UDPReceive Error！", ex);
            }
            finally
            {
                if (userver.NetWork.Client != null)
                {
                    try
                    {
                        userver.NetWork.BeginReceive(new AsyncCallback(ReceiveCallback), userver);//继续异步接收数据
                    }
                    catch (Exception ex)
                    {
                        LogService.Instance.Error("[LOG]UDPReceive Error！", ex);
                    }
                }
            }
        }

        /// <summary>
        /// 停止UDP监听
        /// </summary>
        /// <returns></returns>
        public void StopUDPServer()
        {
            try
            {
                udpserver.NetWork.Close();
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("[LOG]UDPStop Error！", ex);
            }
            lstClient.Clear();
            isListen = false;
        }
    }
}
