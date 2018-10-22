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
            var network = new NetworkEditViewModel()
            {
                Id = id,
                CashFlows = GetCashFlows(),
                Nodes = GetNodes(),
                Portfolio = GetPortfolio()
            };
            return View(network);
        }

        [HttpPost]
        public IActionResult Edit([FromBody]NetworkEditViewModel network)
        {
            return Json(network);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IList<double> GetCashFlows()
        {
            return new double[] {
                60000, 61000, 62000, 63000, 64000, 65000
            };
        }

        private IList<NodeViewModel> GetNodes()
        {
            var distribution = new DistributionViewModel
            {
                Minimum = 50,
                Worst = 85,
                Likely = 105,
                Best = 125,
                Maximum = 150,
                HeightWorst = .57,
                HeightLikely = 3.31,
                HeightBest = .8,
                Mean = 1,
                Stdev = 2,
                Skew = 3,
                Kurt = 4
            };
            var parentNode = new NodeViewModel
            {
                Id = 1,
                Name = "Parent",
                IsPortfolioComponent = false,
                Distributions = new[] { distribution }
            };
            var childNode = new NodeViewModel
            {
                Id = 2,
                Name = "Child",
                InitialInvestment = 200000,
                InitialPrice = 100,
                Parent = parentNode,
                IsPortfolioComponent = true,
                Distributions = new[] { distribution, distribution, distribution, distribution }
            };
            return new NodeViewModel[] { parentNode, childNode};
    }

    private PortfolioViewModel GetPortfolio()
    {
        return new PortfolioViewModel
        {
            SuccessProbabilities = new[] { 1, .9, .7, .3, .1, 0 }
        };
    }

}
}
