using GoalBasedMvc.Models;
using System;
using System.Collections.Generic;

namespace GoalBasedMvc.Mappers
{
    public interface INetworkMapper
    {
        NetworkViewModel MapNetworkToViewModel(INetwork network);
        INetwork MapNetworkComponentsToNetwork(NetworkRecord networkRecord, IDictionary<int, INode> nodes, IList<CashFlow> cashFlows);
    }

    public class NetworkMapper: INetworkMapper
    {
        private readonly INodeMapper _nodeMapper;
        private readonly IPortfolioMapper _portfolioMapper;
        private readonly Func<INetwork> _networkFactory;

        public NetworkMapper(
            INodeMapper nodeMapper,
            IPortfolioMapper portfolioMapper,
            Func<INetwork> networkFactory
            ) {
            _nodeMapper = nodeMapper;
            _portfolioMapper = portfolioMapper;
            _networkFactory = networkFactory;
        }

        public NetworkViewModel MapNetworkToViewModel(INetwork network)
        {
            var viewModel = new NetworkViewModel();
            viewModel.Url = network.Url;
            viewModel.Name = network.Name;
            viewModel.Nodes = _nodeMapper.MapNodesToViewModels(network.Nodes);
            viewModel.Portfolio = _portfolioMapper.MapPortfolioToViewModel(network.Portfolio);
            viewModel.CashFlows = network.CashFlows;
            return viewModel;
        }

        public INetwork MapNetworkComponentsToNetwork(NetworkRecord networkRecord, IDictionary<int, INode> nodes, IList<CashFlow> cashFlows)
        {
            var network = _networkFactory();
            network.Name = networkRecord.Name;
            network.Url = networkRecord.Url;
            network.Nodes = nodes;
            network.CashFlows = cashFlows;
            return network;
        }

    }
}
