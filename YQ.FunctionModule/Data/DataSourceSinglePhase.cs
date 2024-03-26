namespace YQ.FunctionModule.Data
{
    public class DataSourceSinglePhase : DataSource
    {
        public override List<CmdData> GetData()
        {
            List<CmdData> lstDatas = new List<CmdData>
            {
                new CmdData("下电", "cmd=0201,data=0;0;50;0;0;0;0;0;0;0;0;0;0;0;0;0"),
                new CmdData("上电压", "cmd=0201,data=0;1;50;220;0;0;0;0;0;0;0;0;0;0;0;0"),
                new CmdData("上5A电流", "cmd=0201,data=0;9;50;220;0;0;0;0;0;5.0;0;0;0;0;0;0"),
                new CmdData("上30A电流", "cmd=0201,data=0;9;50;220;0;0;0;0;0;30.0;0;0;0;0;0;0")
            };
            lstDatas.AddRange(base.GetData());
            return lstDatas;
        }
    }
}
