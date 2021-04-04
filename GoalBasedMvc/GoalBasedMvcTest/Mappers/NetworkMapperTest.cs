using GoalBasedMvc.Mappers;
using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace GoalBasedMvcTest.Mappers
{
    [TestClass]
    public class NetworkMapperTest
    {
        [TestMethod]
        public void MapEntityToViewModelOnSuccessMapsProperties()
        {
            //arrange
            var url = "url";
            var name = "name";
            var network = new Mock<INetwork>();
            network.Setup(n => n.Url).Returns(url);
            network.Setup(n => n.Name).Returns(name);

            var nodeId = 1;
            var node = new Mock<INode>();
            node.Setup(n => n.Id).Returns(nodeId);
            IDictionary<int, INode> nodes = new Dictionary<int, INode> { { nodeId, node.Object } };
            network.Setup(n => n.Nodes).Returns(nodes);

            var initialValue = 100D;
            var portfolio = new Mock<IPortfolio>();
            portfolio.Setup(p => p.InitialValue).Returns(initialValue);
            network.Setup(n => n.Portfolio).Returns(portfolio.Object);

            var cashFlowId = 3;
            var cashFlow = new CashFlow { Id = cashFlowId };
            IList<CashFlow> cashFlows = new[] { cashFlow };
            network.Setup(n => n.CashFlows).Returns(cashFlows);

            var nodeViewModel = new NodeViewModel { Id = nodeId };
            IDictionary<int, NodeViewModel> nodeViewModels = new Dictionary<int, NodeViewModel> { { nodeId, nodeViewModel } };
            var nodeMapper = new Mock<INodeMapper>();
            nodeMapper.Setup(m => m.MapNodesToViewModels(
                It.Is<IDictionary<int, INode>>(dict => dict[nodeId].Id == nodeId)))
                .Returns(nodeViewModels);

            var successProbability = .5D;
            var successProbabilities = new[] { successProbability };
            var portfolioViewModel = new PortfolioViewModel
            {
                SuccessProbabilities = successProbabilities
            };
            var portfolioMapper = new Mock<IPortfolioMapper>();
            portfolioMapper.Setup(m => m.MapPortfolioToViewModel(
                It.Is<IPortfolio>(p => p.InitialValue == initialValue)))
                .Returns(portfolioViewModel);

            var networkMapper = new NetworkMapper(nodeMapper.Object, portfolioMapper.Object, null);

            //act
            var result = networkMapper.MapNetworkToViewModel(network.Object);

            //assert
            Assert.AreEqual(url, result.Url);
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(nodeId, result.Nodes[nodeId].Id);
            Assert.AreEqual(successProbability, result.Portfolio.SuccessProbabilities[0]);
            Assert.AreEqual(cashFlowId, result.CashFlows[0].Id);
        }

        [TestMethod]
        public void MapNetworkComponentsToNetworkOnSuccessReturnsNetwork()
        {
            //arrange
            var url = "url";
            var name = "name";
            var networkRecord = new NetworkRecord
            {
                Name = name,
                Url = url
            };

            var nodeId = 1;
            var node = new Mock<INode>();
            node.Setup(n => n.Id).Returns(nodeId);
            IDictionary<int, INode> nodes = new Dictionary<int, INode> { { nodeId, node.Object } };

            var cashFlowId = 3;
            var cashFlow = new CashFlow { Id = cashFlowId };
            IList<CashFlow> cashFlows = new[] { cashFlow };

            var network = new Mock<INetwork>();
            network.SetupAllProperties();

            Func<INetwork> networkFactory = () => network.Object;

            var mapper = new NetworkMapper(null, null, networkFactory);

            //act
            var result = mapper.MapNetworkComponentsToNetwork(networkRecord, nodes, cashFlows);

            //assert
            Assert.AreEqual(name, result.Name);
            Assert.AreEqual(url, result.Url);
            Assert.AreEqual(nodeId, result.Nodes[nodeId].Id);
            Assert.AreEqual(cashFlowId, result.CashFlows[0].Id);
        }
    }
}
