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
        public void CalculateTreeOnSuccessRunsCalculations()
        {
            //arrange
            var cashFlow = new CashFlow() { Id = 3 };
            var cashFlows = new[] { cashFlow };

            var parentId = 1;
            var parent = new Mock<INode>();
            parent.Setup(p => p.Id).Returns(parentId);

            var childId = 2;
            var child = new Mock<INode>();
            child.Setup(c => c.Id).Returns(childId);
            child.Setup(c => c.Parent).Returns(parent.Object);

            IDictionary<int, INode> nodeDictionary = new Dictionary<int, INode> { { parentId, parent.Object }, { childId, child.Object } };
            IList<INode> nodes = new List<INode> { parent.Object, child.Object };

            var portfolio = new Mock<IPortfolio>();
            var nodeSimulator = new Mock<INodeSimulator>();
            nodeSimulator.Setup(s => s.SimulateNodes(It.Is<IDictionary<int, INode>>(n => n == nodeDictionary))).Returns(nodeDictionary);

            var network = new Network(nodeSimulator.Object, portfolio.Object);
            network.CashFlows = cashFlows;
            network.Nodes = nodeDictionary;

            //act
            network.Calculate();

            //assert
            Assert.AreEqual(portfolio.Object, network.Portfolio);
            Assert.AreEqual(parentId, network.Nodes.Values.First().Id);
            Assert.AreEqual(childId, network.Nodes.Values.Last().Id);
            Assert.AreEqual(cashFlow.Id, network.CashFlows.First().Id);
            portfolio.Verify(p => p.Init(ref It.Ref<IList<INode>>.IsAny, It.IsAny<IList<CashFlow>>()));
        }
    }
}
