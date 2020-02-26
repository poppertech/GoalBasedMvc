using GoalBasedMvc.Logic;
using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace GoalBasedMvcTest.Logic
{
    [TestClass]
    public class PortfolioSimulatorTest
    {
        [TestMethod]
        public void CalculateSuccessProbabilitiesOnSuccessReturnsCorrectResults()
        {
            //arrange
            IList<double> simulations = new[] { 25D, 200D, 25D, 0 };
            var node = new Mock<INode>();
            node.Setup(n => n.InitialInvestment).Returns(100000);
            node.Setup(n => n.InitialPrice).Returns(100);
            node.Setup(n => n.IsPortfolioComponent).Returns(true);
            node.Setup(n => n.Simulations).Returns(simulations);
            node.Setup(n => n.PortfolioWeight).Returns(1);

            IList<INode> nodes = new[] { node.Object };

            var cashFlow0 = new CashFlow { Cost = 0 };
            var cashFlow1 = new CashFlow { Cost = 50000 };
            var cashFlow2 = new CashFlow { Cost = 50000 };
            var cashFlows = new[] { cashFlow0, cashFlow1, cashFlow2 };


            var portfolioSimulator = new PortfolioSimulator();
            
            //act
            var result = portfolioSimulator.CalculateSuccessProbabilities(nodes, cashFlows);

            //assert
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(.5, result[1]);
            Assert.AreEqual(0, result[2]);
        }
    }
}
