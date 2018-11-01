﻿using System.Diagnostics;
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

        [HttpGet("{url}")]
        public IActionResult Edit(string url)
        {
            var network = _service.GetNetworkByUrl(url);
            return View(network);
        }

        [HttpPost]
        public IActionResult Edit([FromBody]NetworkEditViewModel viewModel)
        {
            var network = _service.CalculateNetwork(viewModel);
            return Json(network);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Instructions()
        {
            return View();
        }


}
}
