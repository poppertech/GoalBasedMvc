using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Logic
{
    public interface INetworkService
    {
        IEnumerable<NetworkViewModel> GetNetworks();
        INetwork GetNetworkByUrl(string url);
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
            IDictionary<int, Node> nodeDictionary = _nodeRepository.GetNodesByNetworkId(network.Id);
            var cashFlows = _cashFlowRepository.GetCashFlowsByNetworkId(network.Id);
            _network.Calculate(ref nodeDictionary, cashFlows);
            return _network;
        }
    }
}
