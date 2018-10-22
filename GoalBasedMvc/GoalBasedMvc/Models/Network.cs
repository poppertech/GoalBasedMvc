using GoalBasedMvc.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalBasedMvc.Models
{
    public interface INetwork
    {
        IDictionary<int, Node> Nodes { get; }
        IPortfolio Portfolio { get; }
        IEnumerable<CashFlow> CashFlows { get; }

        void Calculate(ref IDictionary<int, Node> nodeDictionary, IList<CashFlow> cashFlows);
    }

    public class Network : INetwork
    {
        private readonly INodeSimulator _nodeSimulator;
        private readonly Stack<Node> _stack;
        private readonly IPortfolio _portfolio;

        public Network(INodeSimulator nodeSimulator, IPortfolio portfolio)
        {
            _nodeSimulator = nodeSimulator;
            _portfolio = portfolio;
            _stack = new Stack<Node>();
        }

        public IDictionary<int, Node> Nodes { get; private set; }

        public IPortfolio Portfolio { get; private set; }

        public IEnumerable<CashFlow> CashFlows { get; private set; }

        public void Calculate(ref IDictionary<int, Node> nodeDictionary, IList<CashFlow> cashFlows)
        {
            Nodes = nodeDictionary;
            _nodeSimulator.SimulateNodes(ref nodeDictionary);
            IList<Node> nodes = nodeDictionary.Values.ToList();
            _portfolio.Init(ref nodes, cashFlows);
            Portfolio = _portfolio;
            CashFlows = cashFlows;
        }


    }
}
