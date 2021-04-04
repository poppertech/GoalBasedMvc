using GoalBasedMvc.Mappers;
using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace GoalBasedMvcTest.Mappers
{
    [TestClass]
    public class DistributionMapperTest
    {
        [TestMethod]
        public void MapDistributionToViewModelOnSuccessTransfersProperties()
        {
            //arrange
            var distribution = new Mock<IDistribution>();
            distribution.Setup(d => d.Id).Returns(1);

            distribution.Setup(d => d.Minimum).Returns(2);
            distribution.Setup(d => d.Worst).Returns(3);
            distribution.Setup(d => d.Likely).Returns(4);
            distribution.Setup(d => d.Best).Returns(5);
            distribution.Setup(d => d.Maximum).Returns(6);

            distribution.Setup(d => d.HeightWorst).Returns(7);
            distribution.Setup(d => d.HeightLikely).Returns(8);
            distribution.Setup(d => d.HeightBest).Returns(9);

            distribution.Setup(d => d.Mean).Returns(10);
            distribution.Setup(d => d.Stdev).Returns(11);
            distribution.Setup(d => d.Skew).Returns(12);
            distribution.Setup(d => d.Kurt).Returns(13);

            IList<IDistribution> distributions = new[] { distribution.Object };

            var mapper = new DistributionMapper();

            //act
            var viewModels = mapper.MapDistributionsToViewModels(distributions);

            //assert
            Assert.AreEqual(distributions[0].Id, viewModels[0].Id);

            Assert.AreEqual(distributions[0].Minimum, viewModels[0].Minimum);
            Assert.AreEqual(distributions[0].Worst, viewModels[0].Worst);
            Assert.AreEqual(distributions[0].Likely, viewModels[0].Likely);
            Assert.AreEqual(distributions[0].Best, viewModels[0].Best);
            Assert.AreEqual(distributions[0].Maximum, viewModels[0].Maximum);

            Assert.AreEqual(distributions[0].HeightWorst, viewModels[0].HeightWorst);
            Assert.AreEqual(distributions[0].HeightLikely, viewModels[0].HeightLikely);
            Assert.AreEqual(distributions[0].HeightBest, viewModels[0].HeightBest);

            Assert.AreEqual(distributions[0].Mean, viewModels[0].Mean);
            Assert.AreEqual(distributions[0].Stdev, viewModels[0].Stdev);
            Assert.AreEqual(distributions[0].Skew, viewModels[0].Skew);
            Assert.AreEqual(distributions[0].Kurt, viewModels[0].Kurt);
        }
    }
}
