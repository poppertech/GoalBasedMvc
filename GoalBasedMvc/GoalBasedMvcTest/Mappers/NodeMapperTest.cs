using GoalBasedMvc.Mappers;
using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvcTest.Mappers
{
    [TestClass]
    public class NodeMapperTest
    {
        [TestMethod]
        public void MapNodeRecordsToNodesOnSuccessMapsProperties()
        {
            //arrange
            var distributionId = 5;
            var distribution = new Mock<IDistribution>();
            distribution.Setup(d => d.Id).Returns(distributionId);
            IList<IDistribution> distributions = new List<IDistribution> { distribution.Object };
            var record1 = new NodeRecord
            {
                Id = 1,
                Name = "name",
                Url = "url",
                NetworkName = "networkname",
                NetworkUrl = "networkurl",
                InitialPrice = 2,
                InitialInvestment = 3,
                PortfolioWeight = 4,
                IsPortfolioComponent = true,
                Distributions = distributions
            };

            var record2 = new NodeRecord
            {
                Id = 2,
                Parent = record1
            };

            var records = new SortedDictionary<int, NodeRecord>
            {
                {record1.Id, record1 },
                {record2.Id, record2 }
            };

            Func<INode> factory = () =>
            {
                var node = new Mock<INode>();
                node.SetupAllProperties();
                return node.Object;
            };

            var mapper = new NodeMapper(factory);

            //act
            var result = mapper.MapNodeRecordsToNodes(records);

            //assert
            Assert.AreEqual(record1.Id, result[1].Id);
            Assert.AreEqual(record1.Name, result[1].Name);
            Assert.AreEqual(record1.Url, result[1].Url);
            Assert.AreEqual(record1.NetworkName, result[1].NetworkName);
            Assert.AreEqual(record1.NetworkUrl, result[1].NetworkUrl);
            Assert.AreEqual(record1.InitialPrice, result[1].InitialPrice);
            Assert.AreEqual(record1.InitialInvestment, result[1].InitialInvestment);
            Assert.AreEqual(record1.PortfolioWeight, result[1].PortfolioWeight);
            Assert.IsTrue(record1.IsPortfolioComponent);
            Assert.AreEqual(distributionId, result[1].Distributions[0].Id);
            Assert.AreEqual(record1.Id, result[2].Parent.Id);
            Assert.AreEqual(record1.Id, result.Values.First().Id);
            Assert.AreEqual(record2.Id, result.Values.Last().Id);
        }
    }
}
