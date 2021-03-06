﻿using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public interface INode
    {
        int Id { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        string NetworkName { get; set; }
        string NetworkUrl { get; set; }
        double? InitialPrice { get; set; }
        double? InitialInvestment { get; set; }
        double? PortfolioWeight { get; }
        bool IsPortfolioComponent { get; set; }

        IList<IDistribution> Distributions { get; set; }
        IStatistic Statistics { get; }
        IList<HistogramDatum> Histogram { get; }

        IList<double> Simulations { get; set; }

        INode Parent { get; set; }
        IPortfolio Portfolio { set; }
    }

    public class Node : INode
    {
        private readonly IStatistic _statistic;
        private readonly IHistogram _histogram;

        public Node(
            IStatistic statistic,
            IHistogram histogram
            )
        {
            _statistic = statistic;
            _histogram = histogram;
            Distributions = new List<IDistribution>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string NetworkName { get; set; }
        public string NetworkUrl { get; set; }
        public double? InitialPrice { get; set; }
        public double? InitialInvestment { get; set; }
        public bool IsPortfolioComponent { get; set; }

        public double? PortfolioWeight
        {
            get
            {
                if (InitialInvestment.HasValue && Portfolio != null)
                {
                    return InitialInvestment / Portfolio.InitialValue;
                }
                return null;
            }
        }

        public IList<IDistribution> Distributions { get; set; }

        public IStatistic Statistics
        {
            get
            {
                _statistic.Init(Simulations);
                return _statistic;
            }
        }

        public IList<HistogramDatum> Histogram
        {
            get
            {
                var context = new HistogramContext();
                context.Simulations = Simulations;
                context.GlobalXMin = Distributions.Min(d => d.Minimum);
                context.GlobalXMax = Distributions.Max(d => d.Maximum);
                var data = _histogram.GetHistogramData(context);
                return data;
            }
        }

        public IList<double> Simulations { get; set; }

        public IPortfolio Portfolio { private get; set; }

        public INode Parent { get; set; }

    }
}
