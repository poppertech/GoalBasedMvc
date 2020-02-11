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
            var distributionRecord = new DistributionRecord { Id = distributionId };
            var distributionRecords = new List<DistributionRecord> { distributionRecord };
            var nodeRecord1 = new NodeRecord
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
                Distributions = distributionRecords
            };

            var nodeRecord2 = new NodeRecord
            {
                Id = 2,
                Parent = nodeRecord1
            };

            var records = new SortedDictionary<int, NodeRecord>
            {
                {nodeRecord1.Id, nodeRecord1 },
                {nodeRecord2.Id, nodeRecord2 }
            };

            Func<INode> nodeFactory = () =>
            {
                var node = new Mock<INode>();
                node.SetupAllProperties();
                return node.Object;
            };

            Func<DistributionRecord, IDistribution> distributionFactory = (r) =>
             {
                 var distribution = new Mock<IDistribution>();
                 distribution.Setup(d => d.Id).Returns(r.Id);
                 return distribution.Object;
             };

            var mapper = new NodeMapper(nodeFactory, distributionFactory, null, null);

            //act
            var result = mapper.MapNodeRecordsToNodes(records);

            //assert
            Assert.AreEqual(nodeRecord1.Id, result[1].Id);
            Assert.AreEqual(nodeRecord1.Name, result[1].Name);
            Assert.AreEqual(nodeRecord1.Url, result[1].Url);
            Assert.AreEqual(nodeRecord1.NetworkName, result[1].NetworkName);
            Assert.AreEqual(nodeRecord1.NetworkUrl, result[1].NetworkUrl);
            Assert.AreEqual(nodeRecord1.InitialPrice, result[1].InitialPrice);
            Assert.AreEqual(nodeRecord1.InitialInvestment, result[1].InitialInvestment);
            Assert.AreEqual(nodeRecord1.PortfolioWeight, result[1].PortfolioWeight);
            Assert.IsTrue(nodeRecord1.IsPortfolioComponent);
            Assert.AreEqual(distributionId, result[1].Distributions[0].Id);
            Assert.AreEqual(nodeRecord1.Id, result[2].Parent.Id);
            Assert.AreEqual(nodeRecord1.Id, result.Values.First().Id);
            Assert.AreEqual(nodeRecord2.Id, result.Values.Last().Id);
        }
    }
}
