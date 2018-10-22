using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using System.Collections.Generic;

namespace GoalBasedMvc.Logic
{
    public interface INetworkService
    {
        IEnumerable<NetworkSearchViewModel> GetNetworks();

    }

    public class NetworkService: INetworkService
    {
        private readonly INetworkRepository _networkRepository;

        public NetworkService(INetworkRepository networkRepository)
        {
            _networkRepository = networkRepository;
        }

        public IEnumerable<NetworkSearchViewModel> GetNetworks()
        {
            return _networkRepository.GetNetworks();
        }
    }
}
