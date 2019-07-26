using GoalBasedMvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace GoalBasedMvcTest.Models
{
    [TestClass]
    public class CashFlowsValidationTest
    {
        [TestMethod]
        public void IsValidWhenNoCashFlowsReturnsFalse()
        {
            //arrange
            var cashFlows = new List<CashFlow>();

            var attribute = new CashFlowsValidationAttribute();

            //act
            var result = attribute.IsValid(cashFlows);

            //assert
            Assert.IsFalse(result);
        }

    }
}
