using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public class NodeStatistics:NodeSimulation
    {
        private readonly IStatistic _statistic;
        private readonly IHistogram _histogram;

        public NodeStatistics(
            IStatistic statistic,
            IHistogram histogram
            ) :base()
        {
            _statistic = statistic;
            _histogram = histogram;
        }

        public IStatistic Statistics
        {
            get
            {
                var prices = Simulations.Select(s => s.Price).ToList();
                _statistic.Init(prices);
                return _statistic;
            }
        }

        public IList<HistogramDatum> Histogram
        {
            get
            {
                var context = new HistogramContext();
                context.Simulations = Simulations.Select(s => s.Price).ToList();
                context.GlobalXMin = Distributions.Min(d => d.Minimum);
                context.GlobalXMax = Distributions.Max(d => d.Maximum);
                var data = _histogram.GetHistogramData(context);
                return data;
            }
        }
    }
}
