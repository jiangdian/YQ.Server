using STComm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using YQ.Tool.PowerSupply;

namespace YQ.Tool
{

    public static class PowerHelper
    {
        /// <summary>
        /// 串口号，默认2
        /// </summary>
        public static byte Dev_Port = 1;
        public static double Meter_constant = 400;

        /// <summary>
        /// 标准表常数
        /// 盛迪在电流改变后标准表常数也会改变，所以在读取走字结果前要重新设置标准表常数
        /// </summary>
        public static double Std_consant;
        /// <summary>
        /// 标准表常数
        /// </summary>
        public static double PowerI_flag = 1;

        /// <summary>
        /// 当前接线方式
        /// </summary>
        public static int Phase = 0;

        /// <summary>
        /// 型号 TC-3000C
        /// </summary>
        public static String SModel = "TC-3000C";

        public static IPowerMethod powerMethod;
        /// <summary>
        /// 正在上源
        /// </summary>
        public static bool IsPowering = false;

        public static List<int> HangPos=new List<int>();

        public static Dictionary<int, ClockHelper> dicTimes = new Dictionary<int, ClockHelper>()
        {
            {1,new ClockHelper() },
            {2,new ClockHelper() },
            {3,new ClockHelper() },
            {4,new ClockHelper() },
            {5,new ClockHelper() },
            {6,new ClockHelper() },
            {7,new ClockHelper() },
            {8,new ClockHelper() },
            {9,new ClockHelper() },
            {10,new ClockHelper() },
            {11,new ClockHelper() },
            {12,new ClockHelper() }
        };

        /// <summary>
        /// 设置端口号
        /// </summary>
        /// <param name="dev_Port"></param>
        public static void SetDev_Port(byte dev_Port)
        {
            PowerHelper.Dev_Port = dev_Port;
            GetpowerMethod().OpenComm(PowerHelper.Dev_Port);
        }

        public static void SetSmodel()
        {
            PowerHelper.SModel = ConfigHelper.GetValue("PowerStyle");
        }

        public static Action<string> OnShowPowerStatus;

        /// <summary>
        /// 获取功率源实例
        /// </summary>
        /// <returns></returns>
        private static IPowerMethod GetpowerMethod()
        {
            if (powerMethod != null)
            {
                return powerMethod;
            }
            powerMethod = new FNHelper();
            //switch (config.PowerCategory)
            //{
            //    case 1:
            //        powerMethod = new HSHelper();
            //        break;
            //    case 2:
            //        powerMethod = new SYHelper();
            //        break;
            //    //case 3:
            //    //    powerMethod = new HPHelper();
            //    //    break;
            //    case 4:
            //        powerMethod = new KREHelper();
            //        break;
            //    default:
            //        powerMethod = new HSHelper();
            //        break;
            //}
            return powerMethod;
        }
        ///// <summary>
        ///// 根据表相限和计量方向得到接线方式
        ///// </summary>
        ///// <param name="mh">表相限，0：单相，1：三相三线，2：三相四线</param>
        ///// <param name="cp">计量方向，0：正向有功，1：反向有功，2：正向无功，3：反向无功</param>
        ///// <returns></returns>
        //private static TConnectType GetConnectType(int mh, int cp)
        //{
        //    TConnectType ct = TConnectType.cnP1;
        //    if (cp == 0 || cp == 1) //有功
        //    {
        //        switch (mh)
        //        {
        //            case 0: //单相
        //                ct = TConnectType.cnP1;
        //                break;
        //            case 1: //三相三线
        //                ct = TConnectType.cnP3;
        //                break;
        //            case 2: //三相四线
        //                ct = TConnectType.cnP4;
        //                break;
        //        }

        //    }
        //    return ct;
        //}

        /// <summary>
        /// 设置装置的相关参数
        /// </summary>
        /// <returns></returns>
        public static bool InitFNParams()
        {
            try
            {
                CommCtr.FDevComm.SetTestMode(TTestMode.tmStartPC);// 设置PC控制模式   
                CommCtr.FDevComm.SetMaxPosition(Convert.ToInt32(ConfigHelper.GetValue("MeterNum")), 0); //设置表位总数
                #region 电源量程设置
                //string[] UL = ConfigHelper.GetValue("tbURanges").Split((','));
                //string[] IL = ConfigHelper.GetValue("tbIRanges").Split((','));
                //double[] URanges = new double[UL.Length];
                //double[] IRanges = new double[IL.Length];
                //for (int ii = 0; ii < URanges.Length; ii++)
                //    URanges[ii] = Convert.ToDouble(UL[ii]);
                //for (int ii = 0; ii < IRanges.Length; ii++)
                //    IRanges[ii] = Convert.ToDouble(IL[ii]);
                //(CommCtr.FDevComm as IDirectCtr)._SetPwrRanges(URanges, IRanges);
                #endregion
                //CommCtr.FDevComm.ClearMtParams();
                //CommCtr.FDevComm.SetUb(Convert.ToDouble(ConfigHelper.GetValue("MeterUb")));//设定额定电压
                ////CommCtr.FDevComm.SetIb_Imax(Convert.ToDouble(tbIb.Text), Convert.ToDouble(tbImax.Text));
                ////CommCtr.FDevComm.ConnectType = GetConnectType(0, 0); //接线方式设定
                //CommCtr.FDevComm.ConnectType = TConnectType.cnP1; //接线方式设定
                //CommCtr.FDevComm.SetFreq(Convert.ToDouble(ConfigHelper.GetValue("MeterFreq"))); //设置频率
                //CommCtr.FDevComm.SetPF("1.0"); //功率因数为1
                ////CommCtr.FDevComm.SetCs2(0, TConstType.cttLightActive, tbCst.Text);       //设置所有表位光电类有功常数
                ////CommCtr.FDevComm.SetCs2(0, TConstType.cttPulseActive, tbCst.Text);       //设置所有表位脉冲类有功常数
                ////CommCtr.FDevComm.SetSamplePort(0, TSamplePort.spLight); //设置所有表位信号采样方式
                //CommCtr.FDevComm.SetMtParams();//通知装置刷新参数
                return true;
            }
            catch (Exception)
            {
                return false;
            }           
        }
        /// <summary>
        /// 设置挂表信息
        /// </summary>
        /// <returns></returns>
        public static bool InitMeterParams()
        {
            try
            {
                //CommCtr.FDevComm.SetTestMode(TTestMode.tmStartPC);// 设置PC控制模式   
               
                //CommCtr.FDevComm.ClearMtParams();
                CommCtr.FDevComm.SetUb(Convert.ToDouble(ConfigHelper.GetValue("MeterUb")));//设定额定电压
                //CommCtr.FDevComm.SetIb_Imax(Convert.ToDouble(tbIb.Text), Convert.ToDouble(tbImax.Text));
                //CommCtr.FDevComm.ConnectType = GetConnectType(0, 0); //接线方式设定
                CommCtr.FDevComm.ConnectType = TConnectType.cnP1; //接线方式设定
                CommCtr.FDevComm.SetFreq(Convert.ToDouble(ConfigHelper.GetValue("MeterFreq"))); //设置频率
                CommCtr.FDevComm.SetPF("1.0"); //功率因数为1
                //CommCtr.FDevComm.SetCs2(0, TConstType.cttLightActive, tbCst.Text);       //设置所有表位光电类有功常数
                //CommCtr.FDevComm.SetCs2(0, TConstType.cttPulseActive, tbCst.Text);       //设置所有表位脉冲类有功常数
                //CommCtr.FDevComm.SetSamplePort(0, TSamplePort.spLight); //设置所有表位信号采样方式
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static void NoMeter(int i,bool isHave)
        {
            if (isHave)
            {
                CommCtr.FDevComm.SetPosStatus(i+ 1, TDataType.dtAuToShort, XOnOff.x_Off);
                CommCtr.FDevComm.SetPosStatus(i + 1, TDataType.dtDevPosStatus, XOnOff.x_On);
                CommCtr.FDevComm.SetPosStatus(i + 1, TDataType.dtCaptureChannel, XOnOff.x_On);
                CommCtr.FDevComm.SetPosStatus(i + 1, TDataType.dtRS485, XOnOff.x_On);
            }
            else
            {
                CommCtr.FDevComm.SetPosStatus(i + 1, TDataType.dtAuToShort, XOnOff.x_On);
                CommCtr.FDevComm.SetPosStatus(i + 1, TDataType.dtDevPosStatus, XOnOff.x_Off);
                CommCtr.FDevComm.SetPosStatus(i + 1, TDataType.dtCaptureChannel, XOnOff.x_Off);
                CommCtr.FDevComm.SetPosStatus(i + 1, TDataType.dtRS485, XOnOff.x_Off);
            }
            
        }
        public static void SetParams()
        {
            CommCtr.FDevComm.SetMtParams();//通知装置刷新参数
        }

        public static void GetRefFreq(int Meter_No,int TestCS,int TestTime)
        {
            Thread powerThread;
            powerThread = new Thread(() =>
            {
                dicTimes[Meter_No] = new ClockHelper();
                dicTimes[Meter_No].TestTime = TestTime;
                dicTimes[Meter_No].Mtetr_No = Meter_No;
                dicTimes[Meter_No].TestCS = TestCS;
                while (TestCS > 0)
                {
                    TestCS--;
                    Thread.Sleep(TestTime * 1000);
                    double BasFreq, TstFreq, DayErr;
                    CommCtr.FDevComm.GetRefFreq(Meter_No, out BasFreq, out TstFreq, out DayErr);
                    dicTimes[Meter_No].TestValueList.Add(BasFreq);
                    dicTimes[Meter_No].TestDayList.Add(DayErr);

                }
            });
            powerThread.IsBackground = true;
            powerThread.Start();
        }
        

        /// <summary>
        /// 负载点调整
        /// </summary> 
        /// <returns></returns>
        public static bool Power_On(PowerParam power)
        {
            bool rlt = false;
            try
            {
                LogService.Instance.Error("Power_On Start");
                IsPowering = true;
                rlt = GetpowerMethod().Power_On(power);
                IsPowering = false;
                 LogService.Instance.Error("Power_On End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }      

        /// <summary>
        ///校表脉冲采集通道切换
        /// </summary>
        /// <param name="Meter_No">表位号</param>
        /// <param name="Channel_Flag">脉冲采样通道：0-有功正向/反向；1-有功反向；2-无功反向/正向；3-无功反向； 4-时钟；5-投切；6-需量 
        /// <param name="Dev_Port">端口</param>
        /// <returns></returns>       
        public static bool Set_Pulse_Channel(int Meter_No, int Channel_Flag, int Dev_Port)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("Set_Pulse_Channel Start");
                rlt = GetpowerMethod().Set_Pulse_Channel(Meter_No, Channel_Flag, Dev_Port);
                 LogService.Instance.Error("Set_Pulse_Channel End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }

        /// <summary>
        /// 开始测试误差
        /// </summary>
        /// <param name="Meter_No"></param>
        /// <param name="Constant"></param>
        /// <param name="Pulse"></param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        public static bool Error_Start(int Meter_No, double Constant, int Pulse, int Dev_Port)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("Error_Start Start");
                rlt = GetpowerMethod().Error_Start(Meter_No, Constant, Pulse, Dev_Port);
                 LogService.Instance.Error("Error_Start End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }

        /// <summary>
        /// 读取误差，次数+","+误差  如2,-0.024 表示误差为-0.024,是第二次误差,第0次误差表示误差还没有，需要丢弃。
        /// </summary>
        /// <param name="MError"></param>
        /// <param name="Meter_No"></param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        public static bool Error_Read(ref string error, int Meter_No, int circle, int Dev_Port)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("Error_Read Start");
                rlt = GetpowerMethod().Error_Read(ref error, Meter_No, circle, Dev_Port);
                 LogService.Instance.Error("Error_Read End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }

        

        /// <summary>
        /// 指示仪表数据
        /// </summary>
        /// <param name="SData">结果Ua,Ub,Uc,Ia,Ib,Ic,φa,φb,φc,Pa,Pb,Pc,Qa,Qb,Qc,Sa,Sb,Sc,有功功率(总),无功功率(总),视在功率(总),频率,{电压档位},电流档位</param>
        /// <param name="SModel">装置配置的标准表型号</param>
        /// <param name="Dev_Port">通讯口号。如：1=COM1</param>
        /// <returns></returns>
        public static PowerParam StdMeter_Read()
        {

            if (IsPowering)
            {
                return new PowerParam();
            }
             LogService.Instance.Error("StdMeter_Read Start");
            PowerParam power = GetpowerMethod().StdMeter_Read();
             LogService.Instance.Error("StdMeter_Read End");
            return power;
        }
        //TODO:功耗
        public static void PowerBoard_Read(int Meter_No,out double P,out double S)
        {
            P = 0; S = 0;
        }

        /// <summary>
        /// 停止潜动
        /// </summary>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        public static bool Stop_Test(int Dev_Port)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("Stop_Test Start");
                rlt = GetpowerMethod().Stop_Test(Dev_Port);
                 LogService.Instance.Error("Stop_Test End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }

        /// <summary>
        /// 误差仪总清
        /// </summary>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        public static bool Error_Clear(int Dev_Port)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("Error_Clear Start");
                rlt = GetpowerMethod().Error_Clear(Dev_Port);
                 LogService.Instance.Error("Error_Clear End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }

        /// <summary>
        /// 降电压电流
        /// </summary>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        public static bool Power_Off(int Dev_Port)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("Power_Off Start");
                rlt = GetpowerMethod().Power_Off(Dev_Port);
                 LogService.Instance.Error("Power_Off End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }

       

        /// <summary>
        /// 标准时钟仪切换
        /// </summary>
        /// <param name="SetFlag">0-切开 ；1-切上</param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        public static bool SetRefClock(byte SetFlag, int Dev_Port)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("SetRefClock Start");
                rlt = GetpowerMethod().SetRefClock(SetFlag, Dev_Port);
                 LogService.Instance.Error("SetRefClock End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }
        /// <summary>
        /// 开始测试时钟误差
        /// </summary>
        /// <param name="Meter_No"></param>
        /// <param name="TheoryFreq">输出频率(Hz)</param>
        /// <param name="TestTime">测试时间(s)</param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        public static bool Clock_Error_Start(int Meter_No, double TheoryFreq, int TestTime, int Dev_Port=1)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("Clock_Error_Start Start");
                rlt = GetpowerMethod().Clock_Error_Start(Meter_No, TheoryFreq, TestTime, Dev_Port);
                 LogService.Instance.Error("Clock_Error_Start End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }
        /// <summary>
        /// 读取时钟误差
        /// </summary>
        /// <param name="MError"></param>
        /// <param name="TheoryFreq"></param>
        /// <param name="ErrorType">0-频率；1-日计时误差；2-相对误差</param>
        /// <param name="Meter_No"></param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        public static bool Clock_Error_Read(ref string MError, double TheoryFreq, byte ErrorType, int Meter_No, int Dev_Port)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("Clock_Error_Read Start");
                rlt = GetpowerMethod().Clock_Error_Read(ref MError, TheoryFreq, ErrorType, Meter_No, Dev_Port);
                 LogService.Instance.Error("Clock_Error_Read End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }
        public static bool Clock_Error_End()
        {
            try
            {
                CommCtr.FDevComm.SwitchU(XLABC.x_L, XOnOff.x_Off);                           //关输出
                CommCtr.FDevComm.StopTest(TTestMode.tmTimeTest);                                     //退出基频测试
                foreach (var item in dicTimes)
                {
                    dicTimes[item.Key] = new ClockHelper();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        /// <summary>
        /// 设置时钟仪高频频率
        /// </summary>
        /// <param name="StdFreq">时钟仪高频频率</param>
        /// <returns></returns>
        public static bool SetStdFreq(double StdFreq)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("SetStdFreq Start");
                rlt = GetpowerMethod().SetStdFreq(StdFreq);
                 LogService.Instance.Error("SetStdFreq End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }


      

        /// <summary>
        /// 设置谐波参数
        /// <para>说明：所有参数为三维数组，在升源命令前调用</para>
        /// <para>举例：Set_Harmonic_Data([0,0,0], [7,3,5], [0,0,0], [10,15,30])</para>
        /// </summary>
        /// <param name="HAng">谐波相位 三维数组</param>
        /// <param name="HNum">谐波次数 (3、5、7 次可任意叠加，传送时高次在前，其它次数只能发送单次，且放在第一位)</param>
        /// <param name="HVolt">电压谐波含量(总含量不能超40%)</param>
        /// <param name="HCur">电流谐波含量(总含量不能超40%)</param>
        /// <returns></returns>
        public static bool Set_Harmonic_Data(double[] HAng, int[] HNum, double[] HVolt, double[] HCur)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("Set_Harmonic_Data Start");
                rlt = GetpowerMethod().Set_Harmonic_Data(HAng, HNum, HVolt, HCur);
                 LogService.Instance.Error("Set_Harmonic_Data End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }

       

        /// <summary>
        /// 自动复位
        /// </summary>
        /// <param name="Meter_No">1开始</param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>    
        public static bool AutoSub_Reset(int Meter_No, byte Dev_Port)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("AutoSub_Reset Start");
                rlt = GetpowerMethod().AutoSub_Reset(Meter_No, Dev_Port);
                 LogService.Instance.Error("AutoSub_Reset End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }

            return rlt;
        }

        /// <summary>
        /// 自动短接
        /// </summary>
        /// <param name="Meter_No">1开始</param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        public static bool AutoSub(int Meter_No, byte Dev_Port)
        {
            bool rlt = false;

            try
            {
                 LogService.Instance.Error("AutoSub Start");
                rlt = GetpowerMethod().AutoSub(Meter_No, Dev_Port);
                 LogService.Instance.Error("AutoSub End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }

            return rlt;
        }

        public static bool AutoAll_ResetSY()
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("AutoAll_ResetSY Start");
                rlt = GetpowerMethod().AutoAll_ResetSY();
                 LogService.Instance.Error("AutoAll_ResetSY End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }

        /// <summary>
        /// 发送命令 有返回值
        /// </summary>
        /// <param name="Dev_Port">通讯串口号 ,整形</param>
        /// <param name="ErrAddr"> 误差仪地址 41H+表位号，如2表位43H,H代表16进制 ; $FF为广播地址 ;$41为0#误差仪(功能切换总控板)</param>
        /// <param name="ErrCmd">命令字 ,整形18H - 读取走字脉冲数33H - 指示灯控制41H - 485极性反转</param>
        /// <param name="CmdData">命令参数 ,字符串 在C中为指针型        如果要传16进制格式，就加前缀#。如’#13’，代表传进去为十进制19的字符串。</param>
        /// <param name="BackData">命令返回数据帧,字符串 在C中为指针型        返回：01H+地址(41H+表位号) +长度+电表脉冲数(6位)+ 标准表脉冲数10位(每个分机一样)+校验位+ 结束(17H)</param>
        /// <returns></returns>
        public static bool ErrorComm(int Dev_Port, int ErrAddr, byte ErrCmd, byte[] CmdData, ref StringBuilder BackData)
        {
            bool rlt = false;

            try
            {
                 LogService.Instance.Error("ErrorComm Start");
                rlt = GetpowerMethod().ErrorComm(Dev_Port, ErrAddr, ErrCmd, CmdData, ref BackData);
                 LogService.Instance.Error("ErrorComm End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }

            return rlt;
        }

        /// <summary>
        /// 发送命令 有返回值
        /// </summary>
        /// <param name="Dev_Port">通讯串口号 ,整形</param>
        /// <param name="ErrAddr"> 误差仪地址 41H+表位号，如2表位43H,H代表16进制 ; $FF为广播地址 ;$41为0#误差仪(功能切换总控板)</param>
        /// <param name="ErrCmd">命令字 ,整形18H - 读取走字脉冲数33H - 指示灯控制41H - 485极性反转</param>
        /// <param name="CmdData">命令参数 ,字符串 在C中为指针型        如果要传16进制格式，就加前缀#。如’#13’，代表传进去为十进制19的字符串。</param>
        /// <param name="BackData">命令返回数据帧,字符串 在C中为指针型        返回：01H+地址(41H+表位号) +长度+电表脉冲数(6位)+ 标准表脉冲数10位(每个分机一样)+校验位+ 结束(17H)</param>
        /// <returns></returns>
        public static bool ErrorComm1(int Dev_Port, int ErrAddr, byte ErrCmd, byte[] CmdData)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("ErrorComm1 Start");
                rlt = GetpowerMethod().ErrorComm1(Dev_Port, ErrAddr, ErrCmd, CmdData);
                 LogService.Instance.Error("ErrorComm1 End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }

            return rlt;
        }


        /// <summary>
        /// 误差仪累计脉冲读取 读取脉冲数
        /// </summary>
        /// <param name="MPulse">表累计脉冲数</param>
        /// <param name="Meter_No"></param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>  
        public static bool Count_Read(ref int MPulse, int Meter_No, byte Dev_Port)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("Count_Read Start");
                rlt = GetpowerMethod().Count_Read(ref MPulse, Meter_No, Dev_Port);
                 LogService.Instance.Error("Count_Read End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }

        /// <summary>
        /// 开始潜动实验
        /// </summary>
        /// <param name="Meter_No"></param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        public static bool Count_Start(int Meter_No, bool Active, int Pulse, int totaltime)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("Count_Start Start");
                rlt = GetpowerMethod().Count_Start(Meter_No, Active, Pulse, totaltime);
                 LogService.Instance.Error("Count_Start End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }
        /// <summary>
        /// 走字结果读取
        /// </summary>
        /// <param name="Pulse">脉冲数</param>
        /// <param name="StdKWH">标准表走字量</param>
        /// <param name="Constant">被校表常数  1200 1600 etc</param>
        /// <param name="Meter_No"></param>
        /// <param name="Dev_Port"></param>
        /// <returns></returns>
        public static bool ConstPulse_Read(ref int Pulse, ref double StdKWH, double Constant, int Meter_No, int Dev_Port)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("ConstPulse_Read Start");
                rlt = GetpowerMethod().ConstPulse_Read(ref Pulse, ref StdKWH, Constant, Meter_No, Dev_Port);
                 LogService.Instance.Error("ConstPulse_Read End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }

        /// <summary>
        /// 功率源复位与短接
        /// </summary>
        /// <param name="isused">true:复位，false：短接</param>
        /// <param name="index">台位号(>0)</param>
        public static bool PowerAutoReset(bool isused, int index)
        {
            bool rlt = false;
            try
            {
                 LogService.Instance.Error("PowerAutoReset Start");
                if (isused) //复位
                    rlt = AutoSub_Reset(index, PowerHelper.Dev_Port);
                else //短路
                    rlt = AutoSub(index, PowerHelper.Dev_Port);
                 LogService.Instance.Error("PowerAutoReset End");
            }
            catch (Exception ex)
            {
                 LogService.Instance.Error(ex);
            }
            return rlt;
        }
    }
}
