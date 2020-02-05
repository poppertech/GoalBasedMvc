using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GoalBasedMvc.Models
{
    public class NetworkEditViewModel
    {
        [Required]
        [NodesValidation]
        public SortedDictionary<int, NodeEditViewModel> Nodes { get; set; }

        [Required]
        [CashFlowsValidation]
        public IList<CashFlow> CashFlows { get; set; }

        public IList<string> ErrorMessages { get; set; }
    }

    public class NodesValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is SortedDictionary<int, NodeEditViewModel>)
            {
                var dictionary = (SortedDictionary<int, NodeEditViewModel>)value;
                if (dictionary.Count < 1 || dictionary.Keys.Any(k => k < 1))
                    return false;
            }
            return true;
        }
    }

    public class CashFlowsValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is IList<CashFlow>)
            {
                var list = (IList<CashFlow>)value;
                if (list.Count < 1)
                    return false;
            }
            return true;
        }
    }
}
