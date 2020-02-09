using System.Collections.Generic;

namespace GoalBasedMvc.Models
{
    public class NodeViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? InitialPrice { get; set; }

        public double? InitialInvestment { get; set; }

        public double? PortfolioWeight { get; set; }

        public bool IsPortfolioComponent { get; set; }

        public IList<DistributionViewModel> Distributions { get; set; }

        public NodeViewModel Parent { get; set; }
    }

}
