﻿using GoalBasedMvc.Logic;
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
            var child = new Node { Id = 2 };
            var parent = new Node { Id = 1, Children = new[] { child } };

            IDictionary<int, Node> nodeDictionary = new Dictionary<int, Node> { { parent.Id, parent }, { child.Id, child } };
            IList<Node> nodes = new List<Node> { parent, child };

            var portfolio = new Mock<IPortfolio>();
            var nodeSimulator = new Mock<INodeSimulator>();

            var network = new Network(nodeSimulator.Object, portfolio.Object);

            //act
            network.Calculate(ref nodeDictionary, cashFlows);

            //assert
            Assert.AreEqual(portfolio.Object, network.Portfolio);
            Assert.AreEqual(parent.Id, network.Nodes.Values.First().Id);
            Assert.AreEqual(child.Id, network.Nodes.Values.Last().Id);
            Assert.AreEqual(cashFlow.Id, network.CashFlows.First().Id);
        }
    }
}
