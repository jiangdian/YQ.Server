﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace YQ.Tool
{
    /// <summary>
    /// 项目持久化串口管理类
    /// </summary>
    public class SerialManager
    {
        #region private属性
        private static readonly object lockobj = new object();//线程锁
        private static SerialManager instance;
        private Dictionary<string, SerialPort> portList;

        #endregion

        #region public属性

        /// <summary>
        /// 串口列表
        /// </summary>
        public Dictionary<string, SerialPort> PortList => portList;

        /// <summary>
        /// 串口管理类实例
        /// </summary>
        public static SerialManager Instance => GetInstance();

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        private SerialManager()
        {
            portList = new Dictionary<string, SerialPort>();
            // ShengDiHelper.SetDev_Port((byte)LocalSeting.powercom);//config read
        }
        #endregion

        public static SerialManager GetInstance()
        {
            if (instance == null)
            {
                lock (lockobj)
                {
                    if (instance == null)
                    {
                        instance = new SerialManager();
                    }
                }
            }
            return instance;
        }

        public void ClosePorts()
        {
            lock (lockobj)
            {
                foreach (SerialPort port in this.PortList.Values)
                {
                    try
                    {
                        port.Close();
                    }
                    catch (Exception)
                    {
                    }
                }
                this.PortList.Clear();
                instance = null;
            }
        }

        public void ClosePort(string portName)
        {
            lock (lockobj)
            {
                if (string.IsNullOrEmpty(portName))
                {
                    return;
                }
                portName = portName.ToUpper();
                try
                {
                    if (this.PortList.Keys.Contains(portName))
                    {
                        SerialPort port = this.PortList[portName];
                        port.Close();
                    }
                }
                catch (Exception)
                {
                }
                finally { try { this.PortList.Remove(portName); } catch { } }
            }
        }


        /// <summary>
        /// 创建并打开串口，或返回已创建的串口
        /// </summary>
        /// <param name="com"></param>
        /// <returns></returns>
        public SerialPort CreateAndOpenPort(ComParamter com)
        {
            return CreateAndOpenPort(com.PortName, com.BaudRate, com.Parity, com.DataBits, com.StopBits);
        }

        /// <summary>
        /// 创建并打开串口，或返回已创建的串口
        /// </summary>
        /// <param name="portNo">串口号</param>
        /// <param name="baudRate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <returns></returns>
        /// <returns></returns>
        public SerialPort CreateAndOpenPort(int portNo, int baudRate = 2400, Parity parity = Parity.Even, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            return CreateAndOpenPort("COM" + portNo, baudRate, parity, dataBits, stopBits);
        }

        /// <summary>
        /// 创建并打开串口，或返回已创建的串口
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate"></param>
        /// <param name="parity"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <returns></returns>
        public SerialPort CreateAndOpenPort(string portName, int baudRate = 2400, Parity parity = Parity.Even, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            if (string.IsNullOrEmpty(portName))
            {
                return null;
            }
            SerialPort port;
            lock (lockobj)
            {
                portName = portName.ToUpper();
                if (this.PortList.Keys.Contains(portName))
                {
                    port = this.PortList[portName];
                }
                else
                {
                    port = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
                    port.RtsEnable = false;
                    try
                    {
                        port.Open();
                    }
                    catch (Exception e)
                    {
                        LogService.Instance.Error("创建串口失败！");
                        throw new Exception();
                        return null;
                    }
                    this.PortList.Add(portName, port);
                }
            }
            return port;
        }


        /// <summary>
        /// 获取串口
        /// </summary>
        /// <param name="portName"></param>
        /// <returns></returns>
        private SerialPort GetPort(string portName)
        {
            if (string.IsNullOrEmpty(portName))
            {
                return null;
            }
            lock (lockobj)
            {
                portName = portName.ToUpper();
                SerialPort port;
                if (!string.IsNullOrEmpty(portName) && this.PortList.Keys.Contains(portName))
                {
                    port = this.PortList[portName];
                }
                else
                {
                    port = null;
                    LogService.Instance.Error($"串口{portName}未实例化!");
                }
                return port;
            }
        }

        /// <summary>
        /// 查询串口状态，如果关闭，尝试打开串口
        /// </summary>
        /// <param name="portName"></param>
        /// <param name="trytimes">尝试次数，默认1次，等待时间：100ms/次 </param>
        /// <returns></returns>
        public bool IsOpen(string portName, int trytimes = 1)
        {
            if (string.IsNullOrEmpty(portName))
            {
                return false;
            }
            lock (lockobj)
            {
                bool rlt = false;
                do
                {
                    SerialPort port = GetPort(portName);
                    if (port == null)
                    {
                        return false;
                    }
                    rlt = port.IsOpen;
                    if (rlt)
                    {
                        break;
                    }
                    else
                    {
                        try
                        {
                            port.Open();
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                    trytimes--;
                } while (trytimes > 0);
                return rlt;
            }
        }
        /// <summary>
        /// 发送命令，并获取应答
        /// </summary>
        /// <param name="portNo">串口号，例如：1</param>
        /// <param name="sendData">发送数据</param>
        /// <param name="sendTimes">尝试发送次数</param>
        /// <param name="rcvTimes">每次发送后等待接收次数</param>
        /// <param name="rcvSleep">每次接收前等待时间</param>
        /// <param name="hopeRcvLen">期望得到的应答数据长度</param>
        /// <returns></returns>
        public byte[] SendCommand(int portNo, byte[] sendData, int sendTimes, int rcvTimes, int rcvSleep, int hopeRcvLen)
        {
            return SendCommand("COM" + portNo, sendData, sendTimes, rcvTimes, rcvSleep, hopeRcvLen);
        }

        /// <summary>
        /// 发送命令，并获取应答
        /// </summary>
        /// <param name="portName">串口名，例如：COM1</param>
        /// <param name="sendData">发送数据</param>
        /// <param name="sendTimes">尝试发送次数</param>
        /// <param name="rcvTimes">每次发送后等待接收次数</param>
        /// <param name="rcvSleep">每次接收前等待时间</param>
        /// <param name="hopeRcvLen">期望得到的应答数据长度</param>
        /// <returns></returns>
        public byte[] SendCommand(string portName, byte[] sendData, int sendTimes, int rcvTimes, int rcvSleep, int hopeRcvLen)
        {
            List<byte> rcvData = null;
            SerialPort port = this.GetPort(portName);
            if (rcvTimes <= 0)
            {
                rcvTimes = 1;
            }
            if (port.SendCommand(sendData, sendTimes, ref rcvData, rcvTimes, rcvSleep, hopeRcvLen) > 0)
            {
                return rcvData.ToArray();
            }
            else
            {
                return null;
            }
        }

        ~SerialManager()
        {
            ClosePorts();
        }
    }
}
