namespace YQ.FunctionModule
{
    public class FunctionModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("OneRegion", typeof(ListenIn));
            regionManager.RegisterViewWithRegion("TwoRegion", typeof(Setting));
            regionManager.RegisterViewWithRegion("ThreeRegion", typeof(SerialPortSetting));
            regionManager.RegisterViewWithRegion("FourRegion", typeof(SingleTest));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILogService, LogService>();
            containerRegistry.RegisterSingleton<IFreeSqlHelper>(() => new FreeSqliteHelper(ConfigurationManager.AppSettings["SqlStr"].ToString()));
        }
    }
}
