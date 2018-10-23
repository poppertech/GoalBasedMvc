using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoalBasedMvcTest.Models
{
    [TestClass]
    public class DistributionTest
    {
        [TestMethod]
        public void DistributionOnSuccessReturnsCorrectCalculations()
        {
            //arrange
            int id = 1;
            double xMin = 50;
            double xBad = 85;
            double xLikely = 105;
            double xGood = 125;
            double xMax = 150;

            var uniformRandom1 = 3.65182845504721;
            var uniformRandom2 = 12.1312092438079;
            var uniformRandom3 = 66.673560619284;
            var uniformRandom4 = 94.5083821398705;

            //act
            var distribution = new Distribution(id, xMin, xBad, xLikely, xGood, xMax);
            var price1 = distribution.GetPrice(uniformRandom1, 0);
            var price2 = distribution.GetPrice(uniformRandom2, 1);
            var price3 = distribution.GetPrice(uniformRandom3, 2);
            var price4 = distribution.GetPrice(uniformRandom4, 3);

            //assert
            Assert.AreEqual(.57, distribution.HeightWorst, .01);
            Assert.AreEqual(.8, distribution.HeightBest, .01);
            Assert.AreEqual(3.31, distribution.HeightLikely, .01);

            Assert.AreEqual(104.97, distribution.Mean, .01);
            Assert.AreEqual(16.2, distribution.Stdev, .01);
            Assert.AreEqual(-.277, distribution.Skew, .01);
            Assert.AreEqual(.537, distribution.Kurt, .01);

            Assert.AreEqual(10, distribution.CdfProbabilities[0], .01);
            Assert.AreEqual(48.86, distribution.CdfProbabilities[1], .01);
            Assert.AreEqual(90, distribution.CdfProbabilities[2], .01);
            Assert.AreEqual(100, distribution.CdfProbabilities[3], .01);

            Assert.AreEqual(71.15062613, price1, .01);
            Assert.AreEqual(87.7933079, price2, .01);
            Assert.AreEqual(111.0757505, price3, .01);
            Assert.AreEqual(131.4736373, price4, .01);
        }
    }
}
