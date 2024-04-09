using System;
using System.Threading;
using YQ.Tool;

namespace YQ.Parsing.DoCmdAnalyse
{

    /// <summary> 
    /// 升降源
    /// 可以指定接线方式、频率、电压、电流，相角、是否输出谐波等
    /// </summary>
    public class Cmd0203 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = null;
            Thread powerThread;             
            powerThread = new Thread(() =>
            {
                StdBuffer.Clear();                
                    PowerHelper.IsPowering = true;
                    PowerHelper.Power_Off(1);//一般情况下的升降源
                    PowerHelper.IsPowering = false;                    
            });
            powerThread.IsBackground = true;
            powerThread.Start();
            Thread.Sleep(1500);
            rlt = new ResponseCmd(requestCmd.cmd, 0, null);//
            return rlt;
        }
    }
}
