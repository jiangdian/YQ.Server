using System;
using System.Threading;
using YQ.Tool;
using YQ.Tool.JY;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 撤销自动压接
    /// </summary>
    public class Cmd0409 : AbstractCmdAnalyse
    { 
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            JYHelper jYHelper = new JYHelper();
            JYHelper jYHelper2 = new JYHelper();
            if (jYHelper.JYConnet(ConfigHelper.GetValue("JY1IP"), Convert.ToInt32(ConfigHelper.GetValue("JY1Port")))
                && jYHelper2.JYConnet(ConfigHelper.GetValue("JY2IP"), Convert.ToInt32(ConfigHelper.GetValue("JY2Port"))))
            {
                jYHelper.CloseAllDO(Convert.ToInt32(ConfigHelper.GetValue("JY1Addr")), Convert.ToInt32(ConfigHelper.GetValue("JY1Num")));
                jYHelper2.CloseAllDO(Convert.ToInt32(ConfigHelper.GetValue("JY2Addr")), Convert.ToInt32(ConfigHelper.GetValue("JY2Num")));
            }
            else
            {
                rlt = new ResponseCmd(requestCmd.cmd, 1, null);
            }

            return rlt;
        }

     
    }
}
