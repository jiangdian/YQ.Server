using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YQ.FunctionModule.Views;

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
        }
    }
}
