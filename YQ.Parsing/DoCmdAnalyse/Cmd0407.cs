using System;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 通信模块接入 IO控制器1-8路与载波1-8对应
    /// 接通某个通信模块和表位通信链路
    /// </summary>
    public class Cmd0407 : AbstractCmdAnalyse
    {
        //public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        //{
        //    AbstractCmd rlt = null;
        //    try
        //    {
        //        //data=通道序号
        //        //[说明：通道序号：1~n，对应不同的通信模块。n 小于等于 8 时为载波模块，n 大于 8 时为非载波模块]
        //        int roadno = int.Parse(requestCmd.data[0]);//目前我们的载波模块只有8路 -- IO口16-9，对应载波1-8路
        //        if (roadno > 8)
        //        {
        //            rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 1, "only 1 - 8 road.");
        //            return rlt;
        //        }
        //        roadno = 17 - roadno;//目前我们的载波模块只有8路 -- IO口16-9，对应载波1-8路
        //        SysConfig sysConfig = LocalDB.GetSysConfig(2);
        //        ShengDiHelper.Power_Off(sysConfig.powercom);
        //        //切换电源                            
        //        if (!ChangeCityPower(true))
        //        {
        //            rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, requestCmd.sn, 1, "power change error"));
        //            return rlt;
        //        }

        //        RelayControllerHelper helper = new RelayControllerFT(sysConfig.RoadComName);
        //        helper.Connect(roadno);
        //        rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 0, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        YQLogHelper.WriteLog("通信模块接入失败!", ex);
        //        rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 1, null);
        //    }
        //    return rlt;
        //}
        //public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        //{
        //    AbstractCmd rlt = null;
        //    try
        //    {
        //        //data=通道序号
        //        //[说明：通道序号：1~n，对应不同的通信模块。n 小于等于 8 时为载波模块，n 大于 8 时为非载波模块]
        //        int roadno = int.Parse(requestCmd.data[0]);//目前我们的载波模块只有8路 -- IO口16-9，对应载波1-8路
        //        if (roadno > 8)//目前我们的载波模块只有8路 -- IO口1-8，对应载波1-8路
        //        {
        //            rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 1, "only 1 - 8 road.");
        //            return rlt;
        //        }
        //        //     SysConfig sysConfig = LocalDB.GetSysConfig(2);
        //        ComParamter com = new ComParamter(PowerHelper.config.YQARMCom);
        //        YQARMHelper yqARM = new YQARMHelper(com.PortName, com.BaudRate);
        //        //选择哪一路电压供电
        //        if (!yqARM.Open())
        //        {
        //            rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, requestCmd.sn, 1, "载波接入时，打开串口失败!"));
        //            return rlt;
        //        }
        //        try
        //        {
        //            if (!yqARM.ConnectCarrier(requestCmd.sn))
        //            {

        //                rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, requestCmd.sn, 1, "载波模块接入表位" + requestCmd.sn + "失败"));
        //                return rlt;
        //            }
        //            //切换选择指定的某载波模块
        //            ComParamter comcarr = new ComParamter(PowerHelper.config.RoadComName);
        //            RelayControllerHelper helper = new RelayControllerFT(comcarr.PortName, comcarr.BaudRate, comcarr.Parity, comcarr.DataBits, comcarr.StopBits);
        //            if (helper.Connect(roadno))
        //                rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 0, null);
        //            else
        //                rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 1, null);
        //        }
        //        catch (Exception ex)
        //        {
        //            YQLogHelper.WriteLog("载波模块接入失败!", ex, YQLogHelper.MeterLogger);
        //            rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 1, null);
        //        }
        //        finally
        //        {
        //            try { yqARM.Close(); } catch { }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        YQLogHelper.WriteLog("通信模块接入失败!", ex);
        //        rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 1, null);
        //    }
        //    return rlt;
        //}

        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)//因诺方案
        {
            AbstractCmd rlt = null;
            //try
            //{
            //    //data=通道序号
            //    //[说明：通道序号：1~n，对应不同的通信模块。n 小于等于 8 时为载波模块，n 大于 8 时为非载波模块]
            //    int roadno = int.Parse(requestCmd.data[0]);//目前我们的载波模块只有8路 -- IO口16-9，对应载波1-8路
            //    if (roadno > 8)//目前我们的载波模块只有8路 -- IO口1-8，对应载波1-8路
            //    {
            //        rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 1, "only 1 - 8 road.");
            //        return rlt;
            //    }
            //    try
            //    {
            //        //切换选择指定的某载波模块
            //        ComParamter comcarr = new ComParamter(PowerHelper.config.LightCom);
            //        RelayControllerHelper helper = new RelayControllerYN(comcarr.PortName, comcarr.BaudRate, comcarr.Parity, comcarr.DataBits, comcarr.StopBits);
            //        if (helper.Connect(roadno))
            //            rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 0, null);
            //        else
            //            rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 1, null);
            //    }
            //    catch (Exception ex)
            //    {
            //        YQLogHelper.WriteLog("载波模块接入失败!", ex, YQLogHelper.MeterLogger);
            //        rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 1, null);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    YQLogHelper.WriteLog("通信模块接入失败!", ex);
            //    rlt = new ResponseCmd(requestCmd.cmd, requestCmd.sn, 1, null);
            //}
            return rlt;
        }
    }
}
