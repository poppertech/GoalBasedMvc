using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalBasedMvc.Models
{
    public class NetworkEditViewModel
    {
        public SortedDictionary<int, Node> Nodes { get; set; }
        public IList<CashFlow> CashFlows { get; set; }
    }
}
