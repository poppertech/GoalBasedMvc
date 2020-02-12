using GoalBasedMvc.Mappers;
using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Logic
{
    public interface INetworkService
    {
        IEnumerable<NetworkRecord> GetNetworks();
        NetworkViewModel GetNetworkByUrl(string url);
    }

    public class NetworkService: INetworkService
    {
        private readonly INetworkRepository _networkRepository;
        private readonly INodeRepository _nodeRepository;
        private readonly ICashFlowRepository _cashFlowRepository;
        private readonly INetwork _network;
        private readonly INetworkMapper _networkMapper;
        private readonly INodeMapper _nodeMapper;

        public NetworkService(
            INetworkRepository networkRepository, 
            INodeRepository nodeRepository, 
            ICashFlowRepository cashFlowRepository, 
            INetwork network,
            INetworkMapper mapper,
            INodeMapper nodeMapper
            )
        {
            _networkRepository = networkRepository;
            _nodeRepository = nodeRepository;
            _cashFlowRepository = cashFlowRepository;
            _network = network;
            _networkMapper = mapper;
            _nodeMapper = nodeMapper;
        }

        public IEnumerable<NetworkRecord> GetNetworks()
        {
            return _networkRepository.GetNetworks();
        }

        public NetworkViewModel GetNetworkByUrl(string url)
        {
            var network = _networkRepository.GetNetworks(url).Single();
            _network.Name = network.Name;
            _network.Url = network.Url;
             var nodeRecords = _nodeRepository.GetNodesByNetworkId(network.Id);
            _network.Nodes = _nodeMapper.MapNodeRecordsToNodes(nodeRecords);
            _network.CashFlows = _cashFlowRepository.GetCashFlowsByNetworkId(network.Id);
            _network.Calculate();
            var networkViewModel = _networkMapper.MapNetworkToViewModel(_network);
            return networkViewModel;
        }

    }
}
