using Autofac;
using GoalBasedMvc.Logic;
using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using System.Linq;

namespace GoalBasedMvc
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CashFlowRepository>().As<ICashFlowRepository>();
            builder.RegisterType<NodeRepository>().As<INodeRepository>();
            builder.RegisterType<UniformRandomRepository>().As<IUniformRandomRepository>();
            builder.RegisterType<NetworkRepository>().As<INetworkRepository>();

            builder.RegisterType<NetworkService>().As<INetworkService>();

            builder.RegisterType<NodeSimulator>().As<INodeSimulator>();
            builder.RegisterType<SimulationEvaluator>().As<ISimulationEvaluator>();

            builder.RegisterType<Portfolio>().As<IPortfolio>();
            builder.RegisterType<Network>().As<INetwork>();
            builder.RegisterType<Statistic>().As<IStatistic>();
            builder.Register<INode>((c, p) => new Node(c.Resolve<IStatistic>()));
            builder.Register<IDistribution>((c, p) =>
            {
                var parameter = p.First() as TypedParameter;
                return new Distribution(parameter.Value as DistributionContext);
            });

            base.Load(builder);
        }
    }
}
