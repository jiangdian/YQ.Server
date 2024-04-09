namespace YQ.FunctionModule.Data
{
    public class DataSourceThreePhase : DataSource
    {
        public override List<CmdData> GetData()
        {
            List<CmdData> lstDatas = new List<CmdData>
            {
                new CmdData("关源", "cmd=0203,data=null"),
                new CmdData("上电压220(常用)", "cmd=0201,data=0;7;220;0;0;50;0"),
                new CmdData("上电压220(高级)", "cmd=0202,data=0;7;50;220;0;220;0;220;0;0;0;0;0;0;0;0"),
            };
            lstDatas.AddRange(base.GetData());
            return lstDatas;
        }
    }
}
