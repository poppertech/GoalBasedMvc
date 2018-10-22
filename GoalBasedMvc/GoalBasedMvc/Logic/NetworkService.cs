using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using System.Collections.Generic;

namespace GoalBasedMvc.Logic
{
    public interface INetworkService
    {
        IEnumerable<NetworkSearchViewModel> GetNetworks();
        INetwork GetNetworkById(int id);
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

        public IEnumerable<NetworkSearchViewModel> GetNetworks()
        {
            return _networkRepository.GetNetworks();
        }

        public INetwork GetNetworkById(int id)
        {
            IDictionary<int, Node> nodeDictionary = _nodeRepository.GetNodesByNetworkId(id);
            var cashFlows = _cashFlowRepository.GetCashFlowsByNetworkId(id);
            _network.Calculate(ref nodeDictionary, cashFlows);
            return _network;
        }
    }
}
