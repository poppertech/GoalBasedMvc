using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoalBasedMvc.Models
{
    public class NodeEditViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? InitialPrice { get; set; }

        public double? InitialInvestment { get; set; }

        public double? PortfolioWeight { get; set; }

        public bool IsPortfolioComponent { get; set; }

        public IList<DistributionEditViewModel> Distributions { get; set; }

        public NodeEditViewModel Parent { get; set; }
    }

}
