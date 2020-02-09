using GoalBasedMvc.Models;
using System;
using System.Collections.Generic;

namespace GoalBasedMvc.Mappers
{
    public interface INetworkMapper
    {
        NetworkViewModel MapEntityToViewModel(INetwork network);
    }

    public class NetworkMapper: INetworkMapper
    {

        public NetworkMapper(){}

        public NetworkViewModel MapEntityToViewModel(INetwork network)
        {
            var viewModel = new NetworkViewModel();
            return viewModel;
        }

    }
}
