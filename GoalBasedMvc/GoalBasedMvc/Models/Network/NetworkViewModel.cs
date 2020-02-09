using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public class NetworkViewModel
    {
        public SortedDictionary<int, NodeViewModel> Nodes { get; set; }

        public IList<CashFlow> CashFlows { get; set; }

    }


}
