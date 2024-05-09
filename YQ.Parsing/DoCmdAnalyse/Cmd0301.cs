using System;
using System.Linq;
using System.Threading;
using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 读标准表测量值
    /// 读取电压、电流、有功功率、无功功率、视在功率、功率因素、频率、相位角、功率因素角等
    /// </summary>
    public class Cmd0301 : AbstractCmdAnalyse
    {
        private static int ReadFailTimes = 1;
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            //if (StdBuffer.Count > 0)// StdBuffer：读取标准表的示值缓存 -- 降低标准表读取接口的调用频率
            //{
            //    rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, 0, StdBuffer.First()));// 
            //    StdBuffer.RemoveAt(0);
            //    Thread.Sleep(200);
            //    return rlt;
            //}
            PowerParam power = new PowerParam();
            try
            {
                power = PowerHelper.StdMeter_Read();
                //if (power.Result)
                //{
                    //StdBuffer.Add(power.ToString());
                    rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, 0, power.ToString()));
                    Thread.Sleep(500);//让国网软件多等一会
                //}
                //else
                //{
                    //if (ReadFailTimes >= 2 && LastPower != null)
                    //{
                    //    ReadFailTimes = 1;
                    //    rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, requestCmd.sn, 0, LastPower.ToString()));
                    //    YQLogHelper.WriteLog("StdMeter_Read未读到，应答缓存结果！");
                    //}
                    //else
                    //{
                        //ReadFailTimes++;
                        //LogService.Instance.Error("StdMeter_Read未读到，应答异常结果！");
                        //rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, 1));
                    //}
                    //Thread.Sleep(1000);//让国网软件多等一会
                //}
            }
            catch (Exception ex)
            {
                LogService.Instance.Error("Cmd0301 Error！");
                rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, 1));
            }
            return rlt;
        }
    }
}
