using GoalBasedMvc.Models;
using System;
using System.Collections.Generic;

namespace GoalBasedMvc.Mappers
{
    public interface INetworkMapper
    {
        INetwork MapViewModelToEntity(NetworkEditViewModel viewModel);
        NetworkEditViewModel MapEntityToViewModel(INetwork entity);
    }

    public class NetworkMapper: INetworkMapper
    {
        private readonly INetwork _network;
        private readonly Func<INode> _nodeFactory;
        private readonly Func<DistributionContext, IDistribution> _distributionFactory;

        private NetworkEditViewModel _networkViewModel;

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

        public NetworkEditViewModel MapEntityToViewModel(INetwork entity)
        {
            _networkViewModel = new NetworkEditViewModel();
            _networkViewModel.CashFlows = entity.CashFlows;
            _networkViewModel.Nodes = CreateNodeViewModelDictionary(entity.Nodes);
            return _networkViewModel;
        }

        public INetwork MapViewModelToEntity(NetworkEditViewModel viewModel)
        {
            _network.CashFlows = viewModel.CashFlows;
            _network.Nodes = CreateNodeDictionary(viewModel.Nodes);
            return _network;
        }

        private SortedDictionary<int, NodeEditViewModel> CreateNodeViewModelDictionary(IDictionary<int, INode> entityDictionary)
        {
            var viewModelDictionary = new SortedDictionary<int, NodeEditViewModel>();
            foreach (var key in entityDictionary.Keys)
            {
                var entity = entityDictionary[key];
                var viewModel = MapNodeEntityToViewModel(entity);
                viewModelDictionary.Add(key, viewModel);
            }
            return viewModelDictionary;
        }

        private NodeEditViewModel MapNodeEntityToViewModel(INode entity)
        {
            var viewModel = new NodeEditViewModel();
            viewModel.Id = entity.Id;
            viewModel.Name = entity.Name;
            viewModel.InitialPrice = entity.InitialPrice;
            viewModel.InitialInvestment = entity.InitialInvestment;
            viewModel.PortfolioWeight = entity.PortfolioWeight;
            viewModel.IsPortfolioComponent = entity.IsPortfolioComponent;
            viewModel.Parent = entity.Parent != null ? _networkViewModel.Nodes[entity.Parent.Id] : null;
            viewModel.Distributions = MapDistributionsToViewModels(entity.Distributions);
            return viewModel;
        }

        private IList<DistributionEditViewModel> MapDistributionsToViewModels(IList<IDistribution> entities)
        {
            var viewModels = new DistributionEditViewModel[entities.Count];
            for (int cnt = 0; cnt < entities.Count; cnt++)
            {
                var entity = entities[cnt];
                var viewModel = MapDistributionToViewModel(entity);
                viewModels[cnt] = viewModel;
            }
            return viewModels;
        }

        private DistributionEditViewModel MapDistributionToViewModel(IDistribution entity)
        {
            var viewModel = new DistributionEditViewModel();
            viewModel.Id = entity.Id;
            viewModel.Minimum = entity.Minimum;
            viewModel.Worst = entity.Worst;
            viewModel.Likely = entity.Likely;
            viewModel.Best = entity.Best;
            viewModel.Maximum = entity.Maximum;
            return viewModel;
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
