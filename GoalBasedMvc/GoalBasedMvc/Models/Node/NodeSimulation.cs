using System.Collections.Generic;

namespace GoalBasedMvc.Models
{
    public class NodeSimulation : NodeBase
    {
        public NodeSimulation(
            ):base(){}

        public IList<Simulation> Simulations { get; set; }

    }
}
