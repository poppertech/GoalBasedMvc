using System.Collections.Generic;

namespace GoalBasedMvc.Models
{
    public class NodeEditViewModel
    {
        public NodeEditViewModel()
        {
            Distributions = new List<DistributionViewModel>();
            Children = new List<NodeEditViewModel>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double? InitialPrice { get; set; }
        public double? InitialInvestment { get; set; }
        public bool IsPortfolioComponent { get; set; }

        public IList<DistributionViewModel> Distributions { get; set; }

        public IList<NodeEditViewModel> Children { get; set; }
    }
}
