using GoalBasedMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoalBasedMvc.Mappers
{
    public interface INodeMapper
    {
        IDictionary<int, INode> MapNodeRecordsToNodes(IDictionary<int, NodeRecord> records);
    }

    public class NodeMapper : INodeMapper
    {
        private readonly Func<INode> _nodeFactory;
        private readonly Func<DistributionRecord, IDistribution> _distributionFactory;

        public NodeMapper(
            Func<INode> nodeFactory,
            Func<DistributionRecord, IDistribution> distributionFactory
            )
        {
            _nodeFactory = nodeFactory;
            _distributionFactory = distributionFactory;
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
