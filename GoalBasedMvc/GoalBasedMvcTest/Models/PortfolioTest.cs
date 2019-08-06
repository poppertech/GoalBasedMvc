using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
                Simulations = simulations,
                PortfolioWeight = 1
            };
            IList<INode> nodes = new[] { node };

            var cashFlow0 = new CashFlow { Cost = 0 };
            var cashFlow1 = new CashFlow { Cost = 48000 };
            var cashFlow2 = new CashFlow { Cost = 49200 };
            var cashFlows = new[] { cashFlow0, cashFlow1, cashFlow2 };

            var expectedSimulations = new double[4]
            {
                (simulation1.Price / node.InitialPrice.Value - 1)*100,
                (simulation2.Price / node.InitialPrice.Value - 1)*100,
                (simulation3.Price / node.InitialPrice.Value - 1)*100,
                (simulation4.Price / node.InitialPrice.Value - 1)*100
            };

            var statistics = new Mock<IStatistic>();

            var datum = new HistogramDatum { Interval = 1 };
            var data = new[] { datum };
            var histogram = new Mock<IHistogram>();
            histogram.Setup(h => h.GetHistogramData(
                It.Is<HistogramContext>(ctx => 
                ctx.GlobalXMin == expectedSimulations[3] &&
                ctx.GlobalXMax == expectedSimulations[1] &&
                ctx.Simulations[0] == expectedSimulations[0] &&
                ctx.Simulations[2] == expectedSimulations[2]),
                It.IsAny<int>()))
                .Returns(data);

            var portfolio = new Portfolio(statistics.Object, histogram.Object);

            //act
            portfolio.Init(ref nodes, cashFlows);

            //assert
            Assert.AreEqual(node.InitialInvestment, node.ValueSimulations[0, 0]);
            Assert.AreEqual(node.InitialInvestment, node.ValueSimulations[1, 0]);
            Assert.AreEqual(235012.0791, node.ValueSimulations[0, 1], .01);
            Assert.AreEqual(254010.9706, node.ValueSimulations[1, 1], .01);
            Assert.AreEqual(223759.3773, node.ValueSimulations[0, 2], .01);
            Assert.AreEqual(234346.1748, node.ValueSimulations[1, 2], .01);

            Assert.AreEqual(1, portfolio.SuccessProbabilities[0]);
            Assert.AreEqual(1, portfolio.SuccessProbabilities[1]);
            Assert.AreEqual(1, portfolio.SuccessProbabilities[2]);

            Assert.AreSame(portfolio.Statistics, statistics.Object);
            statistics.Verify(s => s.Init(It.Is<IList<double>>(
                sims => sims[0] == expectedSimulations[0] &&
                sims[1] == expectedSimulations[1] &&
                sims[2] == expectedSimulations[2] &&
                sims[3] == expectedSimulations[3])));

            Assert.AreEqual(datum.Interval, portfolio.Histogram[0].Interval);
        }
    }
}
