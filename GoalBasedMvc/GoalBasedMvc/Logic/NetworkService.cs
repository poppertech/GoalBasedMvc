using GoalBasedMvc.Mappers;
using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Logic
{
    public interface INetworkService
    {
        IEnumerable<NetworkRecord> GetNetworks();
        NetworkViewModel GetNetworkByUrl(string url);
    }

    public class NetworkService : INetworkService
    {
        private readonly INetworkRepository _networkRepository;
        private readonly INodeRepository _nodeRepository;
        private readonly ICashFlowRepository _cashFlowRepository;
        private readonly INetwork _network;
        private readonly INodeSimulator _nodeSimulator;
        private readonly INetworkMapper _networkMapper;
        private readonly INodeMapper _nodeMapper;
        private readonly IMemoryCache _cache;

        public NetworkService(
            INetworkRepository networkRepository,
            INodeRepository nodeRepository,
            ICashFlowRepository cashFlowRepository,
            INetwork network,
            INodeSimulator nodeSimulator,
        INetworkMapper mapper,
            INodeMapper nodeMapper,
            IMemoryCache cache
            )
        {
            _networkRepository = networkRepository;
            _nodeRepository = nodeRepository;
            _cashFlowRepository = cashFlowRepository;
            _network = network;
            _nodeSimulator = nodeSimulator;
            _networkMapper = mapper;
            _nodeMapper = nodeMapper;
            _cache = cache;
        }

        public IEnumerable<NetworkRecord> GetNetworks()
        {
            return _networkRepository.GetNetworks();
        }

        public NetworkViewModel GetNetworkByUrl(string url)
        {
            NetworkViewModel networkViewModel;
            if (!_cache.TryGetValue(url, out networkViewModel))
            {
                var network = _networkRepository.GetNetworks(url).Single();
                _network.Name = network.Name;
                _network.Url = network.Url;
                var nodeRecords = _nodeRepository.GetNodesByNetworkId(network.Id);
                var nodes = _nodeMapper.MapNodeRecordsToNodes(nodeRecords);
                _network.Nodes = _nodeSimulator.SimulateNodes(nodes);
                _network.CashFlows = _cashFlowRepository.GetCashFlowsByNetworkId(network.Id);
                _network.Calculate();
                networkViewModel = _networkMapper.MapNetworkToViewModel(_network);
                var entryOptions = new MemoryCacheEntryOptions();
                _cache.Set(url, networkViewModel, entryOptions);
            }
            return networkViewModel;
        }

    }
}
