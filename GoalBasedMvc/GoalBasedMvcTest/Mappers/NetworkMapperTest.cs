using GoalBasedMvc.Mappers;
using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace GoalBasedMvcTest.Mappers
{
    [TestClass]
    public class NetworkMapperTest
    {
        [TestMethod]
        public void MapViewModelToEntityOnSuccessMapsProperties()
        {
            //arrange
            var distributionViewModel = new DistributionViewModel
            {
                Id = 1,
                Minimum = 2,
                Worst = 3,
                Likely = 4,
                Best = 5,
                Maximum = 6
            };
            var parentViewModel = new NodeViewModel
            {
                Id = 7,
                Name = "parent",
                InitialPrice = 9,
                InitialInvestment = 10,
                PortfolioWeight = 11,
                IsPortfolioComponent = true,
                Distributions = new[] { distributionViewModel },
                Parent = null
            };
            var childViewModel = new NodeViewModel
            {
                Id = 12,
                Distributions = new[] { distributionViewModel },
                Parent = parentViewModel
            };
            var cashFlowViewModel = new CashFlow();
            var networkViewModel = new NetworkViewModel
            {
                CashFlows = new[] { cashFlowViewModel },
                Nodes = new SortedDictionary<int, NodeViewModel> { { parentViewModel.Id, parentViewModel }, { childViewModel.Id, childViewModel } }
            };

            var network = new Network(null, null);

            var mapper = new NetworkMapper(network, () => new Node(null, null), ctx => new Distribution(ctx));

            //act
            var result = mapper.MapViewModelToEntity(networkViewModel);

            //assert
            Assert.AreSame(cashFlowViewModel, result.CashFlows[0]);

            Assert.AreEqual(distributionViewModel.Id, result.Nodes[parentViewModel.Id].Distributions[0].Id);
            Assert.AreEqual(distributionViewModel.Minimum, result.Nodes[parentViewModel.Id].Distributions[0].Minimum);
            Assert.AreEqual(distributionViewModel.Worst, result.Nodes[parentViewModel.Id].Distributions[0].Worst);
            Assert.AreEqual(distributionViewModel.Likely, result.Nodes[parentViewModel.Id].Distributions[0].Likely);
            Assert.AreEqual(distributionViewModel.Best, result.Nodes[parentViewModel.Id].Distributions[0].Best);
            Assert.AreEqual(distributionViewModel.Maximum, result.Nodes[parentViewModel.Id].Distributions[0].Maximum);

            Assert.AreEqual(parentViewModel.Id, result.Nodes[parentViewModel.Id].Id);
            Assert.AreEqual(parentViewModel.Name, result.Nodes[parentViewModel.Id].Name);
            Assert.AreEqual(parentViewModel.InitialPrice, result.Nodes[parentViewModel.Id].InitialPrice);
            Assert.AreEqual(parentViewModel.InitialInvestment, result.Nodes[parentViewModel.Id].InitialInvestment);
            Assert.AreEqual(parentViewModel.PortfolioWeight, result.Nodes[parentViewModel.Id].PortfolioWeight);
            Assert.IsTrue(result.Nodes[parentViewModel.Id].IsPortfolioComponent);
            Assert.IsNull(result.Nodes[parentViewModel.Id].Parent);
        }
    }
}
