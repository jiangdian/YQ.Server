using System;
using System.Threading;
using YQ.Tool;
using YQ.Tool.JY;

namespace YQ.Parsing.DoCmdAnalyse
{

    /// <summary> 
    /// 关源
    /// </summary>
    public class Cmd0203 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            Thread powerThread;             
            powerThread = new Thread(() =>
            {
                    PowerHelper.IsPowering = true;
                    PowerHelper.Power_Off(1);//一般情况下的升降源
                    CloseLight();
                    PowerHelper.IsPowering = false;                    
            });
            powerThread.IsBackground = true;
            powerThread.Start();
            Thread.Sleep(1500);
            rlt = new ResponseCmd(requestCmd.cmd, 0, null);//
            return rlt;
        }
        private void CloseLight()
        {
            //JYHelper jYHelper = new JYHelper();
            JYHelper jYHelper2 = new JYHelper();
            if (jYHelper2.JYConnet(ConfigHelper.GetValue("JY2IP"), Convert.ToInt32(ConfigHelper.GetValue("JY2Port"))))
            {
                //jYHelper.CloseAllDO(Convert.ToInt32(ConfigHelper.GetValue("JY1Addr")), Convert.ToInt32(ConfigHelper.GetValue("JY1Num")));
                jYHelper2.CloseAllDO(Convert.ToInt32(ConfigHelper.GetValue("JY2Addr")), Convert.ToInt32(ConfigHelper.GetValue("JY2Num")));
            }
            PowerHelper.HangPos?.Clear();
            for (int i = 0; i < 12; i++)
            {
                PowerHelper.NoMeter(i, false);
            }
        }
    }
}
