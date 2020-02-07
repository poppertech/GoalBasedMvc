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
        IDictionary<int, NodeRecord> GetNodesByNetworkId(int networkId);
        NodeRecord GetNodeByUrl(string nodeUrl, string networkUrl);
    }

    public class NodeRepository : INodeRepository
    {
        private readonly string _connectionString;
        private readonly Func<DistributionRecord, IDistribution> _distributionFactory;
        private IDictionary<int, NodeRecord> _nodeDictionary;

        public NodeRepository(
            IOptions<MvcOptions> optionsAccessor, 
            Func<DistributionRecord, IDistribution> distributionFactory
            )
        {
            _connectionString = optionsAccessor.Value.ConnString;
            _distributionFactory = distributionFactory;
        }

        public IDictionary<int, NodeRecord> GetNodesByNetworkId(int networkId)
        {
            _nodeDictionary = new SortedDictionary<int, NodeRecord>();

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

        public NodeRecord GetNodeByUrl(string nodeUrl, string networkUrl)
        {
            var node = new NodeRecord();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("GetNodeByUrl", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@NodeUrl", nodeUrl);
                command.Parameters.AddWithValue("@NetworkUrl", networkUrl);

                connection.Open();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        node.Id = (int)reader["NodeId"];
                        node.Name = (string)reader["NodeName"];
                        node.Url = (string)reader["NodeUrl"];
                        node.InitialInvestment = reader["InitialInvestment"] != DBNull.Value ? (double?)reader["InitialInvestment"] : null;
                        node.InitialPrice = reader["InitialPrice"] != DBNull.Value ? (double?)reader["InitialPrice"] : null;
                        node.IsPortfolioComponent = (bool)reader["IsPortfolioComponent"];
                    }
                }
            }
            return node;
        }

        private NodeRecord GetNode(IDataReader reader)
        {
            NodeRecord node;
            var nodeId = (int)reader["NodeId"];
            if (nodeId > 0 && !_nodeDictionary.ContainsKey(nodeId))
            {
                node = new NodeRecord();
                node.Id = nodeId;
                node.Name = (string)reader["NodeName"];
                node.Url = (string)reader["NodeUrl"];
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

        private DistributionRecord GetDistribution(IDataReader reader)
        {
            var record = new DistributionRecord();
            record.Id = (int)reader["DistributionId"];
            record.Minimum = (double)reader["Minimum"];
            record.Worst = (double)reader["Worst"];
            record.Likely = (double)reader["Likely"];
            record.Best = (double)reader["Best"];
            record.Maximum = (double)reader["Maximum"];
            return record;
        }

    }
}
