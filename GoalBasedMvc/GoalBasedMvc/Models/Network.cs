using GoalBasedMvc.Logic;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public interface INetwork
    {
        IDictionary<int, INode> Nodes { get; set; }
        IPortfolio Portfolio { get; }
        IList<CashFlow> CashFlows { get; set; }

        void Calculate();
    }

    public class Network : INetwork
    {
        private readonly INodeSimulator _nodeSimulator;
        private readonly IPortfolio _portfolio;

        public Network(INodeSimulator nodeSimulator, IPortfolio portfolio)
        {
            _nodeSimulator = nodeSimulator;
            _portfolio = portfolio;
        }

        public IDictionary<int, INode> Nodes { get; set; }

        public IPortfolio Portfolio { get; private set; }

        public IList<CashFlow> CashFlows { get; set; }

        public void Calculate()
        {
            Nodes = _nodeSimulator.SimulateNodes(Nodes);
            IList<INode> nodes = Nodes.Values.ToList();
            IList<CashFlow> cashFlows = CashFlows.ToList();
            _portfolio.Init(ref nodes, cashFlows);
            Portfolio = _portfolio;
            CashFlows = cashFlows;
        }

    }
}
