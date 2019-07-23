using GoalBasedMvc.Logic;
using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoalBasedMvcTest.Logic
{
    [TestClass]
    public class NodeSimulatorTest
    {
        [TestMethod]
        public void SimulateNodesUpdatesNodeDictionary()
        {
            //arrange
            var price = 7;
            var uniformRandom = .1D;
            var uniformRandoms = new[] { uniformRandom };
            var simulation = new Simulation { Price = price, DistributionIndex = 0 };
            var distribution = new Mock<IDistribution>();

            var parentId = 1;
            var parent = new Mock<INode>();
            parent.Setup(p => p.Id).Returns(parentId);
            parent.Setup(p => p.Distributions).Returns(new[] { distribution.Object });
            parent.Setup(p => p.Simulations).Returns(new[] { simulation });

            var childId = 2;
            var child = new Mock<INode>();
            child.Setup(c => c.Id).Returns(childId);
            child.Setup(c => c.Parent).Returns(parent.Object);
            child.Setup(c => c.Distributions).Returns(new[] { distribution.Object });

            IDictionary<int, INode> nodeDictionary = new SortedDictionary<int, INode> { { parentId, parent.Object }, { childId, child.Object } };

            var evaluator = new Mock<ISimulationEvaluator>();
            evaluator.Setup(e => e.Evaluate(It.Is<int>(ix => ix == 0), It.Is<double>(r => r == uniformRandom))).Returns(simulation);

            var repository = new Mock<IUniformRandomRepository>();
            repository.Setup(r => r.GetUniformRandoms()).Returns(uniformRandoms);

            var simulator = new NodeSimulator(evaluator.Object, repository.Object);

            //act
            simulator.SimulateNodes(nodeDictionary);

            //assert
            parent.VerifySet(p => p.Simulations = It.Is<IList<Simulation>>(s => s[0].Price == price));
            child.VerifySet(c => c.Simulations = It.Is<IList<Simulation>>(s => s[0].Price == price));
            evaluator.Verify(e => e.Init(It.IsAny<IList<IDistribution>>()));
            evaluator.Verify(e => e.Evaluate(It.Is<int>(ix => ix == 0), It.Is<double>(r => r == uniformRandom)));
        }
    }
}
