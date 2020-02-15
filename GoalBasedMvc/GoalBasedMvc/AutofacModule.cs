﻿using Autofac;
using GoalBasedMvc.Logic;
using GoalBasedMvc.Mappers;
using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using Microsoft.Extensions.Caching.Memory;
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
            builder.RegisterType<NodeService>().As<INodeService>();

            builder.RegisterType<NetworkMapper>().As<INetworkMapper>();
            builder.RegisterType<NodeMapper>().As<INodeMapper>();
            builder.RegisterType<PortfolioMapper>().As<IPortfolioMapper>();
            builder.RegisterType<DistributionMapper>().As<IDistributionMapper>();
            builder.RegisterType<StatisticMapper>().As<IStatisticMapper>();

            builder.RegisterType<NodeSimulator>().As<INodeSimulator>();

            builder.RegisterType<Portfolio>().As<IPortfolio>();
            builder.RegisterType<Network>().As<INetwork>();
            builder.RegisterType<Statistic>().As<IStatistic>();
            builder.RegisterType<Histogram>().As<IHistogram>();
            builder.Register<INode>((c, p) => new Node(c.Resolve<IStatistic>(), c.Resolve<IHistogram>()));
            builder.Register<IDistribution>((c, p) =>
            {
                var parameter = p.First() as TypedParameter;
                return new Distribution(parameter.Value as DistributionRecord);
            });

            builder.Register<IMemoryCache>((c, p) => {
                var options = new MemoryCacheOptions();
                return new MemoryCache(options);
            }).SingleInstance();

            base.Load(builder);
        }
    }
}
