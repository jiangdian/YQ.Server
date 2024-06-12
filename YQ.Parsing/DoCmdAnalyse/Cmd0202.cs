using System.Threading;
using System;
using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 升降源
    /// </summary>
    public class Cmd0202 : AbstractCmdAnalyse
    { 
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            if (requestCmd.data.Count < 15)
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
                Phase = Convert.ToInt32(requestCmd.data[0]),
                PowerFlag = Convert.ToByte(requestCmd.data[1]),
                Rated_Freq = double.Parse(requestCmd.data[2]),
                Ua = Convert.ToDouble(requestCmd.data[3]),
                Ua_Phase = Convert.ToDouble(requestCmd.data[4]),
                Ub = Convert.ToDouble(requestCmd.data[5]),
                Ub_Phase = Convert.ToDouble(requestCmd.data[6]),
                Uc = Convert.ToDouble(requestCmd.data[7]),
                Uc_Phase = Convert.ToDouble(requestCmd.data[8]),
                Ia = Convert.ToDouble(requestCmd.data[9]),
                Ia_Phase = Convert.ToDouble(requestCmd.data[10]),
                Ib = Convert.ToDouble(requestCmd.data[11]),
                Ib_Phase = Convert.ToDouble(requestCmd.data[12]),
                Ic = Convert.ToDouble(requestCmd.data[13]),
                Ic_Phase = Convert.ToDouble(requestCmd.data[14]),
                HarmonicFlag = Convert.ToByte(requestCmd.data[15])
            };
            

                Thread powerThread;
        

                powerThread = new Thread(() =>
                {
                    //StdBuffer.Clear();

                  
                        PowerHelper.IsPowering = true;
                        PowerHelper.Power_On(power);//一般情况下的升降源
                        PowerHelper.IsPowering = false;
                    
                });
                powerThread.IsBackground = true;
                powerThread.Start();
                Thread.Sleep(1500);
            
            PowerHelper.Phase = power.Phase;    //记录接线方式，以供三相三线读数时用         
            rlt = new ResponseCmd(requestCmd.cmd, 0, null);//盛迪标准表必须上源结束后才能读取
            return rlt;
        }
    }
}
