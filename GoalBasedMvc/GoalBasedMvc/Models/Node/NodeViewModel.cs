using System.Collections.Generic;

namespace GoalBasedMvc.Models
{
    public class NodeViewModel
    {
        int Id { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        string NetworkName { get; set; }
        string NetworkUrl { get; set; }
        double? InitialPrice { get; set; }
        double? InitialInvestment { get; set; }
        double? PortfolioWeight { get; set; }
        bool IsPortfolioComponent { get; set; }

        IList<DistributionViewModel> Distributions { get; set; }
        StatisticViewModel Statistics { get; }
        HistogramViewModel Histogram { get; }

        NodeViewModel Parent { get; set; }
    }
}
