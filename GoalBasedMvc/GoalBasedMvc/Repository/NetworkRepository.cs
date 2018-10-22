using GoalBasedMvc.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GoalBasedMvc.Repository
{
    public interface INetworkRepository
    {
        IEnumerable<NetworkSearchViewModel> GetNetworks();
    }

    public class NetworkRepository: INetworkRepository
    {
        private readonly string _connectionString;
        private readonly INodeRepository _nodeRepository;
        private readonly ICashFlowRepository _cashFlowRepository;

        public NetworkRepository(IOptions<MvcOptions> optionsAccessor)
        {
            _connectionString = optionsAccessor.Value.ConnString;
        }

        public IEnumerable<NetworkSearchViewModel> GetNetworks()
        {
            var networks = new List<NetworkSearchViewModel>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("GetNetworks", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var network = new NetworkSearchViewModel();
                        network.Id = (int)reader["Id"];
                        network.Name = (string)reader["Name"];
                        networks.Add(network);
                    }
                }
            }
            return networks;
        }


    }
}
