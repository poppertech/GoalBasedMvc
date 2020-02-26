using GoalBasedMvc.Logic;
using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GoalBasedMvcTest.Logic
{
    [TestClass]
    public class NodeServiceTest
    {
        [TestMethod]
        public void GetNodeByUrlOnSuccessReturnsNodeRecord()
        {
            //arrange
            var networkUrl = "networkurl";
            var nodeUrl = "nodeurl";

            var nodeRecord = new NodeRecord
            {
                Url = nodeUrl
            };

            var repository = new Mock<INodeRepository>();
            repository.Setup(r => r.GetNodeByUrl(
                It.Is<string>(url => url == nodeUrl),
                It.Is<string>(url => url == networkUrl)
                ))
                .Returns(nodeRecord);

            var service = new NodeService(repository.Object);

            //act
            var result = service.GetNodeByUrl(nodeUrl, networkUrl);

            //assert
            Assert.AreEqual(nodeUrl, result.Url);
        }
    }
}
