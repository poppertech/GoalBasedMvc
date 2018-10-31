using GoalBasedMvc.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalBasedMvc.Models
{
    public interface INetwork
    {
        IDictionary<int, Node> Nodes { get; set; }
        IPortfolio Portfolio { get; }
        IEnumerable<CashFlow> CashFlows { get; set; }

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

        public IDictionary<int, Node> Nodes { get; set; }

        public IPortfolio Portfolio { get; private set; }

        public IEnumerable<CashFlow> CashFlows { get; set; }

        public void Calculate()
        {
            _nodeSimulator.SimulateNodes(Nodes);
            IList<Node> nodes = Nodes.Values.ToList();
            IList<CashFlow> cashFlows = CashFlows.ToList();
            _portfolio.Init(ref nodes, cashFlows);
            Portfolio = _portfolio;
            CashFlows = cashFlows;
        }

    }
}
