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
        private readonly IDictionary<int, IList<byte>> _simulationIndexes;
        private readonly IDictionary<int, IList<double>> _simulationPrices;

        public NodeSimulator(IUniformRandomRepository uniformRandomRepository)
        {
            _uniformRandomRepository = uniformRandomRepository;
            _simulationIndexes = new Dictionary<int, IList<byte>>();
            _simulationPrices = new Dictionary<int, IList<double>>();
        }

        public IDictionary<int, INode> SimulateNodes(IDictionary<int, INode> nodeDictionary)
        {
            var keys = nodeDictionary.Keys.ToList();
            for (int cnt = 0; cnt < nodeDictionary.Count; cnt++)
            {
                var node = nodeDictionary[keys[cnt]];
                var uniformRandoms = _uniformRandomRepository.GetUniformRandoms();
                node.Simulations = SimulateNode(node, uniformRandoms);
                _simulationPrices[node.Id]= node.Simulations;
            }
            _simulationIndexes.Clear();
            _simulationPrices.Clear();
            return nodeDictionary;
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
