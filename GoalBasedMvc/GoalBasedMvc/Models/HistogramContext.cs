using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalBasedMvc.Models
{
    public class HistogramContext
    {
        public IList<double> Simulations { get; set; }
        public double GlobalXMin { get; set; }
        public double GlobalXMax { get; set; }
    }
}
