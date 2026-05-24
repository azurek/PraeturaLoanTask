using LoanLogic.Models;
using LoanLogic.Models.EligiblityRules;
using LoanLogic.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLogicTests.Models.EligiblityRules
{
    [TestClass]
    public class TermPeriodRuleTests
    {

        private readonly EligibilitySettings _eligibilitySettings;


        public TermPeriodRuleTests()
        {
            _eligibilitySettings = new EligibilitySettings
            {
                MinimumMonthlyIncome = 2000m,
                MaximumRequestedAmountToMonthIncomeRatio = 4m,
                MinimumTermMonths = 12,
                MaximumTermMonths = 24,
                
            };
        }

        [TestMethod]
        [DataRow(9d, 1d, 11)]
        [DataRow(9d, 1d, 25)]
        [DataRow(9d, 99d, 11)]
        [DataRow(9d, 99d, 25)]
        [DataRow(2001d, 1d, 11)]
        [DataRow(2001d, 1d, 25)]
        [DataRow(2001d, 2000d, 11)]
        [DataRow(2001d, 2000d, 25)]
        public void TermOutsideOfRange_RejectedRegardlessOfOtherValues(double loanAmount, double monthlyIncome, int term)
        {
            // Arrange
            var loanApplication = new LoanApplication
            {
                MonthlyIncome = (decimal)monthlyIncome,
                RequestedAmount = (decimal)loanAmount,
                TermMonths = term
            };

            var rule = new TermPeriodRule();
            // Act
            var result = rule.Evaluate(loanApplication, _eligibilitySettings);
            // Assert
            Assert.IsFalse(result.Passed);
            Assert.IsTrue(result.Message.Contains("outside the allowed range of"));
        }

        [TestMethod]
        [DataRow(9d, 1d, 12)]
        [DataRow(9d, 1d, 24)]
        [DataRow(9d, 99d, 12)]
        [DataRow(9d, 99d, 24)]
        [DataRow(2001d, 1d, 12)]
        [DataRow(2001d, 1d, 24)]
        [DataRow(2001d, 2000d, 12)]
        [DataRow(2001d, 2000d, 24)]
        public void TermWithinRange_ApprovedRegardlessOfOtherValues(double loanAmount, double monthlyIncome, int term)
        {
            // Arrange
            var loanApplication = new LoanApplication
            {
                MonthlyIncome = (decimal)monthlyIncome,
                RequestedAmount = (decimal)loanAmount,
                TermMonths = term
            };

            var rule = new TermPeriodRule();
            // Act
            var result = rule.Evaluate(loanApplication, _eligibilitySettings);
            // Assert
            Assert.IsTrue(result.Passed);
            Assert.IsTrue(result.Message.Contains(" is within the allowed range"));
        }
    }
}
