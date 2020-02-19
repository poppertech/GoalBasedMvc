using GoalBasedMvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalBasedMvc.Logic
{
    public interface IPortfolioSimulator
    {
        IList<double> CalculateSuccessProbabilities(IList<INode> nodes, IList<CashFlow> cashFlows);
    }

    public class PortfolioSimulator : IPortfolioSimulator
    {
        private readonly IDictionary<int, double[,]> _cumulativeSimulations = new Dictionary<int, double[,]>();
        private readonly IDictionary<int, double[,]> _valueSimulations = new Dictionary<int, double[,]>();

        public IList<double> CalculateSuccessProbabilities(IList<INode> nodes, IList<CashFlow> cashFlows)
        {
            var numSimulations = nodes[0].Simulations.Count / (cashFlows.Count - 1);
            InitSimulations(nodes, numSimulations, cashFlows.Count);
            var successCounts = CalculateSuccessCounts(nodes, cashFlows, numSimulations);
            var successProbabilities = CalculateSuccessProbabilities(successCounts, numSimulations, cashFlows.Count);
            _cumulativeSimulations.Clear();
            _valueSimulations.Clear();
            return successProbabilities;
        }

        private void InitSimulations(IList<INode> nodes, int numSimulations, int numCashFlows)
        {
            foreach (var node in nodes)
            {
                var valueSimulations = new double[numSimulations, numCashFlows];
                var cumulativeSimulations = new double[numSimulations, numCashFlows - 1];
                Parallel.For(0, node.Simulations.Count, simulationCnt =>
                {
                    var portfolioCnt = simulationCnt % numSimulations;
                    var cashFlowCnt = simulationCnt / numSimulations;
                    valueSimulations[portfolioCnt, 0] = node.InitialInvestment.Value;

                    if (cashFlowCnt < (numCashFlows - 1))
                    {
                        var simulation = node.Simulations[simulationCnt];
                        var cumulativeSimulation = simulation / node.InitialPrice;
                        cumulativeSimulations[portfolioCnt, cashFlowCnt] = cumulativeSimulation.Value;
                    }
                });
                _cumulativeSimulations[node.Id] = cumulativeSimulations;
                _valueSimulations[node.Id] = valueSimulations;
            }
        }

        private IList<double> CalculateSuccessCounts(IList<INode> nodes, IList<CashFlow> cashFlows, int numSimulations)
        {
            var successCounts = new double[cashFlows.Count];
            var portfolioSimulations = new double[numSimulations][];
            for (int portfolioCnt = 0; portfolioCnt < numSimulations; portfolioCnt++)
            {
                portfolioSimulations[portfolioCnt] = new double[cashFlows.Count];
                for (int periodCnt = 0; periodCnt < cashFlows.Count; periodCnt++)
                {
                    var portfolioValue = nodes.Sum(n => _valueSimulations[n.Id][portfolioCnt, periodCnt]);
                    var portfolioValueNet = portfolioValue - cashFlows[periodCnt].Cost;
                    portfolioValueNet = portfolioValueNet > 0 ? portfolioValueNet : 0;
                    portfolioSimulations[portfolioCnt][periodCnt] = portfolioValueNet;
                    if (periodCnt < cashFlows.Count - 1)
                        foreach (var node in nodes)
                            _valueSimulations[node.Id][portfolioCnt, periodCnt + 1] = _cumulativeSimulations[node.Id][portfolioCnt, periodCnt] * portfolioValueNet * node.PortfolioWeight.Value;

                    successCounts[periodCnt] = portfolioValueNet > 0 ? successCounts[periodCnt] + 1 : successCounts[periodCnt];
                }
            }
            return successCounts;
        }

        private IList<double> CalculateSuccessProbabilities(IList<double> successCounts, int numSimulations, int numCashFlows)
        {
            IList<double> successProbabilities = new double[numCashFlows];
            for (int periodCnt = 0; periodCnt < numCashFlows; periodCnt++)
            {
                successProbabilities[periodCnt] = successCounts[periodCnt] / numSimulations;
            }
            return successProbabilities;
        }
    }
}
