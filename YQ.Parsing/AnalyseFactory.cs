using Autofac;
using YQ.Parsing;
using YQ.Parsing.DoCmdAnalyse;

namespace SECS.Parsing
{
    public class AnalyseFactory
    {
        private static readonly object lockobj = new object();
        private static AnalyseFactory _instance;
        public static AnalyseFactory Instance
        {
            get
            {
                return GetInstance();
            }
        }
        private static AnalyseFactory GetInstance()
        {
            if (_instance == null)
            {
                lock (lockobj)
                {
                    if (_instance == null)
                    {
                        _instance = new AnalyseFactory();
                    }
                }
            }
            return _instance;
        }
        private AnalyseFactory()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Cmd0101>().Named<ICmdAnalyse>("Cmd0101");
            builder.RegisterType<Cmd0102>().Named<ICmdAnalyse>("Cmd0102");
            builder.RegisterType<Cmd0103>().Named<ICmdAnalyse>("Cmd0103");
            builder.RegisterType<Cmd0104>().Named<ICmdAnalyse>("Cmd0104");
            builder.RegisterType<Cmd0105>().Named<ICmdAnalyse>("Cmd0105");
            builder.RegisterType<Cmd0106>().Named<ICmdAnalyse>("Cmd0106");
            builder.RegisterType<Cmd0107>().Named<ICmdAnalyse>("Cmd0107");
            builder.RegisterType<Cmd0108>().Named<ICmdAnalyse>("Cmd0108");
            builder.RegisterType<Cmd0109>().Named<ICmdAnalyse>("Cmd0109");
            builder.RegisterType<Cmd010A>().Named<ICmdAnalyse>("Cmd010A");
            builder.RegisterType<Cmd010B>().Named<ICmdAnalyse>("Cmd010B");
            builder.RegisterType<Cmd010C>().Named<ICmdAnalyse>("Cmd010C");
            builder.RegisterType<Cmd0201>().Named<ICmdAnalyse>("Cmd0201");
            builder.RegisterType<Cmd0202>().Named<ICmdAnalyse>("Cmd0202");
            builder.RegisterType<Cmd0203>().Named<ICmdAnalyse>("Cmd0203");
            builder.RegisterType<Cmd0301>().Named<ICmdAnalyse>("Cmd0301");
            builder.RegisterType<Cmd0401>().Named<ICmdAnalyse>("Cmd0401");
            builder.RegisterType<Cmd0402>().Named<ICmdAnalyse>("Cmd0402");
            builder.RegisterType<Cmd0403>().Named<ICmdAnalyse>("Cmd0403");
            builder.RegisterType<Cmd0404>().Named<ICmdAnalyse>("Cmd0404");
            builder.RegisterType<Cmd0405>().Named<ICmdAnalyse>("Cmd0405");
            builder.RegisterType<Cmd0406>().Named<ICmdAnalyse>("Cmd0406");
            builder.RegisterType<Cmd0407>().Named<ICmdAnalyse>("Cmd0407");
            builder.RegisterType<Cmd0408>().Named<ICmdAnalyse>("Cmd0408");
            builder.RegisterType<Cmd0409>().Named<ICmdAnalyse>("Cmd0409");
            builder.RegisterType<Cmd0410>().Named<ICmdAnalyse>("Cmd0410");
            builder.RegisterType<Cmd0411>().Named<ICmdAnalyse>("Cmd0411"); 
            builder.RegisterType<Cmd0412>().Named<ICmdAnalyse>("Cmd0412"); 
            builder.RegisterType<Cmd0413>().Named<ICmdAnalyse>("Cmd0413"); 
            builder.RegisterType<Cmd0417>().Named<ICmdAnalyse>("Cmd0417"); 
           
            container = builder.Build();
        }

        private IContainer container;

        public ICmdAnalyse GetCmdAnalyse(string curcmd)
        {
            ICmdAnalyse cmdAnalyse;
            cmdAnalyse = container.ResolveNamed<ICmdAnalyse>("Cmd" + curcmd);
            return cmdAnalyse;
        }      
    }
}
