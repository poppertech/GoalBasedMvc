using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GoalBasedMvcTest.Models
{
    [TestClass]
    public class DistributionValidationTest
    {
        [TestMethod]
        public void IsValidWhenMinimumEqualToWorstReturnsFalse()
        {
            //arrange
            var viewModel = new DistributionEditViewModel
            {
                Minimum = 1,
                Worst = 1,
                Likely = 2,
                Best = 3,
                Maximum = 4
            };

            var attribute = new DistributionValidationAttribute();

            //act
            var result = attribute.IsValid(viewModel);

            //assert
            Assert.IsFalse(result);
        }

        public void IsValidWhenWorstEqualToLikelyReturnsFalse()
        {
            //arrange
            var viewModel = new DistributionEditViewModel
            {
                Minimum = 1,
                Worst = 2,
                Likely = 2,
                Best = 3,
                Maximum = 4
            };

            var attribute = new DistributionValidationAttribute();

            //act
            var result = attribute.IsValid(viewModel);

            //assert
            Assert.IsFalse(result);
        }

        public void IsValidWhenLikelyEqualToBestReturnsFalse()
        {
            //arrange
            var viewModel = new DistributionEditViewModel
            {
                Minimum = 1,
                Worst = 2,
                Likely = 3,
                Best = 3,
                Maximum = 4
            };

            var attribute = new DistributionValidationAttribute();

            //act
            var result = attribute.IsValid(viewModel);

            //assert
            Assert.IsFalse(result);
        }

        public void IsValidWhenBestEqualToMaximumReturnsFalse()
        {
            //arrange
            var viewModel = new DistributionEditViewModel
            {
                Minimum = 1,
                Worst = 2,
                Likely = 3,
                Best = 3,
                Maximum = 4
            };

            var attribute = new DistributionValidationAttribute();

            //act
            var result = attribute.IsValid(viewModel);

            //assert
            Assert.IsFalse(result);
        }
    }
}
