using System;
using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 设置计时误差测试的相关参数
    /// </summary>
    public class Cmd0411 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            try
            {
                var Meter_No = Convert.ToInt32(requestCmd.data[0]);
                var TheoryFreq = PowerHelper.Std_consant;
                var TestTime = Convert.ToInt32(requestCmd.data[1]);
                var TestCS = Convert.ToInt32(requestCmd.data[2]);
                PowerHelper.Clock_Error_Start(Meter_No, TheoryFreq, TestTime);
                PowerHelper.GetRefFreq(Meter_No, TestCS, TestTime);
                rlt = new ResponseCmd(requestCmd.cmd, 0, null);
            }
            catch (Exception)
            {
                rlt = new ResponseCmd(requestCmd.cmd, 1, null);
            }
            return rlt;
        }
    }
}
