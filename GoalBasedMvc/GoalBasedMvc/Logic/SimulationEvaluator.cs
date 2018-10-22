using GoalBasedMvc.Models;
using System;
using System.Collections.Generic;

namespace GoalBasedMvc.Logic
{
    public interface ISimulationEvaluator
    {
        void Init(IList<Distribution> distributions);

        Simulation Evaluate(int distributionIndex, double uniformRandom);
    }

    public class SimulationEvaluator : ISimulationEvaluator
    {
        private IList<Distribution> _distributions;

        public void Init(IList<Distribution> distributions)
        {
            _distributions = distributions;
        }

        public Simulation Evaluate(int distributionIndex, double uniformRandom)
        {
            var simulation = new Simulation();
            var distribution = _distributions[distributionIndex];
            for (int index = 0; index < distribution.CdfProbabilities.Count; index++)
            {
                if (uniformRandom < distribution.CdfProbabilities[index])
                {
                    simulation.DistributionIndex = index;
                    simulation.Price = distribution.GetPrice(uniformRandom, index);
                    return simulation;
                }
            }
            throw new NotImplementedException();
        }

    }
}
