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
        IDictionary<int, IList<double>> SimulateNodes(IDictionary<int, INode> nodeDictionary);
    }

    public class NodeSimulator : INodeSimulator
    {
        private readonly IUniformRandomRepository _uniformRandomRepository;
        private IDictionary<int, IList<byte>> _simulationIndexes;

        public NodeSimulator(IUniformRandomRepository uniformRandomRepository)
        {
            _uniformRandomRepository = uniformRandomRepository;
        }

        public IDictionary<int, IList<double>> SimulateNodes(IDictionary<int, INode> nodeDictionary)
        {
            IDictionary<int, IList<double>> simulationPrices = new Dictionary<int, IList<double>>();
            _simulationIndexes = new Dictionary<int, IList<byte>>();
            foreach (var key in nodeDictionary.Keys)
            {
                var node = nodeDictionary[key];
                var uniformRandoms = _uniformRandomRepository.GetUniformRandoms();
                simulationPrices[node.Id] = SimulateNode(node, uniformRandoms);
            }
            _simulationIndexes = null;
            return simulationPrices;
        }

        private IList<double> SimulateNode(INode node, IList<double> uniformRandoms)
        {
            var simulationIndexes = new byte[uniformRandoms.Count];
            var simulationPrices = new double[uniformRandoms.Count];
            IList<IDistribution> distributions = node.Distributions;
            var parent = node.Parent;
            Parallel.For(0, uniformRandoms.Count, cnt =>
            {
                var distributionIndex = parent == null ? 0 : _simulationIndexes[parent.Id][cnt];
                var simulation = Evaluate(distributions[distributionIndex], uniformRandoms[cnt]);
                simulationIndexes[cnt] = simulation.DistributionIndex;
                simulationPrices[cnt] = simulation.Price;
            });
            _simulationIndexes[node.Id] = simulationIndexes;
            return simulationPrices;
        }

        public Simulation Evaluate(IDistribution distribution, double uniformRandom)
        {
            var simulation = new Simulation();
            for (byte index = 0; index < distribution.CdfProbabilities.Count; index++)
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
