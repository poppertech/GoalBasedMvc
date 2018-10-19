using System.Collections.Generic;

namespace GoalBasedMvc.Models
{

    public class NetworkEditViewModel
    {
        public int Id { get; set; }
        public IList<NodeViewModel> Nodes { get; set; }
        public PortfolioViewModel Portfolio { get; set; }
        public IList<double> CashFlows { get; set; }
    }
}
