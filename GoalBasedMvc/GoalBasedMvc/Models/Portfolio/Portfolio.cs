using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalBasedMvc.Models
{
    public interface IPortfolio
    {
        IStatistic Statistics { get; }
        IList<HistogramDatum> Histogram { get; }
        IList<double> SuccessProbabilities { get; }

        void Init(ref IList<INode> nodes, IList<CashFlow> cashFlows);
    }

    public class Portfolio : IPortfolio
    {
        private readonly IStatistic _statistics;
        private readonly IHistogram _histogram;

        private IList<INode> _nodes;
        private IList<CashFlow> _cashFlows;
        private IList<double> _simulations;

        private int _numSimulations;
        private double _initialValue;

        public Portfolio(
            IStatistic statistics,
            IHistogram histogram
            )
        {
            _statistics = statistics;
            _histogram = histogram;
        }

        public void Init(ref IList<INode> nodes, IList<CashFlow> cashFlows)
        {
            _cashFlows = cashFlows;
            _nodes = nodes.Where(n => n.IsPortfolioComponent).ToList();
            _numSimulations = _nodes[0].Simulations.Count / (cashFlows.Count - 1);

            _initialValue = _nodes.Sum(n => n.InitialInvestment.Value);

            InitNodeProperties();
            var successCounts = CalculateSuccessCounts();
            SuccessProbabilities = CalculateSuccessProbabilities(successCounts);
            RemoveTransientNodeProperties();
        }

        private void RemoveTransientNodeProperties()
        {
            foreach (var node in _nodes)
            {
                node.CumulativeSimulations = null;
                node.ValueSimulations = null;
            }
        }

        public IList<double> SuccessProbabilities { get; private set; }

        public IStatistic Statistics
        {
            get
            {
                _statistics.Init(Simulations);
                return _statistics;
            }
        }

        public IList<HistogramDatum> Histogram
        {
            get
            {
                var context = new HistogramContext();
                context.Simulations = Simulations;
                context.GlobalXMin = Simulations.Min();
                context.GlobalXMax = Simulations.Max();
                var data = _histogram.GetHistogramData(context);
                return data;
            }
        }

        private IList<double> Simulations
        {
            get
            {
                if(_simulations == null)
                {
                    const double SCALING_FACTOR = 100;
                    _simulations = new double[_nodes[0].Simulations.Count];
                    foreach (var node in _nodes)
                        for (int cnt = 0; cnt < node.Simulations.Count; cnt++)
                            _simulations[cnt] += (node.Simulations[cnt] / node.InitialPrice.Value-1)* SCALING_FACTOR * node.PortfolioWeight.Value;
                }
                return _simulations;
            }
        }

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
            node.PortfolioWeight = node.InitialInvestment / _initialValue;
            node.ValueSimulations = new double[_numSimulations, numCashFlows];
            var cumulativeSimulations = new double[_numSimulations, numCashFlows - 1];
            Parallel.For(0, node.Simulations.Count, simulationCnt =>
            {
                var portfolioCnt = simulationCnt % _numSimulations;
                var cashFlowCnt = simulationCnt / _numSimulations;
                node.ValueSimulations[portfolioCnt, 0] = node.InitialInvestment.Value;

                if (cashFlowCnt < (numCashFlows - 1))
                {
                    var simulation = node.Simulations[simulationCnt];
                    var cumulativeSimulation = simulation / node.InitialPrice;
                    cumulativeSimulations[portfolioCnt, cashFlowCnt] = cumulativeSimulation.Value;
                }
            });
            node.CumulativeSimulations = cumulativeSimulations;
        }

        private IList<double> CalculateSuccessCounts()
        {
            var successCounts = new double[_cashFlows.Count];
            var portfolioSimulations = new double[_numSimulations][];
            for (int portfolioCnt = 0; portfolioCnt < _numSimulations; portfolioCnt++)
            {
                portfolioSimulations[portfolioCnt] = new double[_cashFlows.Count];
                for (int periodCnt = 0; periodCnt < _cashFlows.Count; periodCnt++)
                {
                    var portfolioValue = _nodes.Sum(n => n.ValueSimulations[portfolioCnt, periodCnt]);
                    var portfolioValueNet = portfolioValue - _cashFlows[periodCnt].Cost;
                    portfolioValueNet = portfolioValueNet > 0 ? portfolioValueNet : 0;
                    portfolioSimulations[portfolioCnt][periodCnt] = portfolioValueNet;
                    if (periodCnt < _cashFlows.Count - 1)
                        foreach (var node in _nodes)
                            node.ValueSimulations[portfolioCnt, periodCnt + 1] = node.CumulativeSimulations[portfolioCnt, periodCnt] * portfolioValueNet * node.PortfolioWeight.Value;

                    successCounts[periodCnt] = portfolioValueNet > 0 ? successCounts[periodCnt] + 1 : successCounts[periodCnt];
                }
            }
            return successCounts;
        }

        private IList<double> CalculateSuccessProbabilities(IList<double> successCounts)
        {
            IList<double> successProbabilities = new double[_cashFlows.Count];
            for (int periodCnt = 0; periodCnt < _cashFlows.Count; periodCnt++)
            {
                successProbabilities[periodCnt] = successCounts[periodCnt] / _numSimulations;
            }
            return successProbabilities;
        }

    }
}
