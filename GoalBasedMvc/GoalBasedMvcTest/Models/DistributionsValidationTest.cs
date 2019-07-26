using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GoalBasedMvcTest.Models
{
    [TestClass]
    public class DistributionsValidationTest
    {
        [TestMethod]
        public void IsValidWhenNoDistributionsReturnsFalse()
        {
            //arrange
            var distributions = new List<DistributionEditViewModel>();

            var attribute = new DistributionsValidationAttribute();

            //act
            var result = attribute.IsValid(distributions);

            //assert
            Assert.IsFalse(result);
        }

    }
}
