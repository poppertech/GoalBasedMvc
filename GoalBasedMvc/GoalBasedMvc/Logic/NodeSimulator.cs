using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoalBasedMvc.Logic
{
    public interface INodeSimulator
    {
        IDictionary<int, Node> SimulateNodes(IDictionary<int, Node> nodeDictionary);
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

        public IDictionary<int, Node> SimulateNodes(IDictionary<int, Node> nodeDictionary)
        {
            var keys = nodeDictionary.Keys.ToList();
            for (int cnt = 0; cnt < nodeDictionary.Count; cnt++)
            {
                var key = keys[cnt];
                var node = nodeDictionary[key];
                var uniformRandoms = _uniformRandomRepository.GetUniformRandoms();
                SimulateNode(ref node, uniformRandoms);
            }
            return nodeDictionary;
        }

        private void SimulateNode(ref Node node, IList<double> uniformRandoms)
        {
            var simulations = new Simulation[uniformRandoms.Count];
            IList<Distribution> distributions = node.Distributions;
            var parent = node.Parent;
            _evaluator.Init(node.Distributions);
            Parallel.For(0, uniformRandoms.Count, cnt =>
            {
                var uniformRandom = uniformRandoms[cnt];
                var distributionIndex = parent == null ? 0 : parent.Simulations[cnt].DistributionIndex;
                var simulation = _evaluator.Evaluate(distributionIndex, uniformRandom);
                simulations[cnt] = simulation;
            });
            node.Simulations = simulations;
        }
    }
}
