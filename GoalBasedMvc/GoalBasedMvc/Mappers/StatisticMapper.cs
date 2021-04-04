using GoalBasedMvc.Models;

namespace GoalBasedMvc.Mappers
{
    public interface IStatisticMapper
    {
        StatisticViewModel MapStatisticToViewModel(IStatistic statistics);
    }

    public class StatisticMapper : IStatisticMapper
    {
        public StatisticViewModel MapStatisticToViewModel(IStatistic statistics)
        {
            var viewModel = new StatisticViewModel();
            viewModel.Mean = statistics.Mean;
            viewModel.Stdev = statistics.Stdev;
            viewModel.Skew = statistics.Skew;
            viewModel.Kurt = statistics.Kurt;
            return viewModel;
        }
    }
}
