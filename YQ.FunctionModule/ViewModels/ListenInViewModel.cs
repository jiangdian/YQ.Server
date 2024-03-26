using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YQ.FunctionModule.Data;

namespace YQ.FunctionModule.ViewModels
{
    internal class ListenInViewModel : BindableBase
    {
        private readonly IContainerExtension container;
        private readonly IEventAggregator eventAggregator;
        public List<CmdData> cmdList;
        public List<CmdData> CmdList
        {
            get { return cmdList; }
            set { SetProperty(ref cmdList, value); }
        }
        public ListenInViewModel(IEventAggregator eventAggregator, IContainerExtension container)
        {
            this.container = container;
            this.eventAggregator = eventAggregator;
            CmdList = this.container.Resolve<DataSourceThreePhase>().GetData();
        }
    }
}
