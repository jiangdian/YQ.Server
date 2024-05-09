using System;
using YQ.FreeSQL.Entity;
using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    /// 给接入通信单元数据
    ///<para>表位号;通讯方式;符合规约要求的 16 进制字节串的数据帧</para>
    ///<para>【说明:表位号:1~16，对应台体的各表位】</para>
    ///<para>【说明:通讯方式:1 表示 485-1,2 表示 485-2,3.485-3(逆变器?),4.485-45.485-5(逆变器?)，6.485-6，7 蓝牙，8.STA，9.CCO。】</para>
    /// </summary>
    public class Cmd1007 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            try
            {
                var meterID = Convert.ToInt32(requestCmd.data[0]);
                var comType = Convert.ToInt32(requestCmd.data[1]);

                ComSet value = new ComSet();
                if (comType == 9)//cco
                {
                    value = freeSql.Select<ComSet>()
                    .Where(a => a.ComType == comType)
                    .ToOne();
                }
                else
                {
                    value = freeSql.Select<ComSet>()
                    .Where(a => a.MeterID == meterID && a.ComType == comType)
                    .ToOne();
                }

                ComParamter com = new ComParamter(value.ComName + "-" + value.ComPara);
                SerialManager.Instance.CreateAndOpenPort(com);
                string sdata = requestCmd.data[2];
               
                //if (comType == 1)
                //{
                //    if (sdata.Length % 2 != 0)
                //    {
                //        sdata.PadLeft(sdata.Length + 1, '0');
                //    }
                //    for (int i = 0; i < sdata.Length; i += 2)
                //    {
                //        byte b = Convert.ToByte(sdata.Substring(i, 2), 16);
                //        com.SendData.Add(b);
                //    }
                //    SerialPortService.SendData(com);
                //}
                //else
                //{
                //    for (int i = 0; i < sdata.Length; i += 2)
                //    {
                //        byte b = Convert.ToByte(sdata.Substring(i, 2), 16);
                //        com.SendData.Add(b);
                //    }

                //    SerialPortService.SendByte(com);

                //}
                if (sdata.Length % 2 != 0)
                {
                    sdata.PadLeft(sdata.Length + 1, '0');
                }
                for (int i = 0; i < sdata.Length; i += 2)
                {
                    byte b = Convert.ToByte(sdata.Substring(i, 2), 16);
                    com.SendData.Add(b);
                }
                SerialPortService.SendByte(com);
                if (com.RecData == null)
                {
                    rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, 1, null));
                }
                else
                {
                    //返回data内容：通信端口；应答帧（无空格）
                    string redata = FrameHelper.bytetostr(com.RecData.ToArray()).Replace(" ", "");
                    rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, 0, meterID+ ";" + comType +";"+ redata));
                }
                return rlt;
            }
            catch (Exception)
            {
                rlt = new ResponseCmd(requestCmd.cmd, 1, null);
            }
            return rlt;
        }
    }
}
