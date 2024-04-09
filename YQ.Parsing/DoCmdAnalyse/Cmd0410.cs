namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 电压回路测试
    /// 测试电能表电压回路是否短路或故障，测试电能表是否能上电
    /// 电压回路状态：1表示正常，0表示故障
    /// </summary>
    public class Cmd0410 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            //if (PowerHelper.config.PowerCategory == 2)
            //{
            //    double vol = 0;
            //    if (!double.TryParse(requestCmd.GetDataString(), out vol))
            //    {
            //        vol = 12;
            //    }
            //    PowerParam power = new PowerParam()
            //    {
            //        Ua = vol,
            //        Ub = vol,
            //        Uc = vol,
            //        Uab = 120,
            //        Uac = 240,
            //        Ua_Phase = 0,
            //        Ub_Phase = 120,
            //        Uc_Phase = 140,
            //        Frequency = 50,
            //    };
            //    PowerHelper.Power_On(power);
            //    Thread.Sleep(2000);//注:不加延时通信有干扰
            //    rlt = CheckVol_SY(requestCmd);
            //    Thread.Sleep(2000);//注:不加延时通信有干扰
            //    PowerHelper.Power_Off(PowerHelper.config.PowerCom);
            //    Thread.Sleep(2000);//注:不加延时可能导致后续升源间隔时间过短，升源失败
            //}
            //else if (PowerHelper.config.PowerCategory == 4)
            //{
            //    float[] setData = new float[16];
            //    setData[0] = 0;
            //    setData[1] = 0;
            //    setData[2] = 0;
            //    setData[3] = 0;
            //    string eee = "";
            //    int IPnum = (int)Math.Ceiling(PowerHelper.config.MeterCount / 4.0);       // 获取表位误差计主板通讯ip地址的数量
            //                                                                              // 设置回路闭合，包含回路检测的功能。表架各表位负荷开关（电压）设置
            //    bool retval_Switch_Set = KREMethod.KreRelaySwitchSet(eee);
            //    Thread.Sleep(1000);
            //    bool retval_Switch_Read = KREMethod.KreRelaySwitchRead(ref eee);// 读6个表位的电压回路状态
            //    retval_Switch_Set = KREMethod.KreRelaySwitchSet(eee);
            //    rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, requestCmd.sn, 0, eee));
            //}
            //else
            //{
            //    rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 1, "不支持电压回路测试.");
            //}
            return rlt;
        }
    }
}
