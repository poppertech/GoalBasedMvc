using System;
using System.Collections.Generic;

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
                uniformRandoms[cnt] = _random.NextDouble();
            }
            return uniformRandoms;
        }
    }
}
