﻿using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Logic
{
    public interface INetworkService
    {
        IEnumerable<NetworkViewModel> GetNetworks();
        INetwork GetNetworkByUrl(string url);
        INetwork CalculateNetwork(NetworkEditViewModel viewModel);
    }

    public class NetworkService: INetworkService
    {
        private readonly INetworkRepository _networkRepository;
        private readonly INodeRepository _nodeRepository;
        private readonly ICashFlowRepository _cashFlowRepository;
        private readonly INetwork _network;

        public NetworkService(
            INetworkRepository networkRepository, 
            INodeRepository nodeRepository, 
            ICashFlowRepository cashFlowRepository, 
            INetwork network
            )
        {
            _networkRepository = networkRepository;
            _nodeRepository = nodeRepository;
            _cashFlowRepository = cashFlowRepository;
            _network = network;
        }

        public IEnumerable<NetworkViewModel> GetNetworks()
        {
            return _networkRepository.GetNetworks();
        }

        public INetwork GetNetworkByUrl(string url)
        {
            var network = _networkRepository.GetNetworks(url).Single();
            _network.Nodes = _nodeRepository.GetNodesByNetworkId(network.Id);
            _network.CashFlows = _cashFlowRepository.GetCashFlowsByNetworkId(network.Id);
            _network.Calculate();
            return _network;
        }

        public INetwork CalculateNetwork(NetworkEditViewModel viewModel)
        {
            var nodes = viewModel.Nodes;
            var keys = nodes.Keys.ToList();
            for (int cnt = 0; cnt < keys.Count; cnt++)
            {
                var key = keys[cnt];
                var node = nodes[key];
                node.Parent = node.Parent != null ? nodes[node.Parent.Id] : null;
            }
            _network.CashFlows = viewModel.CashFlows;
            _network.Nodes = viewModel.Nodes;
            _network.Calculate();
            return _network;
        }
    }
}
