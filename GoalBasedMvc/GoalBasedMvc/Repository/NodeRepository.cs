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
        IDictionary<int, Node> GetNodesByNetworkId(int networkId);
    }

    public class NodeRepository : INodeRepository
    {
        private readonly string _connectionString;
        private IDictionary<int, Node> _nodeDictionary;

        public NodeRepository(IOptions<MvcOptions> optionsAccessor)
        {
            _connectionString = optionsAccessor.Value.ConnString;
        }

        public IDictionary<int, Node> GetNodesByNetworkId(int networkId)
        {
            _nodeDictionary = new SortedDictionary<int, Node>();

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

        private Node GetNode(IDataReader reader)
        {
            Node node;
            var nodeId = (int)reader["NodeId"];
            if (nodeId > 0 && !_nodeDictionary.ContainsKey(nodeId))
            {
                node = new Node { Id = nodeId };
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

        private Distribution GetDistribution(IDataReader reader)
        {
            var id = (int)reader["DistributionId"];
            var index = (int)reader["DistributionIndex"];
            var minimum = (double)reader["Minimum"];
            var worst = (double)reader["Worst"];
            var likely = (double)reader["Likely"];
            var best = (double)reader["Best"];
            var maximum = (double)reader["Maximum"];
            return new Distribution(id: id, minimum: minimum, worst: worst, likely: likely, best: best, maximum: maximum);
        }

    }
}
