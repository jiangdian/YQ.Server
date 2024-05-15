﻿using System;
using System.Net.Sockets;
using System.Threading;
using YQ.Tool.JY;

namespace JYTest
{
    public class JYHelper
    {
        private TcpClient tclient = new TcpClient();
        private NetworkStream ns1;
        private NetworkStream ns2;
        private int sAddr;
        private int sPort;
        private string sIP;
        private JYHelper()
        {
            JYConnet("192.168.1.232",10000);
        }
        private static readonly object olock=new object();
        private static volatile JYHelper _JYHelper;
        public static JYHelper Instanse()
        {
            if (_JYHelper==null)
            {
                lock (olock)
                {
                    if (_JYHelper == null)
                    {
                        _JYHelper = new JYHelper();
                    }
                }
            }
            return _JYHelper;
        }
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool JYConnet(string IP, int port)
        {
            try
            {
                if (tclient.Connected == false)
                {
                    tclient.Connect(IP, port);
                    ns1 = tclient.GetStream();
                    ns2 = tclient.GetStream();
                    sIP = IP;
                    sPort = port;

                }
                return true;
            }
            catch (Exception ex)
            {
                tclient.Close();
                tclient = new TcpClient();
                return false;
            }

        }
        DateTime conntime = DateTime.Now;
        private void connectClient()
        {
            try
            {
                if (tclient == null)
                {
                    conntime = DateTime.Now;
                    tclient = new TcpClient();
                    tclient.BeginConnect(sIP, sPort, new AsyncCallback(ConnectCallback), tclient);
                    //因为要访问ui资源，所以需要使用invoke方式同步ui。

                }
                else if (tclient.Connected == false)
                {
                    tclient.Connect(sIP, sPort);
                    ns1 = tclient.GetStream();
                    ns2 = tclient.GetStream();
                }
            }
            catch (Exception ex)
            {
                tclient = null;
            }
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                //tclient = (TcpClient)ar.AsyncState;
                if (tclient.Connected == true)
                {
                    ns1 = tclient.GetStream();
                    ns2 = tclient.GetStream();

                }
                else
                    connectClient();
            }
            catch (Exception)
            {
                connectClient();
            }
        }
        private static readonly object _lock=new object();
        public void OpenDO(int addr, int io)
        {
            lock (_lock)
            {
                byte[] info = CModbusDll.WriteDO(addr, io, true);
                sendinfo(info);
            }
          }
        public void OpenAllDO(int addr, int io)
        {
            var info = CModbusDll.WriteAllDO(addr, io, true);
            sendinfo(info);
        }

        public void CloseAllDO(int addr, int io)
        {
            var info = CModbusDll.WriteAllDO(addr, io, false);
            sendinfo(info);
        }
        public void CloseDO(int addr, int io)
        {
            byte[] info = CModbusDll.WriteDO(addr, io, false);
            sendinfo(info);
        }
        /// <summary>
        /// 读取DO状态
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="DONum"></param>
        public byte[] ReadDO(int addr, int DONum)
        {
            sAddr = addr;
            byte[] info = CModbusDll.ReadDO(addr, DONum);
            byte[] rst = sendinfo(info);
            return rst;
        }
        private byte[] sendinfo(byte[] info)
        {
            if (tclient == null)
            {
                connectClient();
                return null;
            }

            if (tclient.Connected == false)
            {
                if (conntime.AddMilliseconds(5 * Convert.ToInt32(100)) < DateTime.Now)
                {
                    tclient.Close();
                    tclient = null;
                }
                return null;
            }
            tclient.SendTimeout = 300;
            tclient.ReceiveTimeout = 300;
            try
            {

                try
                {
                    if (ns1 == null)
                    {
                        ns1 = tclient.GetStream();

                    }
                    ns1.WriteTimeout = 3 + info.Length;
                    ns1.Write(info, 0, info.Length);
                }
                catch (Exception)
                {
                    tclient = null;
                    return null;
                }


                byte[] data = RcvData();
                if (data == null) return null;
                return analysisRcv(data, data.Length);
            }
            catch (Exception)
            {

            }
            return null;
        }
        private byte[] RcvData()
        {
            byte[] info = new byte[2048 + 10];
            int len = 0;
            int retrycnt = 0;
            int timeout = 1000;

            Thread.Sleep(10);
            while (timeout > 0)
            {
                timeout -= 20;
                ns2.ReadTimeout = 5;
                try
                {
                    byte[] rcv = new byte[2048 + 10];
                    if (ns2 == null)
                    {
                        ns2 = tclient.GetStream();
                    }
                    int rdlen = ns2.Read(rcv, 0, 2048);
                    for (int i = 0; i < rdlen; i++)
                    {
                        if (len < 2048) info[len++] = rcv[i];
                    }

                    if (rdlen > 0) retrycnt = 0;
                }
                catch (Exception)
                {

                }
                if (len > 0)
                {
                    retrycnt++;
                    if (retrycnt > 3) timeout = 0;
                }
            }
            if (len < 5) return null;
            byte[] rst = new byte[len];
            for (int i = 0; i < len; i++)
                rst[i] = info[i];
            return rst;
        }
        private byte[] analysisRcv(byte[] src, int len)
        {
            if (len < 6) return null;


            switch (src[1])
            {
                case 0x01:
                    if (CMBRTU.CalculateCrc(src, src[2] + 5) == 0x00)
                    {
                        byte[] dst = new byte[src[2]];
                        for (int i = 0; i < src[2]; i++)
                            dst[i] = src[3 + i];
                        return dst;
                    }
                    break;
                case 0x02:
                    if (CMBRTU.CalculateCrc(src, src[2] + 5) == 0x00)
                    {
                        byte[] dst = new byte[src[2]];
                        for (int i = 0; i < src[2]; i++)
                            dst[i] = src[3 + i];
                        return dst;
                    }
                    break;
                case 0x04:
                    if (CMBRTU.CalculateCrc(src, src[2] + 5) == 0x00)
                    {
                        byte[] dst = new byte[src[2]];
                        for (int i = 0; i < src[2]; i++)
                            dst[i] = src[3 + i];
                        return dst;
                    }
                    break;
                case 0x05:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[1];
                        dst[0] = src[4];
                        return dst;
                    }
                    break;
                case 0x0f:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[1];
                        dst[0] = 1;
                        return dst;
                    }
                    break;
                case 0x06:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[4];
                        dst[0] = src[2];
                        dst[1] = src[3];
                        dst[2] = src[4];
                        dst[3] = src[5];
                        return dst;
                    }
                    break;
                case 0x10:
                    if (CMBRTU.CalculateCrc(src, 8) == 0x00)
                    {
                        byte[] dst = new byte[4];
                        dst[0] = src[2];
                        dst[1] = src[3];
                        dst[2] = src[4];
                        dst[3] = src[5];
                        return dst;
                    }
                    break;
            }
            return null;
        }
    }
}
