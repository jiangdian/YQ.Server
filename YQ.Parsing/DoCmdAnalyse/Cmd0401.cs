using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 表位初始化
    /// 初始化表位状态，包括误差计算、日计时、电压回路、电流回路、载波通道、潜启动、走字、时段投切、功耗、需量等状态
    /// </summary>
    public class Cmd0401 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            PowerHelper.InitMeterParams();
            PowerHelper.SetParams();
            rlt = new ResponseCmd(requestCmd.cmd, 0, null);
            return rlt;
        }
    }
}
