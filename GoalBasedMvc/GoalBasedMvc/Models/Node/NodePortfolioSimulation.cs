namespace GoalBasedMvc.Models.Node
{
    public class NodePortfolioSimulation:NodeBase
    {
        public NodePortfolioSimulation():base(){}

        public double[,] CumulativeSimulations { get; set; }

        public double[,] ValueSimulations { get; set; }
    }
}
