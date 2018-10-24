using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GoalBasedMvc.Repository
{
    public interface IUniformRandomRepository
    {
        IList<double> GetUniformRandoms();
    }

    public class UniformRandomRepository : IUniformRandomRepository
    {
        private static readonly Random _random = new Random();
        public IList<double> GetUniformRandoms()
        {
            var uniformRandoms = new double[1000000];
            for (int cnt = 0; cnt < uniformRandoms.Length; cnt++)
            {
                uniformRandoms[cnt] = _random.NextDouble()*100;
            }
            return uniformRandoms;
        }
    }

    public class UniformRandomDbRepository: IUniformRandomRepository
    {
        private readonly string _connectionString;

        public UniformRandomDbRepository(IOptions<MvcOptions> optionsAccessor)
        {
            _connectionString = optionsAccessor.Value.ConnString;
        }

        public IList<double> GetUniformRandoms()
        {
            var uniformRandoms = new List<double>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("GetUniformRandom", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();

                using (IDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var uniformRandom = (double)reader["Rand"];
                        uniformRandoms.Add(uniformRandom);
                    }
                }
            }
            return uniformRandoms;
        }
    }
}
