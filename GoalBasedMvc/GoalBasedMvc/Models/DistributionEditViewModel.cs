using System.ComponentModel.DataAnnotations;

namespace GoalBasedMvc.Models
{
    [DistributionValidation]
    public class DistributionEditViewModel
    {
        public int Id { get; set; }

        public double Minimum { get; set; }
        public double Worst { get; set; }
        public double Likely { get; set; }
        public double Best { get; set; }
        public double Maximum { get; set; }
    }

    public class DistributionValidationAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is Distribution)
            {
                var distribution = (DistributionEditViewModel)value;
                var isValid = true;
                isValid = isValid && distribution.Minimum < distribution.Worst;
                isValid = isValid && distribution.Worst < distribution.Likely;
                isValid = isValid && distribution.Likely < distribution.Best;
                isValid = isValid && distribution.Best < distribution.Maximum;
                if (!isValid)
                    return false;
            }
            return base.IsValid(value);
        }
    }
}
