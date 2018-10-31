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
            var distribution = new Distribution(1, 2, 3, 4, 5, 6);
            var parent = new Node { Id = 1, Distributions = new[] { distribution }, Simulations = new[] { simulation } };
            var child = new Node { Id = 2, Parent = parent, Distributions = new[] { distribution } };

            IDictionary<int, Node> nodeDictionary = new SortedDictionary<int, Node> { { parent.Id, parent }, { child.Id, child } };

            var evaluator = new Mock<ISimulationEvaluator>();
            evaluator.Setup(e => e.Evaluate(It.Is<int>(ix => ix == 0), It.Is<double>(r => r == uniformRandom))).Returns(simulation);

            var repository = new Mock<IUniformRandomRepository>();
            repository.Setup(r => r.GetUniformRandoms()).Returns(uniformRandoms);

            var simulator = new NodeSimulator(evaluator.Object, repository.Object);

            //act
            simulator.SimulateNodes(nodeDictionary);

            //assert
            Assert.AreEqual(price, parent.Simulations[0].Price);
            Assert.AreEqual(price, child.Simulations[0].Price);
            evaluator.Verify(e => e.Init(It.IsAny<IList<Distribution>>()));
            evaluator.Verify(e => e.Evaluate(It.Is<int>(ix => ix == 0), It.Is<double>(r => r == uniformRandom)));
        }
    }
}
