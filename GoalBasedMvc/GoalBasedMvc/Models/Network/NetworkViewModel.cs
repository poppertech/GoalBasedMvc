using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public class NetworkViewModel
    {
        string Url { get; set; }
        string Name { get; set; }
        IDictionary<int, NodeViewModel> Nodes { get; set; }
        PortfolioViewModel Portfolio { get; }
        IList<CashFlow> CashFlows { get; set; }

    }


}
