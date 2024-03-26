namespace YQ.FunctionModule.Data
{
    public class DataSourceThreePhase : DataSource
    {
        public override List<CmdData> GetData()
        {
            List<CmdData> lstDatas = new List<CmdData>
            {
                new CmdData("下电", "cmd=0201,data=0;0;50;0;0;0;120;0;240;0;0;0;0;0;0;0"),
                new CmdData("上电压220", "cmd=0201,data=0;7;50;220;0;220;120;220;240;0;0;0;0;0;0;0"),
                new CmdData("上电压-三相三线", "cmd=0201,data=2;5;50;100;0;0;0;100;300;0;0;0;0;0;0;0"),
                new CmdData("上5A电流", "cmd=0201,data=0;63;50;220;0;220;120;220;240;5.0;0;5.0;120;5.0;240;0"),
                new CmdData("上1.5A电流-三相三线", "cmd=0201,data=2;45;50;100;0;0;0;100;300;1.5;30;0;0;1.5;270;0"),
                new CmdData("上5A电流-三相三线", "cmd=0201,data=2;45;50;100;0;0;0;100;300;5;30;0;0;5;270;0"),
                new CmdData("上30A电流", "cmd=0201,data=0;63;50;220;0;220;120;220;240;30.0;0;30.0;120;30.0;240;0")
            };
            lstDatas.AddRange(base.GetData());
            return lstDatas;
        }
    }
}
