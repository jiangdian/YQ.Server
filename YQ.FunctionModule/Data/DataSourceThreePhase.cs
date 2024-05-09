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
                new CmdData("协议转发","cmd=1007,ret=0,data=8;1;68470043260201000000000010294707010148227f000101020412000116021a07e804120301020203110911000101020216041001f40203110911000101020216041001f400ecc716"),
                new CmdData("功耗查询-表位8","cmd=0506,data=8"),
                new CmdData("初始化","cmd=0401,data=8;1;01"),

            };
            lstDatas.AddRange(base.GetData());
            return lstDatas;
        }
    }
}
