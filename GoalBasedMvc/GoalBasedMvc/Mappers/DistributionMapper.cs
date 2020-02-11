using GoalBasedMvc.Models;
using System.Collections.Generic;

namespace GoalBasedMvc.Mappers
{
    public interface IDistributionMapper
    {
        IList<DistributionViewModel> MapDistributionsToViewModels(IList<IDistribution> distributions);
    }

    public class DistributionMapper : IDistributionMapper
    {
        public IList<DistributionViewModel> MapDistributionsToViewModels(IList<IDistribution> distributions)
        {
            var viewModels = new DistributionViewModel[distributions.Count];
            for (int cnt = 0; cnt < distributions.Count; cnt++)
            {
                viewModels[cnt] = MapDistributionToViewModel(distributions[cnt]);
            }
            return viewModels;
        }

        private DistributionViewModel MapDistributionToViewModel(IDistribution distribution)
        {
            var viewModel = new DistributionViewModel();

            viewModel.Id = distribution.Id;

            viewModel.Minimum = distribution.Minimum;
            viewModel.Worst = distribution.Worst;
            viewModel.Likely = distribution.Likely;
            viewModel.Best = distribution.Best;
            viewModel.Maximum = distribution.Maximum;

            viewModel.HeightWorst = distribution.HeightWorst;
            viewModel.HeightLikely = distribution.HeightLikely;
            viewModel.HeightBest = distribution.HeightBest;

            viewModel.Mean = distribution.Mean;
            viewModel.Stdev = distribution.Stdev;
            viewModel.Skew = distribution.Skew;
            viewModel.Kurt = distribution.Kurt;

            return viewModel;
        }
    }
}
