using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace YQ.Tool.JY
{
    public class JYHelper
    {
        private TcpClient tclient = new TcpClient();
        private NetworkStream ns1;
        private NetworkStream ns2;
        private int sAddr;
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool JYConnet(string IP,int port)
        {
            try
            {
                if (tclient != null)
                {
                    tclient.Close();
                    tclient = new TcpClient();
                }
                if (!tclient.Connected)
                {
                    tclient.Connect(IP, port);
                }
                return true;
            }
            catch (Exception)
            {
                tclient.Close();
                tclient = new TcpClient();
                return false;
            }         
        }
        public void OpenDO(int addr, int io)
        {
            byte[] info = CModbusDll.WriteDO(addr, io, true);
        }
        public void CloseAllDO(int addr,int io)
        {
            CModbusDll.WriteAllDO(addr, io, false);
        }
        /// <summary>
        /// 读取DO状态
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="DONum"></param>
        public byte[] ReadDO(int addr,int DONum)
        {
            sAddr = addr;
            byte[] info = CModbusDll.ReadDO(addr,DONum);
            byte[] rst = sendinfo(info);
            return rst;
        }
        private byte[] sendinfo(byte[] info)
        {
            if (tclient.Connected == false) return null;
            tclient.SendTimeout = 1000;
            try
            {
                ns1.Write(info, 0, info.Length);
                LogService.Instance.Info("发送:"+ info);

                byte[] data = new byte[2048];
                ns2.ReadTimeout = 2000;
                int len = ns2.Read(data, 0, 2048);
                LogService.Instance.Info("接收:" + data);

                return analysisRcv(data, len);
            }
            catch (Exception ex)
            {
                tclient.Close();
                tclient = new TcpClient();
            }
            return null;
        }
        private byte[] analysisRcv(byte[] src, int len)
        {
            if (len < 6) return null;
            if (src[0] != Convert.ToInt16(sAddr)) return null;

            switch (src[1])
            {
                case 0x01:
                    if (CMBRTU.CalculateCrc(src, 6) == 0x00)
                    {
                        byte[] dst = new byte[1];
                        dst[0] = src[3];
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
            }
            return null;
        }
    }
}
