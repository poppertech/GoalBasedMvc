using GoalBasedMvc.Logic;
using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvcTest.Models
{
    [TestClass]
    public class NetworkTest
    {
        [TestMethod]
        public void CalculateOnSuccessRunsCalculations()
        {
            //arrange
            var cashFlow = new CashFlow() { Id = 3 };
            var cashFlows = new[] { cashFlow };

            var isNotPortfolioNodeId = 1;
            var isNotPortfolioNode = new Mock<INode>();
            isNotPortfolioNode.SetupAllProperties();
            isNotPortfolioNode.Setup(p => p.Id).Returns(isNotPortfolioNodeId);
            isNotPortfolioNode.Setup(p => p.IsPortfolioComponent).Returns(false);

            var IsPortfolioNodeId = 2;
            var isPortfolioNode = new Mock<INode>();
            isPortfolioNode.SetupAllProperties();
            isPortfolioNode.Setup(c => c.Id).Returns(IsPortfolioNodeId);
            isPortfolioNode.Setup(p => p.IsPortfolioComponent).Returns(true);

            var nodeDictionary = new Dictionary<int, INode> {
                { isNotPortfolioNodeId, isNotPortfolioNode.Object },
                { IsPortfolioNodeId, isPortfolioNode.Object }
            };

            var isNotPortfolioSimulation = 4D;
            var isPortfolioSimulation = 5D;
            var nodeSimulations = new Dictionary<int, IList<double>> {
                { isNotPortfolioNodeId, new List<double>{isNotPortfolioSimulation } },
                { IsPortfolioNodeId, new List<double>{ isPortfolioSimulation} }
            };

            var nodeSimulator = new Mock<INodeSimulator>();
            nodeSimulator.Setup(s => s.SimulateNodes(
                It.Is<IDictionary<int, INode>>(dict => dict == nodeDictionary )))
                .Returns(nodeSimulations);

            var portfolio = new Mock<IPortfolio>();
            portfolio.Setup(p => p.Init(
                It.Is<IList<INode>>(list => list[0].Id == IsPortfolioNodeId && list.Count == 1),
                It.Is<IList<CashFlow>>(cf => cf[0].Id == cashFlows[0].Id)));

            var network = new Network(portfolio.Object, nodeSimulator.Object);
            network.CashFlows = cashFlows;
            network.Nodes = nodeDictionary;

            //act
            network.Calculate();

            //assert
            Assert.AreEqual(portfolio.Object, network.Portfolio);
            portfolio.Verify(p => p.Init(
                It.Is<IList<INode>>(list => list[0].Id == IsPortfolioNodeId && list.Count == 1),
                It.Is<IList<CashFlow>>(cf => cf[0].Id == cashFlows[0].Id)));

            Assert.AreEqual(cashFlow.Id, network.CashFlows.First().Id);

            Assert.AreEqual(IsPortfolioNodeId, network.Nodes.Values.Last().Id);
            isPortfolioNode.VerifySet(n => n.Portfolio = portfolio.Object);
            Assert.AreEqual(isPortfolioSimulation, isPortfolioNode.Object.Simulations[0]);

            Assert.AreEqual(isNotPortfolioNodeId, network.Nodes.Values.First().Id);
            isNotPortfolioNode.VerifySet(n => n.Portfolio = It.IsAny<IPortfolio>(), Times.Never);
            Assert.AreEqual(isNotPortfolioSimulation, isNotPortfolioNode.Object.Simulations[0]);
        }
    }
}
