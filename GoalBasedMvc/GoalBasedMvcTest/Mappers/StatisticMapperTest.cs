using GoalBasedMvc.Mappers;
using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GoalBasedMvcTest.Mappers
{
    [TestClass]
    public class StatisticMapperTest
    {
        [TestMethod]
        public void MapStatisticToViewModelOnSuccessReturnsProperties()
        {
            //arrange
            var mean = 1D;
            var stdev = 2D;
            var skew = 3D;
            var kurt = 4D;
            var statistic = new Mock<IStatistic>();
            statistic.Setup(s => s.Mean).Returns(mean);
            statistic.Setup(s => s.Stdev).Returns(stdev);
            statistic.Setup(s => s.Skew).Returns(skew);
            statistic.Setup(s => s.Kurt).Returns(kurt);

            var mapper = new StatisticMapper();

            //act
            var result = mapper.MapStatisticToViewModel(statistic.Object);

            //assert
            Assert.AreEqual(mean, result.Mean);
            Assert.AreEqual(stdev, result.Stdev);
            Assert.AreEqual(skew, result.Skew);
            Assert.AreEqual(kurt, result.Kurt);
        }
    }
}
