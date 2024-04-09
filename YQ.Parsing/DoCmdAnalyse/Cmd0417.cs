using System;
using System.Linq;
using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 查询表位电压线路功耗
    /// </summary>
    public class Cmd0417 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            try
            {
                var Meter_No = Convert.ToInt32(requestCmd.data[0]);
                PowerHelper.PowerBoard_Read(Meter_No,out double P,out double S);
                var value = P + ";" + S;
                rlt = new ResponseCmd(requestCmd.cmd, 0, value);
            }
            catch (Exception)
            {
                rlt = new ResponseCmd(requestCmd.cmd, 1, null);
            }
            return rlt;
        }
    }
}
