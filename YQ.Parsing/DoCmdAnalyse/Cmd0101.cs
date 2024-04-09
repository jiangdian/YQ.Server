namespace YQ.Parsing.DoCmdAnalyse
{
    /// <summary> 
    ///连接台体多路服务器
    /// </summary>
    public class Cmd0101 : AbstractCmdAnalyse
    {
        public override AbstractCmd GetResponseCmd(AbstractCmd requestCmd)
        {
            AbstractCmd rlt = rlt = new ResponseCmd(GetCmdString(requestCmd.cmd, null, 0));
            return rlt;
        }
    }
}
