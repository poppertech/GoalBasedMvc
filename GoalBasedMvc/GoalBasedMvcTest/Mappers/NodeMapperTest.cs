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
        public void MapNodesToViewModelsOnSuccessReturnsViewModels()
        {
            //arrange
            var distribution = new Mock<IDistribution>();
            IList<IDistribution> distributions = new[] { distribution.Object };
            var statistics = new Mock<IStatistic>();
            var parentMock = new Mock<INode>();
            parentMock.Setup(n => n.Id).Returns(1);
            parentMock.Setup(n => n.Name).Returns("name");
            parentMock.Setup(n => n.NetworkName).Returns("networkname");
            parentMock.Setup(n => n.NetworkUrl).Returns("networkurl");
            parentMock.Setup(n => n.InitialPrice).Returns(2);
            parentMock.Setup(n => n.InitialInvestment).Returns(3);
            parentMock.Setup(n => n.PortfolioWeight).Returns(4);
            parentMock.Setup(n => n.IsPortfolioComponent).Returns(true);
            parentMock.Setup(n => n.Distributions).Returns(distributions);
            parentMock.Setup(n => n.Statistics).Returns(statistics.Object);
            parentMock.Setup(n => n.Histogram).Returns(new List<HistogramDatum>());

            var childMock = new Mock<INode>();
            childMock.Setup(n => n.Id).Returns(2);
            childMock.Setup(n => n.Parent).Returns(parentMock.Object);
            childMock.Setup(n => n.Distributions).Returns(distributions);
            childMock.Setup(n => n.Statistics).Returns(statistics.Object);

            var parent = parentMock.Object;
            var child = childMock.Object;
            IDictionary<int, INode> nodes = new Dictionary<int, INode>{
                { parent.Id, parent},
                { child.Id, child}
            };

            var distributionViewModel = new DistributionViewModel();
            var distributionViewModels = new[] { distributionViewModel };
            var distributionMapper = new Mock<IDistributionMapper>();
            distributionMapper.Setup(m => m.MapDistributionsToViewModels(
                It.Is<IList<IDistribution>>(list => list == distributions)))
                .Returns(distributionViewModels);

            var statisticsViewModel = new StatisticViewModel();
            var statisticsMapper = new Mock<IStatisticMapper>();
            statisticsMapper.Setup(m => m.MapStatisticToViewModel(
                It.Is<IStatistic>(s => s == statistics.Object)))
                .Returns(statisticsViewModel);

            var nodeMapper = new NodeMapper(null, null, distributionMapper.Object, statisticsMapper.Object);

            //act
            var results = nodeMapper.MapNodesToViewModels(nodes);
            var parentResult = results[parent.Id];
            var childResult = results[child.Id];

            //assert
            Assert.AreEqual(parent.Id, parentResult.Id);
            Assert.AreEqual(parent.Name, parentResult.Name);
            Assert.AreEqual(parent.Url, parentResult.Url);
            Assert.AreEqual(parent.NetworkName, parentResult.NetworkName);
            Assert.AreEqual(parent.NetworkUrl, parentResult.NetworkUrl);
            Assert.AreEqual(parent.InitialPrice, parentResult.InitialPrice);
            Assert.AreEqual(parent.InitialInvestment, parentResult.InitialInvestment);
            Assert.AreEqual(parent.PortfolioWeight, parentResult.PortfolioWeight);
            Assert.IsTrue(parentResult.IsPortfolioComponent);
            Assert.AreSame(distributionViewModels, parentResult.Distributions);
            Assert.AreSame(statisticsViewModel, parentResult.Statistics);
            Assert.AreSame(parent.Histogram, parentResult.Histogram);

            Assert.AreEqual(child.Id, childResult.Id);
            Assert.AreEqual(parent.Id, childResult.Parent.Id);
        }

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
            Assert.IsTrue(nodeRecord1.IsPortfolioComponent);
            Assert.AreEqual(distributionId, result[1].Distributions[0].Id);
            Assert.AreEqual(nodeRecord1.Id, result[2].Parent.Id);
            Assert.AreEqual(nodeRecord1.Id, result.Values.First().Id);
            Assert.AreEqual(nodeRecord2.Id, result.Values.Last().Id);
        }
    }
}
