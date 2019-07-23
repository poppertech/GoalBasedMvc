using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GoalBasedMvcTest.Models
{
    [TestClass]
    public class StatisticTest
    {
        [TestMethod]
        public void InitStatisticsOnSuccessReturnsCorrectStatistics()
        {
            //arrange
            var expectedMean = .5;
            var expectedStdev = 0.534522484;
            double expectedSkew = 0;
            var expectedKurt = -2.8;

            var inputReturns = new double[] { 0, 1, 0, 1, 0, 1, 0, 1 };

            var statistic = new Statistic();

            //act
            statistic.Init(inputReturns);

            //assert
            Assert.IsTrue(Math.Abs(expectedMean - statistic.Mean) < .0001);
            Assert.IsTrue(Math.Abs(expectedStdev - statistic.Stdev) < .0001);
            Assert.IsTrue(Math.Abs(expectedSkew - statistic.Skew) < .0001);
            Assert.IsTrue(Math.Abs(expectedKurt - statistic.Kurt) < .0001);
        }

    }
}
