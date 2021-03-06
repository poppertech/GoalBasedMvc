﻿using System.Collections.Generic;

namespace GoalBasedMvc.Models
{
    public class NodeRecord
    {
        public NodeRecord()
        {
            Distributions = new List<DistributionRecord>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string NetworkName { get; set; }
        public string NetworkUrl { get; set; }
        public double? InitialPrice { get; set; }
        public double? InitialInvestment { get; set; }
        public bool IsPortfolioComponent { get; set; }
        public IList<DistributionRecord> Distributions { get; set; }
        public NodeRecord Parent { get; set; }
    }
}
