using System;
using System.Threading;
using YQ.Tool;
using YQ.Tool.JY;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 升降源
    /// 可以指定接线方式、频率、电压、电流，相角、是否输出谐波等
    /// </summary>
    public class Cmd0201 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            if (requestCmd.data.Count < 7)
            {
                rlt = new ResponseCmd(requestCmd.cmd, 1, "data is wrong! data count=" + requestCmd.data.Count);
                return rlt;
            }

            //盛迪、新跃参数 - 相线：0：单相, 1：三相四线有功, 2：三相三线有功, 3：90 度无功, 4：60 度无功, 5：四线正弦无功, 6：三线正弦无功, 7：单相无功
            //国网参数 - 接线方式：0：PT4 三相四线有功, 1：QT4 三相四线无功, 2：P32 三相三线有功, 3：Q32 三相三线无功。单相表接线方式选择 0。
            //int Convert.ToInt32(requestCmd.data[0]);
            //电压电流输出控制：1Byte 的数值；0-2Bit 分别表示 A、B、C 相电压，3-5Bit 分别表示 A、B、C 相电流；1：表示输出，0：表示不输出。
            //byte powerFlag = Convert.ToByte(requestCmd.data[1]);

            PowerParam power = new PowerParam()
            {
                Phase = Convert.ToInt32(requestCmd.data[0]),//接线方式
                PowerFlag = Convert.ToByte(requestCmd.data[1]),//电压电流输出控制
                Ua = Convert.ToDouble(requestCmd.data[2]),//电压
                Ia = Convert.ToDouble(requestCmd.data[3]),//电流
                FP_Angle = Convert.ToDouble(requestCmd.data[4]),//功率因数角φ
                Rated_Freq = double.Parse(requestCmd.data[5]),//频率
                HarmonicFlag = Convert.ToByte(requestCmd.data[6])//谐波
            };
            Thread powerThread;
            powerThread = new Thread(() =>
            {
                //StdBuffer.Clear();
                PowerHelper.IsPowering = true;
                //if (PowerHelper.HangPos?.Count==0)
                //{
                    LightOn();
                //}
                PowerHelper.SetParams();
                PowerHelper.Power_On(power);//一般情况下的升降源
                PowerHelper.IsPowering = false;
            });
            powerThread.IsBackground = true;
            powerThread.Start();
            //Thread.Sleep(11000);
            //PowerParam power2 = new PowerParam();
            //DateTime dt=DateTime.Now;
            //while (power2.Ua < power.Ua - 10&&dt.AddSeconds(20)>DateTime.Now)
            //{
            //    power2 = PowerHelper.StdMeter_Read();
            //    Thread.Sleep(1000);
            //}
             rlt = new ResponseCmd(requestCmd.cmd,  0, null);
            return rlt;
        }

        private void LightOn()
        {
            JYHelper jYHelper = new JYHelper();
            JYHelper jYHelper2 = new JYHelper();
            if (jYHelper.JYConnet(ConfigHelper.GetValue("JY1IP"), Convert.ToInt32(ConfigHelper.GetValue("JY1Port"))))
            {
                var rst = jYHelper.ReadDO(Convert.ToInt32(ConfigHelper.GetValue("JY1Addr")), Convert.ToInt32(ConfigHelper.GetValue("JY1Num")));
                //PowerHelper.InitMeterParams();
                jYHelper2.JYConnet(ConfigHelper.GetValue("JY2IP"), Convert.ToInt32(ConfigHelper.GetValue("JY2Port")));
                PowerHelper.HangPos?.Clear();
                for (int j = 0; j < rst.Length & j < 4; j++)
                {
                    byte status = rst[j];
                    for (int i = 0; i < 8; i++)
                    {
                        if ((status & (1 << i)) == 0x00)//无表
                        {
                            PowerHelper.NoMeter(j * 8 + i, false);
                        }
                        else
                        {
                            PowerHelper.NoMeter(j * 8 + i, true);
                            jYHelper2.OpenDO(Convert.ToInt32(ConfigHelper.GetValue("JY2Addr")), j * 8 + i);
                            PowerHelper.HangPos.Add(j * 8 + i + 1);
                        }
                    }
                }
                PowerHelper.SetParams();

            }

        }
    }
}
