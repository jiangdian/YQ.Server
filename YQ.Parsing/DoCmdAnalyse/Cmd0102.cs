using System;
using YQ.FreeSQL.Entity;
using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary>
    /// 初始化台体多路服务器串口
    /// </summary>
    public class Cmd0102 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            ComParamter com = new ComParamter(requestCmd.data[1]);
            SerialManager.Instance.CreateAndOpenPort(com.PortName, com.BaudRate, com.Parity, com.DataBits, com.StopBits);
            AbstractCmd rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, null, 0));
            return rlt;           
        }
    }
}
