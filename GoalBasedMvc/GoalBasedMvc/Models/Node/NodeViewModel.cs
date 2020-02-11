using System.Collections.Generic;

namespace GoalBasedMvc.Models
{
    public class NodeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string NetworkName { get; set; }
        public string NetworkUrl { get; set; }
        public double? InitialPrice { get; set; }
        public double? InitialInvestment { get; set; }
        public double? PortfolioWeight { get; set; }
        public bool IsPortfolioComponent { get; set; }

        public IList<DistributionViewModel> Distributions { get; set; }
        public StatisticViewModel Statistics { get; set; }
        public IList<HistogramDatum> Histogram { get; set; }
        public NodeViewModel Parent { get; set; }
    }
}
