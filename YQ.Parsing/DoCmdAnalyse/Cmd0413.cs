using System;
using System.Linq;
using System.Text;
using System.Threading;
using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 读计时误差
    /// </summary>
    public class Cmd0413 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            try
            {
                var Meter_No = Convert.ToInt32(requestCmd.data[0]);
                var TestCS= PowerHelper.dicTimes[Meter_No].TestCS;
                var TestTime= PowerHelper.dicTimes[Meter_No].TestTime;
                var TestValueList = PowerHelper.dicTimes[Meter_No].TestValueList;
                if (TestCS == TestValueList.Count)
                {
                    var value= Math.Round(TestValueList.Sum(x => x / TestTime)/TestCS/86400,1).ToString();
                    //var value = Math.Round(TestValueList.Sum(x => x / TestTime) / TestCS , 1).ToString();
                    rlt = new ResponseCmd(requestCmd.cmd, 0, value);
                }
                else
                {
                    rlt = new ResponseCmd(requestCmd.cmd, 1, null);
                }
               
            }
            catch (Exception)
            {
                rlt = new ResponseCmd(requestCmd.cmd, 1, null);
            }
            return rlt;
        }
    }
}
