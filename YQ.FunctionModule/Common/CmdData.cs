namespace YQ.FunctionModule.Common
{
    /// <summary>
    /// 命令数据
    /// </summary>
    public class CmdData
    {
        public CmdData(string name, string msg)
        {
            this.Name = name;
            this.Msg = msg;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 命令
        /// </summary>
        public string Msg { get; set; }
    }
}
