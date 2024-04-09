using System;
using System.Linq;
using System.Windows.Markup;
using YQ.FreeSQL.Entity;
using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 查询表位电压线路功耗
    /// </summary>
    public class Cmd1001 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            //AbstractCmd rlt = null;
            //try
            //{
            //    var type = Convert.ToInt32(requestCmd.data[1]);
            //    var num = 0;
            //    var name = "";
            //    switch ((TypeEnum)type)
            //    {
            //        case TypeEnum.RS232:
            //            num = 9;
            //            name = "EventReport";
            //            break;
            //        case TypeEnum.RS485:
            //            num = 1;
            //            name = "RS4851";
            //            break;
            //        case TypeEnum.RJ45:
            //            num = 2;
            //            name = "RJ4514851";
            //            break;
            //        case TypeEnum.红外:
            //            break;
            //        case TypeEnum.蓝牙:
            //            num = 7;
            //            name = "Bluetooth";
            //            break;
            //        default:
            //            num = 1;
            //            name = "RS4851";
            //            break;
            //    }
            //    //var value = freeSql.Select<ComSet>()
            //    //    .Where(a => a.ComID == Convert.ToInt32(requestCmd.data[0])*10+num)
            //    //    .ToOne();
            //    //ComParamter com = new ComParamter(value.ComName+"-"+value.ComPara);
            //    var value= freeSql.Select<MeterSet>()
            //        .Where(a => a.MeterID == Convert.ToInt32(requestCmd.data[0]))
            //        .ToOne();
            //    ComParamter com = new ComParamter(value.GetType().GetProperty(name).GetValue(value).ToString());
            //    SerialManager.Instance.CreateAndOpenPort(com.PortName, com.BaudRate, com.Parity, com.DataBits, com.StopBits);
            //    string sdata = requestCmd.data[2];
            //    for (int i = 0; i < sdata.Length; i += 2)
            //    {
            //        byte b = Convert.ToByte(sdata.Substring(i, 2), 16);
            //        com.SendData.Add(b);
            //    }
            //    SerialPortService.SendData(com);
            //    if (com.RecData == null)
            //    {
            //        rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, 1, null));
            //    }
            //    else
            //    {
            //        //返回data内容：通信端口；应答帧（无空格）
            //        string redata = FrameHelper.bytetostr(com.RecData.ToArray()).Replace(" ", "");
            //        rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, 0, requestCmd.data[0] + ";" + redata));
            //    }
            //    return rlt;
            //}
            //catch (Exception)
            //{
            //    rlt = new ResponseCmd(requestCmd.cmd, 1, null);
            //}
            //return rlt;
            AbstractCmd rlt = rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, null, 0));
            return rlt;
        }
    }
}
