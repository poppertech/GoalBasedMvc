using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalBasedMvc.Models
{
    public interface IPortfolio
    {
        double InitialValue { get; }
        IStatistic Statistics { get; }
        IList<HistogramDatum> Histogram { get; }
        IList<double> SuccessProbabilities { get; }

        void Init(IList<INode> nodes, IList<CashFlow> cashFlows);
    }

    public class Portfolio : IPortfolio
    {
        private readonly IStatistic _statistics;
        private readonly IHistogram _histogram;
        private readonly IDictionary<int, double[,]> _cumulativeSimulations = new Dictionary<int, double[,]>();
        private readonly IDictionary<int, double[,]> _valueSimulations = new Dictionary<int, double[,]>();

        private IList<CashFlow> _cashFlows;
        private IList<double> _simulations;

        private int _numSimulations;

        public double InitialValue { get; private set; }

        public Portfolio(
            IStatistic statistics,
            IHistogram histogram
            )
        {
            _statistics = statistics;
            _histogram = histogram;
        }

        public void Init(IList<INode> nodes, IList<CashFlow> cashFlows)
        {
            _cashFlows = cashFlows;
            _numSimulations = nodes[0].Simulations.Count / (cashFlows.Count - 1);
            InitialValue = nodes.Sum(n => n.InitialInvestment.Value);
            SetSimulations(nodes);
            InitSimulations(nodes);
            var successCounts = CalculateSuccessCounts(nodes);
            SuccessProbabilities = CalculateSuccessProbabilities(successCounts);
            _cumulativeSimulations.Clear();
            _valueSimulations.Clear();
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
                return _simulations;
            }
        }

        private void SetSimulations(IList<INode> nodes)
        {
            const double SCALING_FACTOR = 100;
            _simulations = new double[nodes[0].Simulations.Count];
            foreach (var node in nodes)
                for (int cnt = 0; cnt < node.Simulations.Count; cnt++)
                    _simulations[cnt] += (node.Simulations[cnt] / node.InitialPrice.Value - 1) * SCALING_FACTOR * node.PortfolioWeight.Value;
        }


        private void InitSimulations(IList<INode> nodes)
        {
            foreach (var node in nodes)
            {
                var numCashFlows = _cashFlows.Count;
                var valueSimulations = new double[_numSimulations, numCashFlows];
                var cumulativeSimulations = new double[_numSimulations, numCashFlows - 1];
                Parallel.For(0, node.Simulations.Count, simulationCnt =>
                {
                    var portfolioCnt = simulationCnt % _numSimulations;
                    var cashFlowCnt = simulationCnt / _numSimulations;
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

        private IList<double> CalculateSuccessCounts(IList<INode> nodes)
        {
            var successCounts = new double[_cashFlows.Count];
            var portfolioSimulations = new double[_numSimulations][];
            for (int portfolioCnt = 0; portfolioCnt < _numSimulations; portfolioCnt++)
            {
                portfolioSimulations[portfolioCnt] = new double[_cashFlows.Count];
                for (int periodCnt = 0; periodCnt < _cashFlows.Count; periodCnt++)
                {
                    var portfolioValue = nodes.Sum(n => _valueSimulations[n.Id][portfolioCnt, periodCnt]);
                    var portfolioValueNet = portfolioValue - _cashFlows[periodCnt].Cost;
                    portfolioValueNet = portfolioValueNet > 0 ? portfolioValueNet : 0;
                    portfolioSimulations[portfolioCnt][periodCnt] = portfolioValueNet;
                    if (periodCnt < _cashFlows.Count - 1)
                        foreach (var node in nodes)
                            _valueSimulations[node.Id][portfolioCnt, periodCnt + 1] = _cumulativeSimulations[node.Id][portfolioCnt, periodCnt] * portfolioValueNet * node.PortfolioWeight.Value;

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
