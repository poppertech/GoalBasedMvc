using System.Collections.Generic;

namespace GoalBasedMvc.Models
{

    public class NetworkEditViewModel
    {
        public IList<NodeViewModel> Nodes { get; set; }
        public PortfolioViewModel Portfolio { get; set; }
        public IList<CashFlowViewModel> CashFlows { get; set; }
    }
}
