using GoalBasedMvc.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GoalBasedMvc.Repository
{
    public interface INetworkRepository
    {
        IEnumerable<NetworkViewModel> GetNetworks(string url = null);
    }

    public class NetworkRepository: INetworkRepository
    {
        private readonly string _connectionString;

        public NetworkRepository(IOptions<MvcOptions> optionsAccessor)
        {
            _connectionString = optionsAccessor.Value.ConnString;
        }

        public IEnumerable<NetworkViewModel> GetNetworks(string url = null)
        {
            var networks = new List<NetworkViewModel>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("GetNetworks", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                if(!string.IsNullOrEmpty(url))
                    command.Parameters.AddWithValue("@Url", url);

                connection.Open();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var network = new NetworkViewModel();
                        network.Id = (int)reader["Id"];
                        network.Name = (string)reader["Name"];
                        network.Url = (string)reader["Url"];
                        networks.Add(network);
                    }
                }
            }
            return networks;
        }


    }
}
