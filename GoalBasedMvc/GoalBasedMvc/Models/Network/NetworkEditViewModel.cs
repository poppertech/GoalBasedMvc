﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public class NetworkEditViewModel
    {
        public SortedDictionary<int, NodeEditViewModel> Nodes { get; set; }

        public IList<CashFlow> CashFlows { get; set; }

    }


}
