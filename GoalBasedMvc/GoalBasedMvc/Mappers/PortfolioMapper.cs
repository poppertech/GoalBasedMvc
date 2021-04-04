using GoalBasedMvc.Models;

namespace GoalBasedMvc.Mappers
{
    public interface IPortfolioMapper
    {
        PortfolioViewModel MapPortfolioToViewModel(IPortfolio portfolio);
    }

    public class PortfolioMapper : IPortfolioMapper
    {
        private readonly IStatisticMapper _statisticMapper;

        public PortfolioMapper(
            IStatisticMapper statisticMapper
            )
        {
            _statisticMapper = statisticMapper;
        }
        public PortfolioViewModel MapPortfolioToViewModel(IPortfolio portfolio)
        {
            var viewModel = new PortfolioViewModel();
            viewModel.Statistics = _statisticMapper.MapStatisticToViewModel(portfolio.Statistics);
            viewModel.Histogram = portfolio.Histogram;
            viewModel.SuccessProbabilities = portfolio.SuccessProbabilities;
            return viewModel;
        }
    }
}
