namespace GoalBasedMvc.Models
{
    public class DistributionViewModel
    {
        public int Id { get; set; }

        public double Minimum { get; set; }
        public double Worst { get; set; }
        public double Likely { get; set; }
        public double Best { get; set; }
        public double Maximum { get; set; }

        public double HeightWorst { get; set; }
        public double HeightLikely { get; set; }
        public double HeightBest { get; set; }

        public double Mean { get; set; }
        public double Stdev { get; set; }
        public double Skew { get; set; }
        public double Kurt { get; set; }

    }
}
