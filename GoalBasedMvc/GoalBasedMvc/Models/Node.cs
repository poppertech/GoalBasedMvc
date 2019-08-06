using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public interface INode
    {
        int Id { get; set; }
        string Name { get; set; }
        string NetworkName { get; set; }
        string NetworkUrl { get; set; }
        double? InitialPrice { get; set; }
        double? InitialInvestment { get; set; }
        double? PortfolioWeight { get; set; }
        bool IsPortfolioComponent { get; set; }

        IList<IDistribution> Distributions { get; set; }
        IStatistic Statistics { get; }
        IList<HistogramDatum> Histogram { get; }

        [JsonIgnore]
        IList<Simulation> Simulations { get; set; }
        [JsonIgnore]
        double[,] CumulativeSimulations { get; set; }
        [JsonIgnore]
        double[,] ValueSimulations { get; set; }

        INode Parent { get; set; }
    }

    public class Node:INode
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
        public string NetworkName { get; set; }
        public string NetworkUrl { get; set; }
        public double? InitialPrice { get; set; }
        public double? InitialInvestment { get; set; }
        public double? PortfolioWeight { get; set; }
        public bool IsPortfolioComponent { get; set; }

        public IList<IDistribution> Distributions { get; set; }

        public IStatistic Statistics {
            get {
                var prices = Simulations.Select(s => s.Price).ToList();
                _statistic.Init(prices);
                return _statistic;
            }
        }

        public IList<HistogramDatum> Histogram
        {
            get
            {
                var context = new HistogramContext();
                context.Simulations = Simulations.Select(s => s.Price).ToList();
                context.GlobalXMin = Distributions.Min(d => d.Minimum);
                context.GlobalXMax = Distributions.Max(d => d.Maximum);
                var data = _histogram.GetHistogramData(context);
                return data;
            }
        }

        [JsonIgnore]
        public IList<Simulation> Simulations { get; set; }
        [JsonIgnore]
        public double[,] CumulativeSimulations { get; set; }
        [JsonIgnore]
        public double[,] ValueSimulations { get; set; }

        public INode Parent { get; set; }

    }
}
