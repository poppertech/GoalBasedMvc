using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalBasedMvc.Logic
{
    public interface INodeSimulator
    {
        IDictionary<int, INode> SimulateNodes(IDictionary<int, INode> nodeDictionary);
    }

    public class NodeSimulator : INodeSimulator
    {
        private readonly IUniformRandomRepository _uniformRandomRepository;

        public NodeSimulator(IUniformRandomRepository uniformRandomRepository)
        {
            _uniformRandomRepository = uniformRandomRepository;
        }

        public IDictionary<int, INode> SimulateNodes(IDictionary<int, INode> nodeDictionary)
        {
            var keys = nodeDictionary.Keys.ToList();
            for (int cnt = 0; cnt < nodeDictionary.Count; cnt++)
            {
                var node = nodeDictionary[keys[cnt]];
                var uniformRandoms = _uniformRandomRepository.GetUniformRandoms();
                node.Simulations = SimulateNode(node, uniformRandoms);
            }
            return nodeDictionary;
        }

        private IList<Simulation> SimulateNode(INode node, IList<double> uniformRandoms)
        {
            var simulations = new Simulation[uniformRandoms.Count];
            IList<IDistribution> distributions = node.Distributions;
            var parent = node.Parent;
            Parallel.For(0, uniformRandoms.Count, cnt =>
            {
                var distributionIndex = parent == null ? 0 : parent.Simulations[cnt].DistributionIndex;
                simulations[cnt] = Evaluate(distributions[distributionIndex], uniformRandoms[cnt]);
            });
            return simulations;
        }

        public Simulation Evaluate(IDistribution distribution, double uniformRandom)
        {
            var simulation = new Simulation();
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
