using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace GoalBasedMvcTest.Models
{
    [TestClass]
    public class NodeTest
    {
        [TestMethod]
        public void StatisticsOnSuccessReturnsStatistics()
        {
            //arrange
            double price = 1;
            var simulations = new[] { price };

            var statistic = new Mock<IStatistic>();
            var node = new Node(statistic.Object, null);
            node.Simulations = simulations;

            //act
            var result = node.Statistics;

            //assert
            Assert.AreSame(result, statistic.Object);
            statistic.Verify(s => s.Init(It.Is<IList<double>>(p => p[0] == price)));
        }

        [TestMethod]
        public void HistogramOnSuccessReturnsHistogramData()
        {
            //arrange
            var interval = 1;
            var datum = new HistogramDatum { Interval = interval };
            var data = new[] { datum };
            double price = 2;
            var simulations = new[] { price };

            var minimum = 3;
            var maximum = 4;
            var distribution = new Mock<IDistribution>();
            distribution.Setup(d => d.Minimum).Returns(minimum);
            distribution.Setup(d => d.Maximum).Returns(maximum);
            var distributions = new[] { distribution.Object };

            var histogram = new Mock<IHistogram>();
            histogram.Setup(h => h.GetHistogramData(It.Is<HistogramContext>(c => c.GlobalXMin == minimum && c.GlobalXMax == maximum && c.Simulations[0] == price), It.IsAny<int>())).Returns(data);
            var node = new Node(null, histogram.Object);
            node.Simulations = simulations;
            node.Distributions = distributions;

            //act
            var result = node.Histogram;

            //assert
            Assert.AreEqual(interval, result[0].Interval);
        }
    }
}
