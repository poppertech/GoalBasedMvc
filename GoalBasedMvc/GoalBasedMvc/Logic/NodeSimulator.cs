using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalBasedMvc.Logic
{
    public interface INodeSimulator
    {
        void SimulateNodes(ref IDictionary<int, Node> nodeDictionary);
    }

    public class NodeSimulator : INodeSimulator
    {
        private readonly ISimulationEvaluator _evaluator;
        private readonly IUniformRandomRepository _uniformRandomRepository;

        public NodeSimulator(ISimulationEvaluator evaluator, IUniformRandomRepository uniformRandomRepository)
        {
            _evaluator = evaluator;
            _uniformRandomRepository = uniformRandomRepository;
        }

        public void SimulateNodes(ref IDictionary<int, Node> nodeDictionary)
        {

            var nodes = nodeDictionary.Values.ToArray();

            for (int cnt = 0; cnt < nodes.Length; cnt++)
            {
                var node = nodes[cnt];
                var uniformRandoms = _uniformRandomRepository.GetUniformRandoms();
                nodeDictionary[node.Id] = SimulateNode(node, uniformRandoms);
            }

        }

        private Node SimulateNode(Node node, IList<double> uniformRandoms)
        {
            var simulations = new Simulation[uniformRandoms.Count];
            IList<Distribution> distributions = node.Distributions;
            _evaluator.Init(node.Distributions);
            Parallel.For(0, uniformRandoms.Count, cnt =>
            {
                var uniformRandom = uniformRandoms[cnt];
                var distributionIndex = node.Parent == null ? 0 : node.Parent.Simulations[cnt].DistributionIndex;
                var simulation = _evaluator.Evaluate(distributionIndex, uniformRandom);
                simulations[cnt] = simulation;
            });
            node.Simulations = simulations;
            return node;
        }
    }
}
