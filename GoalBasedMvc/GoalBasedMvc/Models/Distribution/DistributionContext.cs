namespace GoalBasedMvc.Models
{
    public class DistributionContext
    {
        public int Id { get; set; }
        public double Minimum { get; set; }
        public double Worst { get; set; }
        public double Likely { get; set; }
        public double Best { get; set; }
        public double Maximum { get; set; }
    }
}
