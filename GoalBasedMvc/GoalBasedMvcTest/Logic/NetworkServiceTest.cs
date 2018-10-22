using GoalBasedMvc.Logic;
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
            var network = new NetworkViewModel { Id = 1 };
            var networks = new[] { network };

            var repository = new Mock<INetworkRepository>();
            repository.Setup(r => r.GetNetworks()).Returns(networks);

            var service = new NetworkService(repository.Object, null, null, null);

            //act
            var results = service.GetNetworks();

            //assert
            Assert.AreEqual(network.Id, results.First().Id);
        }

        [TestMethod]
        public void GetNetworkByIdReturnNetwork()
        {
            //arrange
            var networkId = 1;
            IDictionary<int, Node> nodeDictionary = new SortedDictionary<int, Node>();
            var cashFlows = new CashFlow[0];

            var nodeRepository = new Mock<INodeRepository>();
            nodeRepository.Setup(r => r.GetNodesByNetworkId(It.Is<int>(id => id == networkId))).Returns(nodeDictionary);

            var cashFlowRepository = new Mock<ICashFlowRepository>();
            cashFlowRepository.Setup(r => r.GetCashFlowsByNetworkId(It.Is<int>(id => id == networkId))).Returns(cashFlows);

            var network = new Mock<INetwork>();

            var service = new NetworkService(null, nodeRepository.Object, cashFlowRepository.Object, network.Object);

            //act
            var result = service.GetNetworkById(networkId);

            //assert
            Assert.AreSame(network.Object, result);
            network.Verify(n => n.Calculate(ref It.Ref<IDictionary<int, Node>>.IsAny, It.Is<IList<CashFlow>>(c => c == cashFlows)));
        }


    }
}
