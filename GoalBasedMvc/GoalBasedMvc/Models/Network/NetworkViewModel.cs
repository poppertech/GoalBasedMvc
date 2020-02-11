using System.Collections.Generic;

namespace GoalBasedMvc.Models
{
    public class NetworkViewModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public IDictionary<int, NodeViewModel> Nodes { get; set; }
        public PortfolioViewModel Portfolio { get; set; }
        public IList<CashFlow> CashFlows { get; set; }

    }


}
