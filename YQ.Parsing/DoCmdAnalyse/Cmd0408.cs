using System;
using System.Security.Cryptography;
using System.Threading;
using YQ.Tool;
using YQ.Tool.JY;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 自动压接
    /// </summary>
    public class Cmd0408 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
           //读DO
           JYHelper jYHelper = new JYHelper();
           JYHelper jYHelper2 = new JYHelper();
            if (jYHelper.JYConnet(ConfigHelper.GetValue("JY1IP"), Convert.ToInt32(ConfigHelper.GetValue("JY1Port"))))
            {
                 var rst = jYHelper.ReadDO(Convert.ToInt32(ConfigHelper.GetValue("JY1Addr")), Convert.ToInt32(ConfigHelper.GetValue("JY1Num")));
                PowerHelper.InitMeterParams();
                jYHelper2.JYConnet(ConfigHelper.GetValue("JY2IP"), Convert.ToInt32(ConfigHelper.GetValue("JY2Port")));
                PowerHelper.HangPos.Clear();
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
                            PowerHelper.HangPos.Add(j * 8 + i+1);
                        }
                    }
                }
                PowerHelper.SetParams();
            }
            else
            {
                rlt = new ResponseCmd(requestCmd.cmd, 1, null);
            }
            return rlt;
        }

     
    }
}
