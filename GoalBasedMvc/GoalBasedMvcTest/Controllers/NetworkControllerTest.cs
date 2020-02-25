using GoalBasedMvc.Controllers;
using GoalBasedMvc.Logic;
using GoalBasedMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvcTest.Controllers
{
    [TestClass]
    public class NetworkControllerTest
    {
        [TestMethod]
        public void GetNetworksOnSuccessReturnsNetworks()
        {
            //arrange
            var network = new NetworkRecord { Id = 1 };
            var networks = new[] { network };

            var service = new Mock<INetworkService>();
            service.Setup(s => s.GetNetworks()).Returns(networks);

            var controller = new NetworkController(service.Object, null);

            //act
            var viewResult = (ViewResult)controller.Index();
            var result = viewResult.Model as IEnumerable<NetworkRecord>;

            //assert
            Assert.AreEqual(network.Id, result.First().Id);
        }

        [TestMethod]
        public void GetByUrlReturnsNetwork()
        {
            //arrange
            var url = "url";
            var networkViewModel = new NetworkViewModel { Url = url };
            var service = new Mock<INetworkService>();
            service.Setup(s => s.GetNetworkByUrl(It.Is<string>(u=> u == url))).Returns(networkViewModel);

            var controller = new NetworkController(service.Object, null);

            //act
            var response = (ViewResult)controller.Get(url);
            var result = response.Model as NetworkViewModel;

            //assert
            Assert.AreEqual(url, result.Url);
            service.Verify(s => s.GetNetworkByUrl(It.Is<string>(u => u == url)));
        }

        [TestMethod]
        public void GetNodesOnSuccessReturnsNetwork()
        {
            //arrange 
            var url = "url";
            var network = new NetworkViewModel { Url = url};

            var service = new Mock<INetworkService>();
            service.Setup(s => s.GetNetworkByUrl(It.Is<string>(u => u == url))).Returns(network);

            var controller = new NetworkController(service.Object, null);

            //act
            var response = (ViewResult)controller.Nodes(url);
            var result = response.Model as NetworkViewModel;

            //assert
            Assert.AreEqual(url, result.Url);
        }

        [TestMethod]
        public void NodeOnSuccessReturnsNode()
        {
            //arrange
            var nodeId = 1;
            var networkName = "networkname";
            var networkUrl = "networkurl";
            var nodeUrl = "nodeurl";

            var nodeRecord = new NodeRecord { Id = nodeId};

            var nodeService = new Mock<INodeService>();
            nodeService.Setup(s => s.GetNodeByUrl(It.Is<string>(url => url == nodeUrl), It.Is<string>(url => url == networkUrl))).Returns(nodeRecord);

            var node = new NodeViewModel { Id = nodeId };
            var dictionary = new Dictionary<int, NodeViewModel> { { nodeId, node } };
            var network = new NetworkViewModel {
                Url = networkUrl,
                Name = networkName,
                Nodes = dictionary
            };

            var networkService = new Mock<INetworkService>();
            networkService.Setup(s => s.GetNetworkByUrl(It.Is<string>(url => url == networkUrl))).Returns(network);

            var controller = new NetworkController(networkService.Object, nodeService.Object);

            //act
            var result = (ViewResult)controller.Node(networkUrl, nodeUrl);
            var nodeResult = (NodeViewModel)result.Model;

            Assert.AreEqual("Node", result.ViewName);
            Assert.AreEqual(networkName, nodeResult.NetworkName);
            Assert.AreEqual(networkUrl, nodeResult.NetworkUrl);
        }
    }
}
