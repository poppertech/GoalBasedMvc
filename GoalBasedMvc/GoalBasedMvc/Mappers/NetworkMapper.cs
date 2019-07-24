using GoalBasedMvc.Models;
using System;
using System.Collections.Generic;

namespace GoalBasedMvc.Mappers
{
    public interface INetworkMapper
    {
        INetwork MapViewModelToEntity(NetworkEditViewModel viewModel);
    }

    public class NetworkMapper: INetworkMapper
    {
        private readonly INetwork _network;
        private readonly Func<INode> _nodeFactory;
        private readonly Func<DistributionContext, IDistribution> _distributionFactory;

        public NetworkMapper(
            INetwork network,
            Func<INode> nodeFactory,
            Func<DistributionContext, IDistribution> distributionFactory
            )
        {
            _network = network;
            _nodeFactory = nodeFactory;
            _distributionFactory = distributionFactory;
        }

        public INetwork MapViewModelToEntity(NetworkEditViewModel viewModel)
        {
            _network.CashFlows = viewModel.CashFlows;
            _network.Nodes = CreateNodeDictionary(viewModel.Nodes);
            return _network;
        }

        private IDictionary<int, INode> CreateNodeDictionary(IDictionary<int, NodeEditViewModel> viewModelDictionary)
        {
            _network.Nodes = new SortedDictionary<int, INode>();
            foreach (var key in viewModelDictionary.Keys)
            {
                var viewModel = viewModelDictionary[key];
                var entity = MapNodeViewModelToEntity(viewModel);
                _network.Nodes.Add(key, entity);
            }
            return _network.Nodes;
        }

        private INode MapNodeViewModelToEntity(NodeEditViewModel viewModel)
        {
            var node = _nodeFactory();
            node.Id = viewModel.Id;
            node.Name = viewModel.Name;
            node.InitialPrice = viewModel.InitialPrice;
            node.InitialInvestment = viewModel.InitialInvestment;
            node.PortfolioWeight = viewModel.PortfolioWeight;
            node.IsPortfolioComponent = viewModel.IsPortfolioComponent;
            node.Parent = viewModel.Parent != null ? _network.Nodes[viewModel.Parent.Id]: null;
            node.Distributions = MapDistributionViewModelsToEntities(viewModel.Distributions);
            return node;
        }

        private IList<IDistribution> MapDistributionViewModelsToEntities(IList<DistributionEditViewModel> viewModels)
        {
            var distributions = new IDistribution[viewModels.Count];
            for (int cnt = 0; cnt < viewModels.Count; cnt++)
            {
                var viewModel = viewModels[cnt];
                distributions[cnt] = MapDistributionViewModelToEntity(viewModel);
            }
            return distributions;
        }

        private IDistribution MapDistributionViewModelToEntity(DistributionEditViewModel viewModel)
        {
            var context = new DistributionContext();
            context.Id = viewModel.Id;
            context.Minimum = viewModel.Minimum;
            context.Worst = viewModel.Worst;
            context.Likely = viewModel.Likely;
            context.Best = viewModel.Best;
            context.Maximum = viewModel.Maximum;
            var distibution = _distributionFactory(context);
            return distibution;
        }
    }
}
