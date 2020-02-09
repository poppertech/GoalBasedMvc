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
            var network = new Mock<INetwork>();
            var service = new Mock<INetworkService>();
            service.Setup(s => s.GetNetworkByUrl(It.Is<string>(u=> u == url))).Returns(network.Object);

            var controller = new NetworkController(service.Object, null);

            //act
            var response = (ViewResult)controller.Get(url);
            var result = response.Model as INetwork;

            //assert
            Assert.AreSame(network.Object, result);
            service.Verify(s => s.GetNetworkByUrl(It.Is<string>(u => u == url)));
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

            var node = new Mock<INode>();
            node.SetupAllProperties();
            var dictionary = new SortedDictionary<int, INode> { { nodeId, node.Object } };
            var network = new Mock<INetwork>();
            network.Setup(n => n.Nodes).Returns(dictionary);
            network.Setup(n => n.Name).Returns(networkName);
            network.Setup(n => n.Url).Returns(networkUrl);

            var networkService = new Mock<INetworkService>();
            networkService.Setup(s => s.GetNetworkByUrl(It.Is<string>(url => url == networkUrl))).Returns(network.Object);

            var controller = new NetworkController(networkService.Object, nodeService.Object);

            //act
            var result = (ViewResult)controller.Node(networkUrl, nodeUrl);
            var nodeResult = (INode)result.Model;

            Assert.AreEqual("Node", result.ViewName);
            Assert.AreEqual(networkName, nodeResult.NetworkName);
            Assert.AreEqual(networkUrl, nodeResult.NetworkUrl);
        }
    }
}
