﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalBasedMvc.Models
{
    public interface IPortfolio
    {
        void Init(ref IList<INode> nodes, IList<CashFlow> cashFlows);

        double InitialValue { get; }

        IList<double> SuccessProbabilities { get; }
    }

    public class Portfolio : IPortfolio
    {
        private IList<INode> _nodes;
        private IList<CashFlow> _cashFlows;

        private int _numSimulations;

        public void Init(ref IList<INode> nodes, IList<CashFlow> cashFlows)
        {
            _cashFlows = cashFlows;
            _nodes = nodes.Where(n => n.IsPortfolioComponent).ToList();
            _numSimulations = _nodes[0].Simulations.Count / (cashFlows.Count - 1);

            InitialValue = _nodes.Sum(n => n.InitialInvestment.Value);
            SuccessProbabilities = new double[_cashFlows.Count];

            InitNodeProperties();
            CalculateSuccessCounts();
            CalculateSuccessProbabilities();
        }

        public double InitialValue { get; private set; }

        public IList<double> SuccessProbabilities { get; private set; }

        private void InitNodeProperties()
        {
            foreach (var node in _nodes)
            {
                InitializeNode(node);
            }
        }

        private void InitializeNode(INode node)
        {
            var numCashFlows = _cashFlows.Count;
            node.PortfolioWeight = node.InitialInvestment / InitialValue;
            node.ValueSimulations = new double[_numSimulations, numCashFlows];
            var cumulativeSimulations = new double[_numSimulations, numCashFlows - 1];
            Parallel.For(0, node.Simulations.Count, simulationCnt =>
            {
                var portfolioCnt = simulationCnt % _numSimulations;
                var cashFlowCnt = simulationCnt / _numSimulations;
                node.ValueSimulations[portfolioCnt, 0] = node.InitialInvestment.Value;

                if(cashFlowCnt < (numCashFlows - 1))
                {
                    var simulation = node.Simulations[simulationCnt].Price;
                    var cumulativeSimulation = simulation / node.InitialPrice;
                    cumulativeSimulations[portfolioCnt, cashFlowCnt] = cumulativeSimulation.Value;
                }
            });
            node.CumulativeSimulations = cumulativeSimulations;
        }

    private void CalculateSuccessCounts()
    {
        var portfolioSimulations = new double[_numSimulations][];
        for (int portfolioCnt = 0; portfolioCnt < _numSimulations; portfolioCnt++)
        {
            portfolioSimulations[portfolioCnt] = new double[_cashFlows.Count];
            for (int periodCnt = 0; periodCnt < _cashFlows.Count; periodCnt++)
            {
                var portfolioValue = _nodes.Sum(n => n.ValueSimulations[portfolioCnt, periodCnt] * n.PortfolioWeight);
                var portfolioValueNet = portfolioValue - _cashFlows[periodCnt].Cost;
                portfolioValueNet = portfolioValueNet > 0 ? portfolioValueNet : 0;
                portfolioSimulations[portfolioCnt][periodCnt] = portfolioValueNet.Value;
                if (periodCnt < _cashFlows.Count - 1)
                    foreach (var node in _nodes)
                        node.ValueSimulations[portfolioCnt, periodCnt + 1] = node.CumulativeSimulations[portfolioCnt, periodCnt] * portfolioValueNet.Value * node.PortfolioWeight.Value;

                SuccessProbabilities[periodCnt] = portfolioValueNet > 0 ? SuccessProbabilities[periodCnt] + 1 : SuccessProbabilities[periodCnt];
            }
        }
    }

    private void CalculateSuccessProbabilities()
    {
        for (int periodCnt = 0; periodCnt < _cashFlows.Count; periodCnt++)
        {
            SuccessProbabilities[periodCnt] = SuccessProbabilities[periodCnt] / _numSimulations;
        }
    }

}
}
