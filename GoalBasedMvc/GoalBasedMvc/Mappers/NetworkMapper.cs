using GoalBasedMvc.Models;

namespace GoalBasedMvc.Mappers
{
    public interface INetworkMapper
    {
        NetworkViewModel MapNetworkToViewModel(INetwork network);
    }

    public class NetworkMapper: INetworkMapper
    {
        private readonly INodeMapper _nodeMapper;
        private readonly IPortfolioMapper _portfolioMapper;

        public NetworkMapper(
            INodeMapper nodeMapper,
            IPortfolioMapper portfolioMapper
            ) {
            _nodeMapper = nodeMapper;
            _portfolioMapper = portfolioMapper;
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

    }
}
