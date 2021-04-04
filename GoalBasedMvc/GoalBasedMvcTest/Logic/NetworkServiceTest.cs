using GoalBasedMvc.Logic;
using GoalBasedMvc.Mappers;
using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using Microsoft.Extensions.Caching.Memory;
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
        public void GetNetworkByUrlReturnNetwork()
        {
            //arrange
            var url = "url";
            var networkId = 1;
            var nodeId = 2;
            var cashFlowId = 3;

            var nodeRecord = new NodeRecord { Id = nodeId };
            var nodeRecords = new SortedDictionary<int, NodeRecord>();
            nodeRecords.Add(nodeId, nodeRecord);

            var node = new Mock<INode>();
            node.Setup(n => n.Id).Returns(nodeId);
            var nodes = new SortedDictionary<int, INode>();
            nodes.Add(nodeId, node.Object);

            var cashFlow = new CashFlow { Id = cashFlowId};
            var cashFlows = new[] { cashFlow};

            var networkRecord = new NetworkRecord { Id = 1 };
            var networkRecords = new[] { networkRecord };

            var networkViewModel = new NetworkViewModel { Url = url };

            var repository = new Mock<INetworkRepository>();
            repository.Setup(r => r.GetNetworks(It.Is<string>(u => u == url))).Returns(networkRecords);

            var nodeRepository = new Mock<INodeRepository>();
            nodeRepository.Setup(r => r.GetNodesByNetworkId(It.Is<int>(id => id == networkId))).Returns(nodeRecords);

            var nodeMapper = new Mock<INodeMapper>();
            nodeMapper
                .Setup(m => m.MapNodeRecordsToNodes(It.Is<IDictionary<int, NodeRecord>>(dict => dict[nodeId].Id == nodeId)))
                .Returns(nodes);

            var cashFlowRepository = new Mock<ICashFlowRepository>();
            cashFlowRepository.Setup(r => r.GetCashFlowsByNetworkId(It.Is<int>(id => id == networkId))).Returns(cashFlows);

            var network = new Mock<INetwork>();
            network.Setup(n => n.Url).Returns(url);

            var networkMapper = new Mock<INetworkMapper>();
            networkMapper
                .Setup(m => m.MapNetworkComponentsToNetwork(
                    It.Is<NetworkRecord>(nr => nr.Id == networkRecord.Id),
                    It.Is<IDictionary<int, INode>>(dict => dict[nodeId].Id == nodeId),
                    It.Is<IList<CashFlow>>(cfs => cfs[0].Id == cashFlow.Id)
                ))
                .Returns(network.Object);

            networkMapper
                .Setup(m => m.MapNetworkToViewModel(
                    It.Is<INetwork>(n => n.Url == url))
                )
                .Returns(networkViewModel);

            var options = new MemoryCacheOptions();
            var cache = new MemoryCache(options);

            var service = new NetworkService(repository.Object, nodeRepository.Object, cashFlowRepository.Object, networkMapper.Object, nodeMapper.Object, cache);

            //act
            var result = service.GetNetworkByUrl(url);

            //assert
            Assert.AreEqual(url, result.Url);
            network.Verify(n => n.Calculate());
        }

    }
}
