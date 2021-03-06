﻿using GoalBasedMvc.Logic;
using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace GoalBasedMvcTest.Logic
{
    [TestClass]
    public class NodeSimulatorTest
    {
        [TestMethod]
        public void SimulateNodesUpdatesNodeDictionary()
        {
            //arrange
            var uniformRandom = .01D;
            var price = 2D;
            var distributionIndex = 0;
            var uniformRandoms = new[] { uniformRandom };

            var cdfProbability = 1D;
            var cdfProbabilities = new[] { cdfProbability };
            var distribution = new Mock<IDistribution>();
            distribution.Setup(d => d.CdfProbabilities).Returns(cdfProbabilities);
            distribution.Setup(d => d.GetPrice(It.Is<double>(rand => rand == uniformRandom), It.Is<int>(ix => ix == distributionIndex))).Returns(price);
            var distributions = new[] { distribution.Object };

            var parentId = 1;
            var parent = new Mock<INode>();
            parent.Setup(p => p.Id).Returns(parentId);
            parent.Setup(p => p.Distributions).Returns(distributions);

            var childId = 2;
            var child = new Mock<INode>();
            child.Setup(c => c.Id).Returns(childId);
            child.Setup(c => c.Parent).Returns(parent.Object);
            child.Setup(c => c.Distributions).Returns(distributions);

            IDictionary<int, INode> nodeDictionary = new SortedDictionary<int, INode> { { parentId, parent.Object }, { childId, child.Object } };

            var repository = new Mock<IUniformRandomRepository>();
            repository.Setup(r => r.GetUniformRandoms()).Returns(uniformRandoms);

            var simulator = new NodeSimulator(repository.Object);

            //act
            var results = simulator.SimulateNodes(nodeDictionary);

            //assert
            Assert.AreEqual(results[parentId][0], price);
            Assert.AreEqual(results[childId][0], price);   
        }
    }
}
