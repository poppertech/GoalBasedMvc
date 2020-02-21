using GoalBasedMvc.Logic;
using GoalBasedMvc.Mappers;
using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvcTest.Logic
{
    [TestClass]
    public class NetworkServiceTest
    {
        [TestMethod]
        public void GetNetworksOnSuccessReturnsNetworks()
        {
            //arrange
            var network = new NetworkRecord { Id = 1 };
            var networks = new[] { network };

            var repository = new Mock<INetworkRepository>();
            repository.Setup(r => r.GetNetworks(It.IsAny<string>())).Returns(networks);

            var service = new NetworkService(repository.Object, null, null, null, null, null);

            //act
            var results = service.GetNetworks();

            //assert
            Assert.AreEqual(network.Id, results.First().Id);
        }

        [TestMethod]
        public void GetNetworkByIdReturnNetwork()
        {
            //arrange
            var url = "url";
            var networkId = 1;
            var nodeId = 2;

            var nodeRecord = new NodeRecord { Id = nodeId };
            var nodeRecords = new SortedDictionary<int, NodeRecord>();
            nodeRecords.Add(nodeId, nodeRecord);

            var node = new Mock<INode>();
            node.Setup(n => n.Id).Returns(nodeId);
            var nodes = new SortedDictionary<int, INode>();
            nodes.Add(nodeId, node.Object);

            var cashFlows = new CashFlow[0];

            var view = new NetworkRecord { Id = 1 };
            var networks = new[] { view };

            var repository = new Mock<INetworkRepository>();
            repository.Setup(r => r.GetNetworks(It.Is<string>(u => u == url))).Returns(networks);

            var nodeRepository = new Mock<INodeRepository>();
            nodeRepository.Setup(r => r.GetNodesByNetworkId(It.Is<int>(id => id == networkId))).Returns(nodeRecords);

            var nodeMapper = new Mock<INodeMapper>();
            nodeMapper
                .Setup(m => m.MapNodeRecordsToNodes(It.Is<IDictionary<int, NodeRecord>>(dict => dict[nodeId].Id == nodeId)))
                .Returns(nodes);

            var cashFlowRepository = new Mock<ICashFlowRepository>();
            cashFlowRepository.Setup(r => r.GetCashFlowsByNetworkId(It.Is<int>(id => id == networkId))).Returns(cashFlows);
            var network = new Mock<INetwork>();

            var service = new NetworkService(repository.Object, nodeRepository.Object, cashFlowRepository.Object, null, nodeMapper.Object, null);

            //act
            var result = service.GetNetworkByUrl(url);

            //assert
            Assert.AreSame(null, result);
            network.Verify(n => n.Calculate());
        }

    }
}
