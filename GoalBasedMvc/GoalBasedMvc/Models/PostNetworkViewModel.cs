using Microsoft.AspNetCore.Http;

namespace GoalBasedMvc.Models
{
    public class PostNetworkViewModel
    {
        public IFormFile CashFlows { get; set; }
        public IFormFile Tree { get; set; }
    }
}
