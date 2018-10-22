using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GoalBasedMvc.Models;
using System.Collections.Generic;
using GoalBasedMvc.Logic;

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

        public IActionResult Edit(int id)
        {
            var network = _service.GetNetworkById(id);
            return View(network);
        }

        [HttpPost]
        public IActionResult Edit([FromBody]INetwork network)
        {
            return Json(network);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


}
}
