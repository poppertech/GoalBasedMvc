using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GoalBasedMvc.Models
{
    public class NodeEditViewModel
    {
        private const double MAX_INITIAL_PRICE = 1000000;
        private const double MAX_INITIAL_INVESTMENT = 100000000;
        private const double MAX_PORTFOLIO_WEIGHT = 100;

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(0, MAX_INITIAL_PRICE)]
        public double? InitialPrice { get; set; }

        [Range(0, MAX_INITIAL_INVESTMENT)]
        public double? InitialInvestment { get; set; }

        [Range(0, MAX_PORTFOLIO_WEIGHT)]
        public double? PortfolioWeight { get; set; }

        public bool IsPortfolioComponent { get; set; }

        [Required]
        [DistributionsValidation]
        public IList<DistributionEditViewModel> Distributions { get; set; }

        public NodeEditViewModel Parent { get; set; }
    }

    public class DistributionsValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is IList<DistributionEditViewModel>)
            {
                var list = (IList<DistributionEditViewModel>)value;
                if (list.Count < 1)
                    return false;
            }
            return true;
        }
    }
}
