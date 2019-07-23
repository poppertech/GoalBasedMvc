using GoalBasedMvc.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GoalBasedMvc.Repository
{
    public interface INodeRepository
    {
        IDictionary<int, INode> GetNodesByNetworkId(int networkId);
    }

    public class NodeRepository : INodeRepository
    {
        private readonly string _connectionString;
        private readonly Func<INode> _nodeFactory;
        private readonly Func<DistributionContext, IDistribution> _distributionFactory;
        private IDictionary<int, INode> _nodeDictionary;

        public NodeRepository(
            IOptions<MvcOptions> optionsAccessor, 
            Func<INode> nodeFactory,
            Func<DistributionContext, IDistribution> distributionFactory
            )
        {
            _connectionString = optionsAccessor.Value.ConnString;
            _nodeFactory = nodeFactory;
            _distributionFactory = distributionFactory;
        }

        public IDictionary<int, INode> GetNodesByNetworkId(int networkId)
        {
            _nodeDictionary = new SortedDictionary<int, INode>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("GetNodesByNetworkId", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NetworkId", networkId);

                connection.Open();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var node = GetNode(reader);
                        var distribution = GetDistribution(reader);
                        node.Distributions.Add(distribution);
                    }
                }
            }
            return _nodeDictionary;
        }

        private INode GetNode(IDataReader reader)
        {
            INode node;
            var nodeId = (int)reader["NodeId"];
            if (nodeId > 0 && !_nodeDictionary.ContainsKey(nodeId))
            {
                node = _nodeFactory();
                node.Id = nodeId;
                node.Name = (string)reader["NodeName"];
                node.InitialInvestment = reader["InitialInvestment"] != DBNull.Value ? (double?)reader["InitialInvestment"] : null;
                node.InitialPrice = reader["InitialPrice"] != DBNull.Value ? (double?)reader["InitialPrice"] : null;
                node.IsPortfolioComponent = (bool)reader["IsPortfolioComponent"];
                var parentIdObject = reader["ParentId"];
                var parentId = parentIdObject != DBNull.Value ? (int)parentIdObject : default(int);
                if (parentId > 0)
                    node.Parent = _nodeDictionary[parentId];

                _nodeDictionary.Add(nodeId, node);
            }
            else
            {
                node = _nodeDictionary[nodeId];
            }
            return node;
        }

        private IDistribution GetDistribution(IDataReader reader)
        {
            var context = new DistributionContext();
            context.Id = (int)reader["DistributionId"];
            context.Minimum = (double)reader["Minimum"];
            context.Worst = (double)reader["Worst"];
            context.Likely = (double)reader["Likely"];
            context.Best = (double)reader["Best"];
            context.Maximum = (double)reader["Maximum"];
            var distribution = _distributionFactory(context);
            return distribution;
        }

    }
}
