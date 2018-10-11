using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GoalBasedMvc.Models;

namespace GoalBasedMvc.Controllers
{
    public class NetworkController : Controller
    {
        public IActionResult Index()
        {
            var network1 = new NetworkViewModel { Id = 1, Name = "Network1" };
            var network2 = new NetworkViewModel { Id = 2, Name = "Network2" };
            var networks = new[] { network1, network2 };
            return View(networks);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult Edit([FromForm]PostNetworkViewModel network)
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
