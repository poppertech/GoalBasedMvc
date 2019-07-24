using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoalBasedMvcTest.Models
{
    [TestClass]
    public class HistogramTest
    {
        [TestMethod]
        public void GetHistogramDataOnSuccessReturnsHistogramData()
        {
            //arrange
            var num = 4;
            var xMin = 20;
            var xMax = 26;
            var simulations = new double[] { 19, 21, 22, 24 };
            var histogramContext = new HistogramContext { GlobalXMin = xMin, GlobalXMax = xMax, Simulations = simulations };

            var histogram = new Histogram();

            //act
            var result = histogram.GetHistogramData(histogramContext, num);

            //assert
            Assert.AreEqual(20, result[0].Interval);
            Assert.AreEqual(21.5, result[1].Interval);
            Assert.AreEqual(23, result[2].Interval);
            Assert.AreEqual(24.5, result[3].Interval);

            Assert.AreEqual(.25, result[0].Frequency);
            Assert.AreEqual(.25, result[1].Frequency);
            Assert.AreEqual(.25, result[2].Frequency);
            Assert.AreEqual(.25, result[3].Frequency);
        }
    }
}
