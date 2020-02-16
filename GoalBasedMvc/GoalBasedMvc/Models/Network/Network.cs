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
        private readonly IPortfolio _portfolio;

        public Network(IPortfolio portfolio)
        {
            _portfolio = portfolio;
        }

        public string Url { get; set; }

        public string Name { get; set; }

        public IDictionary<int, INode> Nodes { get; set; }

        public IPortfolio Portfolio { get; private set; }

        public IList<CashFlow> CashFlows { get; set; }

        public void Calculate()
        {
            IList<INode> nodes = Nodes.Values.ToList();
            IList<CashFlow> cashFlows = CashFlows.ToList();
            _portfolio.Init(nodes, cashFlows);
            Portfolio = _portfolio;
            CashFlows = cashFlows;
        }

    }
}
