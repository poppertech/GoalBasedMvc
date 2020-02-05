﻿using GoalBasedMvc.Models;
using GoalBasedMvc.Repository;
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
        private readonly ISimulationEvaluator _evaluator;
        private readonly IUniformRandomRepository _uniformRandomRepository;

        public NodeSimulator(ISimulationEvaluator evaluator, IUniformRandomRepository uniformRandomRepository)
        {
            _evaluator = evaluator;
            _uniformRandomRepository = uniformRandomRepository;
        }

        public IDictionary<int, INode> SimulateNodes(IDictionary<int, INode> nodeDictionary)
        {
            var keys = nodeDictionary.Keys.ToList();
            for (int cnt = 0; cnt < nodeDictionary.Count; cnt++)
            {
                var node = nodeDictionary[keys[cnt]];
                var uniformRandoms = _uniformRandomRepository.GetUniformRandoms();
                SimulateNode(ref node, uniformRandoms);
            }
            return nodeDictionary;
        }

        private void SimulateNode(ref INode node, IList<double> uniformRandoms)
        {
            var simulations = new Simulation[uniformRandoms.Count];
            IList<IDistribution> distributions = node.Distributions;
            var parent = node.Parent;
            _evaluator.Init(node.Distributions);
            Parallel.For(0, uniformRandoms.Count, cnt =>
            {
                var distributionIndex = parent == null ? 0 : parent.Simulations[cnt].DistributionIndex;
                simulations[cnt] = _evaluator.Evaluate(distributionIndex, uniformRandoms[cnt]);
            });
            node.Simulations = simulations;
        }
    }
}
