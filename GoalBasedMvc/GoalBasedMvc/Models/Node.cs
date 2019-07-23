using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public interface INode
    {
        int Id { get; set; }
        string Name { get; set; }
        double? InitialPrice { get; set; }
        double? InitialInvestment { get; set; }
        double? PortfolioWeight { get; set; }
        bool IsPortfolioComponent { get; set; }

        IList<IDistribution> Distributions { get; set; }
        IStatistic Statistics { get; }

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

        public Node(IStatistic statistic)
        {
            _statistic = statistic;
            Distributions = new List<IDistribution>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
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

        [JsonIgnore]
        public IList<Simulation> Simulations { get; set; }
        [JsonIgnore]
        public double[,] CumulativeSimulations { get; set; }
        [JsonIgnore]
        public double[,] ValueSimulations { get; set; }

        public INode Parent { get; set; }

    }
}
