using System.Collections.Generic;

namespace GoalBasedMvc.Models
{
    public class NodeBase : INode
    {
        public NodeBase(
            )
        {
            Distributions = new List<IDistribution>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string NetworkName { get; set; }
        public string NetworkUrl { get; set; }
        public double? InitialPrice { get; set; }
        public double? InitialInvestment { get; set; }
        public double? PortfolioWeight { get; set; }
        public bool IsPortfolioComponent { get; set; }

        public IList<IDistribution> Distributions { get; set; }

        public INode Parent { get; set; }

    }
}
