using System;
using YQ.FreeSQL.Entity;
using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary>
    ///  初始化台体多路服务器串口
    /// </summary>
    public class Cmd0106 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            try
            {
                #region 
                //var type = Convert.ToInt32(requestCmd.data[0]);
                //var num = 0;
                //var name = "";
                //switch ((SerialPoartEnum)type)
                //{
                //    case SerialPoartEnum.RS4851:
                //        num = Convert.ToInt32(requestCmd.data[2]) * 10 + 1;
                //        name = "RS4851";
                //        break;
                //    case SerialPoartEnum.RS4852:
                //        num = Convert.ToInt32(requestCmd.data[2]) * 10 + 2;
                //        name = "RS4852";
                //        break;
                //    case SerialPoartEnum.RS4853:
                //        num = Convert.ToInt32(requestCmd.data[2]) * 10 + 3;
                //        name = "RJ4514851";
                //        break;
                //    case SerialPoartEnum.RS4854:
                //        num = Convert.ToInt32(requestCmd.data[2]) * 10 + 4;
                //        name = "RJ4514852";
                //        break;
                //    case SerialPoartEnum.RS4855:
                //        num = Convert.ToInt32(requestCmd.data[2]) * 10 + 5;
                //        name = "RJ4524851";
                //        break;
                //    case SerialPoartEnum.RS4856:
                //        num = Convert.ToInt32(requestCmd.data[2]) * 10 + 6;
                //        name = "RJ4524852";
                //        break;
                //    case SerialPoartEnum.蓝牙:
                //        num = Convert.ToInt32(requestCmd.data[2]) * 10 + 7;
                //        name = "Bluetooth";
                //        break;
                //    case SerialPoartEnum.STA:
                //        num = Convert.ToInt32(requestCmd.data[2]) * 10 + 9;
                //        name = "EventReport";
                //        break;
                //    case SerialPoartEnum.CCO:
                //        num = 5;
                //        name = "CCO";
                //        break;
                //    default:
                //        break;
                //}
                #endregion
                var meterID = Convert.ToInt32(requestCmd.data[2]);
                var comType= Convert.ToInt32(requestCmd.data[0]);//1表示485-1，2表示485-2，3. 485-3（逆变器），4. 485-4，5. 485-5（逆变器），6. 485-6，7蓝牙，8. STA，9. CCO，其他值备用。
                var comPara = requestCmd.data[3];
                var comPro= requestCmd.data[1];
                ComSet value = new ComSet();
                if (comType==9)//cco   
                {
                    freeSql.Update<ComSet>()
                     .Set(x => x.ComPara == comPara)
                     .Set(x => x.ComPro == comPro)
                     .Where(x =>  x.ComType == comType)
                     .ExecuteAffrows();

                    value = freeSql.Select<ComSet>()
                    .Where(a => a.ComType == comType)
                    .ToOne();
                }
                else
                {
                    freeSql.Update<ComSet>()
                     .Set(x => x.ComPara == comPara)
                     .Set(x => x.ComPro == comPro)
                     .Where(x => x.MeterID == meterID && x.ComType == comType)
                     .ExecuteAffrows();

                    value = freeSql.Select<ComSet>()
                    .Where(a => a.MeterID == meterID && a.ComType == comType)
                    .ToOne();
                }
             
         
                ComParamter com = new ComParamter(value.ComName + "-" + value.ComPara);
                SerialManager.Instance.CreateAndOpenPort(com.PortName, com.BaudRate, com.Parity, com.DataBits, com.StopBits);
                rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, null, 0));
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
