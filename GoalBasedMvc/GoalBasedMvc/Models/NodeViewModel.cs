using System.Collections.Generic;

namespace GoalBasedMvc.Models
{
    public class NodeViewModel
    {
        public NodeViewModel()
        {
            Distributions = new List<DistributionViewModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double? InitialPrice { get; set; }
        public double? InitialInvestment { get; set; }
        public bool IsPortfolioComponent { get; set; }

        public IList<DistributionViewModel> Distributions { get; set; }

        public string Parent { get; set; }
    }
}
