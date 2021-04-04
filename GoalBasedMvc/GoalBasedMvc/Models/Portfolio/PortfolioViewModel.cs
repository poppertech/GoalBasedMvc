using System.Collections.Generic;

namespace GoalBasedMvc.Models
{
    public class PortfolioViewModel
    {
        public StatisticViewModel Statistics { get; set; }
        public IList<HistogramDatum> Histogram { get; set; }
        public IList<double> SuccessProbabilities { get; set; }
    }
}
