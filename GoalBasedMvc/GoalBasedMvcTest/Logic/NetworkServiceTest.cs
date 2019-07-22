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
            repository.Setup(r => r.GetNetworks(It.IsAny<string>())).Returns(networks);

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
            var url = "url";
            var networkId = 1;
            IDictionary<int, Node> nodeDictionary = new SortedDictionary<int, Node>();
            var cashFlows = new CashFlow[0];

            var view = new NetworkViewModel { Id = 1 };
            var networks = new[] { view };

            var repository = new Mock<INetworkRepository>();
            repository.Setup(r => r.GetNetworks(It.Is<string>(u => u == url))).Returns(networks);

            var nodeRepository = new Mock<INodeRepository>();
            nodeRepository.Setup(r => r.GetNodesByNetworkId(It.Is<int>(id => id == networkId))).Returns(nodeDictionary);

            var cashFlowRepository = new Mock<ICashFlowRepository>();
            cashFlowRepository.Setup(r => r.GetCashFlowsByNetworkId(It.Is<int>(id => id == networkId))).Returns(cashFlows);

            var network = new Mock<INetwork>();

            var service = new NetworkService(repository.Object, nodeRepository.Object, cashFlowRepository.Object, network.Object);

            //act
            var result = service.GetNetworkByUrl(url);

            //assert
            Assert.AreSame(network.Object, result);
            network.Verify(n => n.Calculate());
        }

        [TestMethod]
        public void CalculateNetworkOnSuccessReturnsNetwork()
        {
            //arrange
            var parent = new Node { Id = 1 };
            var child = new Node { Id = 2, Parent = parent };
            var dictionary = new SortedDictionary<int, Node> {
                {parent.Id, parent },
                {child.Id, child }
            };

            var cashFlow = new CashFlow { Id = 3, Cost = 100 };
            var cashFlows = new[] { cashFlow };

            var viewModel = new NetworkEditViewModel {
                Nodes = dictionary,
                CashFlows = cashFlows
            };

            var network = new Mock<INetwork>();
            var service = new NetworkService(null, null, null, network.Object);

            //act
            var result = service.CalculateNetwork(viewModel);

            network.VerifySet(n => n.CashFlows = It.Is<IList<CashFlow>>(c => c[0].Id == cashFlow.Id));
            network.VerifySet(n => n.Nodes = It.Is<IDictionary<int, Node>>(d => d[parent.Id].Id == parent.Id && d[child.Id].Id == child.Id));
            network.Verify(n => n.Calculate());
        }

    }
}
