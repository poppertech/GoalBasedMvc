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
            var xMax = 24;
            var simulations = new double[] { 20.5, 21.5, 22.5, 24 };
            var histogramContext = new HistogramContext { GlobalXMin = xMin, GlobalXMax = xMax, Simulations = simulations };

            var histogram = new Histogram();

            //act
            var result = histogram.GetHistogramData(histogramContext, num);

            //assert
            Assert.AreEqual(21, result[0].Interval);
            Assert.AreEqual(22, result[1].Interval);
            Assert.AreEqual(23, result[2].Interval);
            Assert.AreEqual(24, result[3].Interval);

            Assert.AreEqual(.25, result[0].Frequency);
            Assert.AreEqual(.25, result[1].Frequency);
            Assert.AreEqual(.25, result[2].Frequency);
            Assert.AreEqual(.25, result[3].Frequency);
        }
    }
}
