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
            var network = new NetworkViewModel { Id = 1 };
            var networks = new[] { network };

            var service = new Mock<INetworkService>();
            service.Setup(s => s.GetNetworks()).Returns(networks);

            var controller = new NetworkController(service.Object);

            //act
            var viewResult = (ViewResult)controller.Index();
            var result = viewResult.Model as IEnumerable<NetworkViewModel>;

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

            var controller = new NetworkController(service.Object);

            //act
            var response = (ViewResult)controller.Edit(url);
            var result = response.Model as INetwork;

            //assert
            Assert.AreSame(network.Object, result);
            service.Verify(s => s.GetNetworkByUrl(It.Is<string>(u => u == url)));
        }

        [TestMethod]
        public void PostReturnsNetwork()
        {
            //arrange
            var network = new Mock<INetwork>();

            var controller = new NetworkController(null);

            //act
            var response = (JsonResult)controller.Edit(network.Object);
            var result = response.Value as INetwork;

            //assert
            Assert.AreSame(network.Object, result);
        }
    }
}
