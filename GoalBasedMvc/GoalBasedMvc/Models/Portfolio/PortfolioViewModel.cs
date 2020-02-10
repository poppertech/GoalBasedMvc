using System.Collections.Generic;

namespace GoalBasedMvc.Models
{
    public class PortfolioViewModel
    {
        StatisticViewModel Statistics { get; set; }
        IList<HistogramViewModel> Histogram { get; set; }
        IList<double> SuccessProbabilities { get; set; }
    }
}
