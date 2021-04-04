using GoalBasedMvc.Mappers;
using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace GoalBasedMvcTest.Mappers
{
    [TestClass]
    public class PortfolioMapperTest
    {
        [TestMethod]
        public void MapPortfolioToViewModelOnSuccessReturnsProperties()
        {
            //arrange
            var mean = 1D;
            var statistic = new Mock<IStatistic>();
            statistic.Setup(s => s.Mean).Returns(mean);

            var frequency = 2D;
            var histogramDatum = new HistogramDatum
            {
                Frequency = frequency
            };
            IList<HistogramDatum> histogram = new[] { histogramDatum };

            var successProbability = 3D;
            IList<double> successProbabilities = new[] { successProbability };

            var portfolio = new Mock<IPortfolio>();
            portfolio.Setup(p => p.Statistics).Returns(statistic.Object);
            portfolio.Setup(p => p.Histogram).Returns(histogram);
            portfolio.Setup(p => p.SuccessProbabilities).Returns(successProbabilities);

            var statisticsViewModel = new StatisticViewModel {
                Mean = mean
            };
            var statisticMapper = new Mock<IStatisticMapper>();
            statisticMapper.Setup(m => m.MapStatisticToViewModel(
                It.Is<IStatistic>(s => s.Mean == mean)))
                .Returns(statisticsViewModel);

            var mapper = new PortfolioMapper(statisticMapper.Object);

            //act
            var result = mapper.MapPortfolioToViewModel(portfolio.Object);

            //assert
            Assert.AreEqual(mean, result.Statistics.Mean);
            Assert.AreEqual(frequency, result.Histogram[0].Frequency);
            Assert.AreEqual(successProbability, result.SuccessProbabilities[0]);
        }
    }
}
