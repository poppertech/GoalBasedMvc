using GoalBasedMvc.Logic;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public interface INetwork
    {
        string Url { get; set; }
        string Name { get; set; }
        IDictionary<int, INode> Nodes { get; set; }
        IPortfolio Portfolio { get; }
        IList<CashFlow> CashFlows { get; set; }

        void Calculate();
    }

    public class Network : INetwork
    {
        private readonly INodeSimulator _nodeSimulator;

        public Network(
            IPortfolio portfolio, 
            INodeSimulator nodeSimulator
            )
        {
            Portfolio = portfolio;
            _nodeSimulator = nodeSimulator;
        }

        public string Url { get; set; }

        public string Name { get; set; }

        public IDictionary<int, INode> Nodes { get; set; }

        public IPortfolio Portfolio { get;}

        public IList<CashFlow> CashFlows { get; set; }

        public void Calculate()
        {
            Nodes = SimulateNodes(Nodes);
            IList<INode> nodes = Nodes.Values.ToList();
            IList<CashFlow> cashFlows = CashFlows.ToList();
            IList<INode> portfolioNodes = nodes.Where(n => n.IsPortfolioComponent).ToList();
            portfolioNodes = SetPortfolio(portfolioNodes, Portfolio);
            Portfolio.Init(portfolioNodes, cashFlows);
        }

        private IDictionary<int, INode> SimulateNodes(IDictionary<int, INode> nodes)
        {
            var nodeSimulations = _nodeSimulator.SimulateNodes(nodes);
            foreach (var key in nodes.Keys)
            {
                nodes[key].Simulations = nodeSimulations[key];
            }
            return nodes;
        }

        private IList<INode> SetPortfolio(IList<INode> portfolioNodes, IPortfolio portfolio)
        {
            foreach (var node in portfolioNodes)
            {
                node.Portfolio = portfolio;
            }

            return portfolioNodes;
        }

    }
}
