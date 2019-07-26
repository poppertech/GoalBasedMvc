using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GoalBasedMvcTest.Models
{
    [TestClass]
    public class NodesValidationAttributeTest
    {
        [TestMethod]
        public void IsValidWhenNoItemsReturnsFalse()
        {
            //arrange
            var dictionary = new SortedDictionary<int, NodeEditViewModel>();

            var attribute = new NodesValidationAttribute();

            //act
            var result = attribute.IsValid(dictionary);

            //assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidWhenZeroKeyReturnsFalse()
        {
            //arrange
            var dictionary = new SortedDictionary<int, NodeEditViewModel> { { 0, new NodeEditViewModel()} };

            var attribute = new NodesValidationAttribute();

            //act
            var result = attribute.IsValid(dictionary);

            //assert
            Assert.IsFalse(result);
        }
    }
}
