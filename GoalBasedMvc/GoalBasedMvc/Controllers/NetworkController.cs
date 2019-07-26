using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GoalBasedMvc.Models;
using System.Collections.Generic;
using GoalBasedMvc.Logic;
using System.Linq;

namespace GoalBasedMvc.Controllers
{
    public class NetworkController : Controller
    {
        private readonly INetworkService _service;

        public NetworkController(INetworkService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            var networks = _service.GetNetworks();
            return View(networks);
        }

        [HttpGet("{url}")]
        public IActionResult Edit(string url)
        {
            var network = _service.GetNetworkByUrl(url);
            return View(network);
        }

        [HttpPost]
        public IActionResult Edit([FromBody]NetworkEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.ErrorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
                return Json(viewModel);
            }
            var network = _service.CalculateNetwork(viewModel);
            return Json(network);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("instructions")]
        public IActionResult Instructions()
        {
            return View();
        }


}
}
