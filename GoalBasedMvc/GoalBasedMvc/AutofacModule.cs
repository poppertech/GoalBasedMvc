using Autofac;
using GoalBasedMvc.Logic;
using GoalBasedMvc.Repository;

namespace GoalBasedMvc
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<CashFlowRepository>().As<ICashFlowRepository>();
            //builder.RegisterType<NodeRepository>().As<INodeRepository>();
            //builder.RegisterType<UniformRandomRepository>().As<IUniformRandomRepository>();
            builder.RegisterType<NetworkRepository>().As<INetworkRepository>();

            builder.RegisterType<NetworkService>().As<INetworkService>();

            //builder.RegisterType<NodeSimulator>().As<INodeSimulator>();
            //builder.RegisterType<SimulationEvaluator>().As<ISimulationEvaluator>();

            //builder.RegisterType<Portfolio>().As<IPortfolio>();
            //builder.RegisterType<Network>().As<INetwork>();

            base.Load(builder);
        }
    }
}
