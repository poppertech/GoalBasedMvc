using GoalBasedMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Mappers
{
    public interface INodeMapper
    {
        IDictionary<int, NodeViewModel> MapNodesToViewModels(IDictionary<int, INode> nodes);
        IDictionary<int, INode> MapNodeRecordsToNodes(IDictionary<int, NodeRecord> records);
    }

    public class NodeMapper : INodeMapper
    {
        private readonly Func<INode> _nodeFactory;
        private readonly Func<DistributionRecord, IDistribution> _distributionFactory;

        private readonly IDistributionMapper _distributionMapper;
        private readonly IStatisticMapper _statisticMapper;

        public NodeMapper(
            Func<INode> nodeFactory,
            Func<DistributionRecord, IDistribution> distributionFactory,
            IDistributionMapper distributionMapper,
            IStatisticMapper statisticMapper
            )
        {
            _nodeFactory = nodeFactory;
            _distributionFactory = distributionFactory;
            _distributionMapper = distributionMapper;
            _statisticMapper = statisticMapper;
        }

        public IDictionary<int, NodeViewModel> MapNodesToViewModels(IDictionary<int, INode> nodes)
        {
            var viewModels = new SortedDictionary<int, NodeViewModel>();
            foreach (var key in nodes.Keys)
            {
                var node = nodes[key];
                var viewModel = MapNodeToViewModel(node);
                if (node.Parent != null)
                    viewModel.Parent = viewModels[node.Parent.Id];
                viewModels.Add(key, viewModel);
            }
            return viewModels;
        }

        private NodeViewModel MapNodeToViewModel(INode node)
        {
            var viewModel = new NodeViewModel();
            viewModel.Id = node.Id;
            viewModel.Name = node.Name;
            viewModel.Url = node.Url;
            viewModel.NetworkName = node.NetworkName;
            viewModel.NetworkUrl = node.NetworkUrl;
            viewModel.InitialPrice = node.InitialPrice;
            viewModel.InitialInvestment = node.InitialInvestment;
            viewModel.PortfolioWeight = node.PortfolioWeight;
            viewModel.IsPortfolioComponent = node.IsPortfolioComponent;
            viewModel.Distributions = _distributionMapper.MapDistributionsToViewModels(node.Distributions);
            viewModel.Statistics = _statisticMapper.MapStatisticToViewModel(node.Statistics);
            viewModel.Histogram = node.Histogram;
            return viewModel;
        }

        public IDictionary<int, INode> MapNodeRecordsToNodes(IDictionary<int, NodeRecord> records)
        {
            var nodes = new SortedDictionary<int, INode>();
            foreach (var key in records.Keys)
            {
                var record = records[key];
                var node = MapNodeRecordToNode(record);
                if(records[key].Parent != null)
                    node.Parent = nodes[records[key].Parent.Id];
                nodes.Add(key, node);
            }
            return nodes;
        }

        private INode MapNodeRecordToNode(NodeRecord record)
        {
            var node = _nodeFactory();
            node.Id = record.Id;
            node.Name = record.Name;
            node.Url = record.Url;
            node.NetworkName = record.NetworkName;
            node.NetworkUrl = record.NetworkUrl;
            node.InitialPrice = record.InitialPrice;
            node.InitialInvestment = record.InitialInvestment;
            node.PortfolioWeight = record.PortfolioWeight;
            node.IsPortfolioComponent = record.IsPortfolioComponent;
            node.Distributions = record.Distributions.Select(r => _distributionFactory(r)).ToList();
            return node;
        }
    }
}
