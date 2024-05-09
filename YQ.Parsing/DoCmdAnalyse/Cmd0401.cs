using System;
using YQ.Tool;
using YQ.Tool.JY;
using static YQ.Parsing.DoCmdAnalyse.Cmd0506;

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
            //PowerHelper.InitMeterParams();
            int MeterID = Convert.ToInt32(requestCmd.data[0]);
            var IsCheak = requestCmd.data[1];
            var IsYaJie = requestCmd.data[2];
            if (IsCheak=="1")
            {
                PowerHelper.NoMeter(MeterID-1, true);
                //切换电压功耗
                ComParamter com = new ComParamter(ConfigHelper.GetValue("GHCom"));
                SerialManager.Instance.CreateAndOpenPort(com);
                byte[] b = GongHao.ChangeGH(requestCmd.data[0]);
                com.SendData.AddRange(b);
                SerialPortService.SendByte(com);
            }
            else
            {
                PowerHelper.NoMeter(MeterID-1, false);
            }            
            PowerHelper.SetParams();
            
            JYHelper jYHelper = new JYHelper();
            if (jYHelper.JYConnet(ConfigHelper.GetValue("JY1IP"), Convert.ToInt32(ConfigHelper.GetValue("JY1Port"))))
            {
                if (IsYaJie == "01")
                {
                    jYHelper.OpenDO(254,MeterID-1);

                }
                else
                {
                    jYHelper.CloseDO(254, MeterID - 1);
                }
            }
         
            rlt = new ResponseCmd(requestCmd.cmd, 0, null);
            return rlt;
        }
    }
}
