using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GoalBasedMvc.Models;
using System.Collections.Generic;

namespace GoalBasedMvc.Controllers
{
    public class NetworkController : Controller
    {
        public IActionResult Index()
        {
            var network1 = new NetworkSearchViewModel { Id = 1, Name = "Network1" };
            var network2 = new NetworkSearchViewModel { Id = 2, Name = "Network2" };
            var networks = new[] { network1, network2 };
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
        [Consumes("multipart/form-data")]
        public IActionResult Edit([FromForm]PostNetworkViewModel network)
        {
            var viewModel = new NetworkEditViewModel()
            {
                Id = 1,
                CashFlows = GetCashFlows(),
                Nodes = GetNodes(),
                Portfolio = GetPortfolio()
            };
            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IList<CashFlowViewModel> GetCashFlows()
        {
            return new CashFlowViewModel[] {
                new CashFlowViewModel { Cost = 60000 },
                new CashFlowViewModel { Cost = 61000 },
                new CashFlowViewModel { Cost = 62000 },
                new CashFlowViewModel { Cost = 63000 },
                new CashFlowViewModel { Cost = 64000 },
                new CashFlowViewModel { Cost = 65000 },
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
