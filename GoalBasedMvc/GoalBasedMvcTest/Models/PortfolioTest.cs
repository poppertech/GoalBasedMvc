using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GoalBasedMvcTest.Models
{
    [TestClass]
    public class PortfolioTest
    {
        [TestMethod]
        public void InitOnSuccessReturnsCorrectProbabilities()
        {
            //arrange
            var simulation1 = new Simulation { Price = 117.506039533933 };
            var simulation2 = new Simulation { Price = 127.005485296386 };
            var simulation3 = new Simulation { Price = 119.64969234571 };
            var simulation4 = new Simulation { Price = 113.754221009641 };
            var simulations = new[] { simulation1, simulation2, simulation3, simulation4 };
            var node = new Node(null, null)
            {
                InitialInvestment = 200000,
                InitialPrice = 100,
                IsPortfolioComponent = true,
                Simulations = simulations
            };
            IList<INode> nodes = new[] { node };

            var cashFlow0 = new CashFlow { Cost = 0 };
            var cashFlow1 = new CashFlow { Cost = 48000 };
            var cashFlow2 = new CashFlow { Cost = 49200 };
            var cashFlows = new[] { cashFlow0, cashFlow1, cashFlow2 };

            var portfolio = new Portfolio();

            //act
            portfolio.Init(ref nodes, cashFlows);

            //assert
            Assert.AreEqual(node.InitialInvestment, portfolio.InitialValue);
            Assert.AreEqual(node.InitialInvestment, node.ValueSimulations[0,0]);
            Assert.AreEqual(node.InitialInvestment, node.ValueSimulations[1,0]);
            Assert.AreEqual(235012.0791, node.ValueSimulations[0,1], .01);
            Assert.AreEqual(254010.9706, node.ValueSimulations[1,1], .01);
            Assert.AreEqual(223759.3773, node.ValueSimulations[0, 2], .01);
            Assert.AreEqual(234346.1748, node.ValueSimulations[1, 2], .01);

            Assert.AreEqual(1, portfolio.SuccessProbabilities[0]);
            Assert.AreEqual(1, portfolio.SuccessProbabilities[1]);
            Assert.AreEqual(1, portfolio.SuccessProbabilities[2]);
        }
    }
}
