namespace GoalBasedMvc.Models
{
    public class DistributionViewModel
    {
        int Id { get; set; }

        double Minimum { get; set; }
        double Worst { get; set; }
        double Likely { get; set; }
        double Best { get; set; }
        double Maximum { get; set; }

        double HeightWorst { get; set; }
        double HeightLikely { get; set; }
        double HeightBest { get; set; }

        double Mean { get; set; }
        double Stdev { get; set; }
        double Skew { get; set; }
        double Kurt { get; set; }
    }
}
