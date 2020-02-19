using GoalBasedMvc.Logic;
using System.Collections.Generic;
using System.Linq;

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
        private readonly IPortfolioSimulator _portfolioSimulator;

        private IList<double> _simulations;

        public double InitialValue { get; private set; }

        public Portfolio(
            IStatistic statistics,
            IHistogram histogram,
            IPortfolioSimulator portfolioSimulator
            )
        {
            _statistics = statistics;
            _histogram = histogram;
            _portfolioSimulator = portfolioSimulator;
        }

        public void Init(IList<INode> nodes, IList<CashFlow> cashFlows)
        {
            InitialValue = nodes.Sum(n => n.InitialInvestment.Value);
            SetSimulations(nodes);
            SuccessProbabilities = _portfolioSimulator.CalculateSuccessProbabilities(nodes, cashFlows);
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

    }
}
