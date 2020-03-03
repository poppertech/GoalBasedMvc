using GoalBasedMvc.Logic;
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
            int expectedPortfolioSimulation = 10; // int avoids floating point errors
            var simulation = 110D;
            var simulations = new[] { simulation };

            var nodeInitialInvestment = 1000D;
            var nodeInitialPrice = 100D;
            var node = new Mock<INode>();
            node.Setup(n => n.InitialInvestment).Returns(nodeInitialInvestment);
            node.Setup(n => n.InitialPrice).Returns(nodeInitialPrice);
            node.Setup(n => n.Simulations).Returns(simulations);
            node.Setup(n => n.PortfolioWeight).Returns(1);
            var nodes = new[] { node.Object };

            var cashFlowId = 1;
            var cashFlow = new CashFlow { Id = cashFlowId };
            var cashFlows = new[] { cashFlow };

            var successProbability = .5D;
            var successProbabilities = new[] { successProbability };
            var portfolioSimulator = new Mock<IPortfolioSimulator>();
            portfolioSimulator.Setup(ps => ps.CalculateSuccessProbabilities(
                It.Is<IList<INode>>(n => n[0].InitialInvestment == nodeInitialInvestment),
                It.Is<IList<CashFlow>>(cf => cf[0].Id == cashFlowId)))
                .Returns(successProbabilities);

            var statistics = new Mock<IStatistic>();

            var interval = 3D;
            var histogramDatum = new HistogramDatum {Interval = interval};
            var histogramData = new[] { histogramDatum };
            var histogram = new Mock<IHistogram>();
            histogram.Setup(h => h.GetHistogramData(
                    It.Is<HistogramContext>(
                        ctx => (int)ctx.Simulations[0] == expectedPortfolioSimulation &&
                        (int)ctx.GlobalXMin == expectedPortfolioSimulation &&
                        (int)ctx.GlobalXMax == expectedPortfolioSimulation),
                    It.IsAny<int>()))
                .Returns(histogramData);

            var portfolio = new Portfolio(statistics.Object, histogram.Object, portfolioSimulator.Object);

            //act
            portfolio.Init(nodes, cashFlows);
            var initialValueResult = portfolio.InitialValue;
            var statisticsResult = portfolio.Statistics;
            var histogramResult = portfolio.Histogram;
            var successProbabilitiesResult = portfolio.SuccessProbabilities;

            //assert
            Assert.AreEqual(nodeInitialInvestment, initialValueResult);
            Assert.AreSame(statistics.Object, statisticsResult);
            statistics.Verify(s => s.Init(
                It.Is<IList<double>>(sim => (int)sim[0] == expectedPortfolioSimulation)
                )); 
            Assert.AreEqual(interval, histogramResult[0].Interval);
            Assert.AreEqual(successProbability, successProbabilitiesResult[0]);
        }
    }
}
