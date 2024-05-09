using STComm;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace YQ.Tool.PowerSupply
{
    internal class FNHelper : IPowerMethod
    {
        public bool AutoAll_ResetSY()
        {
            throw new NotImplementedException();
        }

        public bool AutoSub(int Meter_No, byte Dev_Port)
        {
            throw new NotImplementedException();
        }

        public bool AutoSub_Reset(int Meter_No, byte Dev_Port)
        {
            throw new NotImplementedException();
        }

        public bool Clock_Error_Read(ref string MError, double TheoryFreq, byte ErrorType, int Meter_No, int Dev_Port)
        {
            throw new NotImplementedException();
        }

        public bool Clock_Error_Start(int Meter_No, double TheoryFreq, int TestTime, int Dev_Port)
        {
            try
            {
                foreach (int kk in PowerHelper.HangPos)
                {
                    CommCtr.FDevComm.SetLocalDisplay(kk, "000000");           //恢复所有表位显示
                    CommCtr.FDevComm.SetPosLight((sbyte)kk, 0, XOnOff.x_Off); //关闭表位状态灯
                };
                Thread.Sleep(1000);
                CommCtr.FDevComm.SetUValueEx("Un");     //设置电压
                CommCtr.FDevComm.SwitchU(XLABC.x_L, XOnOff.x_On);      //打开电压输出开关
                if (TheoryFreq == 0)
                {
                    TheoryFreq = 1;
                }
                if (TestTime > 99 * 60 + 59) //周期最大值
                    TestTime = 99 * 60 + 59;

                CommCtr.FDevComm.SetRefFreqTest(Meter_No, TheoryFreq);       //设置基准频率
                CommCtr.FDevComm.SetTestTime((int)TTestTimeKind.ttkFreqPeriod, Meter_No, TestTime);       //设置测试周期

                //设置为基频测试模式
                CommCtr.FDevComm.SetTestMode(TTestMode.tmTimeTest);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }
       
        public bool ConstPulse_Read(ref int Pulse, ref double StdKWH, double Constant, int Meter_No, int Dev_Port)
        {
            throw new NotImplementedException();
        }

        public bool Count_Read(ref int MPulse, int Meter_No, byte Dev_Port)
        {
            throw new NotImplementedException();
        }

        public bool Count_Start(int Meter_No, bool Active, int Pulse, int TotalTime)
        {
            throw new NotImplementedException();
        }

        public bool ErrorComm(int Dev_Port, int ErrAddr, byte ErrCmd, byte[] CmdData, ref StringBuilder BackData)
        {
            throw new NotImplementedException();
        }

        public bool ErrorComm1(int Dev_Port, int ErrAddr, byte ErrCmd, byte[] CmdData)
        {
            throw new NotImplementedException();
        }

        public bool Error_Clear(int Dev_Port)
        {
            throw new NotImplementedException();
        }

        public bool Error_Read(ref string MError, int Meter_No, int circle, int Dev_Port)
        {
            throw new NotImplementedException();
        }

        public bool Error_Start(int Meter_No, double Constant, int Pulse, int Dev_Port)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 设置节点属性及值
        /// </summary>
        /// <param name="aNode"></param>
        /// <param name="aAttName"></param>
        /// <param name="aValue"></param>
        public static XmlNode SetAttriText(XmlNode aNode, string aAttName, string aValue)
        {
            try
            {
                if (aNode == null)
                    return null;
                else
                {
                    if (aNode.Attributes[aAttName] == null)
                    {
                        XmlNode TemAtt = aNode.OwnerDocument.CreateNode(XmlNodeType.Attribute, aAttName, string.Empty);
                        TemAtt.Value = aValue;
                        aNode.Attributes.SetNamedItem(TemAtt);
                        return TemAtt;
                    }
                    else
                    {
                        aNode.Attributes[aAttName].Value = aValue;
                        return aNode;
                    }
                }
            }
            catch
            {
                //Debug.Write(ex);
                return null;
            }
        }
        /// <summary>
        /// 添加子节点及值
        /// </summary>
        /// <param name="aParentNode"></param>
        /// <param name="aChildName"></param>
        /// <returns></returns>
        public static XmlNode AddChildNode(XmlNode aParentNode, string aChildName, string aChildValue)
        {
            if (aParentNode == null)
                return null;
            XmlNode _node = aParentNode.OwnerDocument.CreateNode(XmlNodeType.Element, aChildName, string.Empty);
            _node.InnerText = aChildValue;
            aParentNode.AppendChild(_node);
            return _node;
        }
        public bool OpenComm(int Port)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"gb2312\"?><XData></XData>");
            XmlNode RootNode = xmlDoc.DocumentElement;
            if (RootNode != null)
            {
                SetAttriText(RootNode, "MainProg", System.Reflection.Assembly.GetEntryAssembly().Location);
                SetAttriText(RootNode, "TwoLoopDev", "false");
                XmlNode tmpNode;
                tmpNode = AddChildNode(RootNode, "Device", ConfigHelper.GetValue("PowerStyle"));//装置型号节点
                SetAttriText(tmpNode, "PVer", "自动判断");
                SetAttriText(tmpNode, "Port", Port.ToString());
                SetAttriText(tmpNode, "PortSetting", "19200,n,8,1");
                SetAttriText(tmpNode, "Protocol", "PSD5V4.dll");
                tmpNode = AddChildNode(RootNode, "VMult", "");//虚拟串口节点
                try
                {
                    int rtn = CommCtr.FDevComm.ConfigCommEx(xmlDoc.OuterXml);
                    if (rtn != 0) //新的初始化函数
                    {
                        CommCtr.FDevComm.GetLastErr(out string ws);
                        MessageBox.Show(ws);
                    }
                    CommCtr.FDevComm.SetTestMode(TTestMode.tmStartPC);// 设置PC控制模式   
                    CommCtr.FDevComm.SetMaxPosition(Convert.ToInt32(ConfigHelper.GetValue("MeterNum")), 0); //设置表位总数
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("端口打开异常,原因：{0}", ex.Message));
                    return false;
                }
            }
            return true;
        }

        public bool Power_Off(int Dev_Port)
        {
            try
            {
                CommCtr.FDevComm.SwitchU(XLABC.x_L, XOnOff.x_Off);
                return true;
            }
            catch (Exception)
            {
                return false;
            }   
        }

        public bool Power_On(PowerParam power)
        {
            try
            {
                CommCtr.FDevComm.SwitchU(XLABC.x_L, XOnOff.x_Off);
                Thread.Sleep(1000);
                CommCtr.FDevComm.SetUValue(power.Ua, false);  //设置电压值为220V
                CommCtr.FDevComm.SwitchU(XLABC.x_L, XOnOff.x_On);

                //CommCtr.FDevComm.GetPwrSetting(XLABC.x_L, TSettingType.sttUOnOff,out object value);
                //if ((XOnOff)value==XOnOff.x_Off)
                //{
                //    CommCtr.FDevComm.SwitchU(XLABC.x_L, XOnOff.x_On);
                //}
                //CommCtr.FDevComm.SetUValue(220, false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SetRefClock(byte SetFlag, int Dev_Port)
        {
            throw new NotImplementedException();
        }

        public bool SetStdFreq(double StdFreq)
        {
            throw new NotImplementedException();
        }

        public bool Set_Harmonic_Data(double[] HAng, int[] HNum, double[] HVolt, double[] HCur)
        {
            throw new NotImplementedException();
        }

        public bool Set_Pulse_Channel(int Meter_No, int Channel_Flag, int Dev_Port)
        {
            throw new NotImplementedException();
        }

        public PowerParam StdMeter_Read()
        {
            double U1, U2, U3;
            bool result=CommCtr.FDevComm.GetU(out U1, out U2, out U3, false);
            LogService.Instance.Info($"HSHelper.StdMeter_Read:{U1},{U2},{U3}");
            PowerParam power = new PowerParam(); ;
            //if (result)//返回true，但没有结果值
            //{
                power.Ua = U1;
                power.Ub = U2;
                power.Uc = U3;
                power.Result = true;
            //}
            //else
            //{
            //    power.Result = false;
            //}
            double I1, I2, I3;//电流
            CommCtr.FDevComm.GetI(out I1, out I2, out I3, false);
            power.Ia = I1;
            power.Ib = I2;
            power.Ic = I3;

            double val2, val3, val4, val5, val6;
            CommCtr.FDevComm.GetPhase(out val2, out val3, out val4, out val5, out val6);
            power.Ua_Phase = 0;
            power.Ub_Phase = val2;
            power.Uc_Phase = val3;
            power.Ia_Phase = val4;
            power.Ib_Phase = val5;
            power.Ic_Phase = val6;

            double P, P1, P2, P3;
            CommCtr.FDevComm.GetP(out P, out P1, out P2, out P3);
            power.P = P;

            double Q, Q1, Q2, Q3;
            CommCtr.FDevComm.GetQ(out Q, out Q1, out Q2, out Q3);
            power.Q = Q;

            double freq;
            CommCtr.FDevComm.GetFreq(out freq);
            power.Frequency = freq;

            //double pf;
            //CommCtr.FDevComm.GetPF(out string pf);
            power.FP = 1;
            return power;
        }

        public bool Stop_Test(int Dev_Port)
        {
            throw new NotImplementedException();
        }
    }
}
