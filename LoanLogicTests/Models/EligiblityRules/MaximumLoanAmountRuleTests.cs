using LoanLogic.Models;
using LoanLogic.Models.EligiblityRules;
using LoanLogic.Settings;

namespace LoanLogicTests.Models.EligiblityRules
{
    [TestClass]
    public class MaximumLoanAmountRuleTests
    {
        private readonly EligibilitySettings _eligibilitySettings;

        
        public MaximumLoanAmountRuleTests()
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
        [DataRow(5d, 1d, 12)]
        [DataRow(5d, 1d, 1)]
        [DataRow(5d, 1d, 24)]
        [DataRow(5d, 1d, 99)]
        [DataRow(9000d, 2001d, 12)]
        [DataRow(9000d, 2001d, 1)]
        [DataRow(9000d, 2001d, 24)]
        [DataRow(9000d, 2001d, 99)]

        public void LoanAmountIsAboveRatio_RejectedRegardlessOfOtherValues(double loanAmount, double monthlyIncome, int term)
        {
            // Arrange
            var loanApplication = new LoanApplication
            {                
                MonthlyIncome = (decimal)monthlyIncome,
                RequestedAmount = (decimal)loanAmount,
                TermMonths = term
            };
         
            var rule = new MaximumLoanAmountRule();
            // Act
            var result = rule.Evaluate(loanApplication, _eligibilitySettings);
            // Assert
            Assert.IsFalse(result.Passed);
            Assert.IsTrue(result.Message.Contains("exceeds"));
        }

        [TestMethod]
        [DataRow(4d, 1d, 12)]
        [DataRow(4d, 1d, 1)]
        [DataRow(4d, 1d, 24)]
        [DataRow(4d, 1d, 99)]
        [DataRow(2002d, 2001d, 12)]
        [DataRow(2002d, 2001d, 1)]
        [DataRow(2002d, 2001d, 24)]
        [DataRow(2002d, 2001d, 99)]
        public void LoanAmountIsBelowRatio_AcceptedRegardlessOfOtherValues(double loanAmount, double monthlyIncome, int term)
        {
            // Arrange
            var loanApplication = new LoanApplication
            {
                MonthlyIncome = (decimal)monthlyIncome,
                RequestedAmount = (decimal)loanAmount,
                TermMonths = term
            };

            var rule = new MaximumLoanAmountRule();
            // Act
            var result = rule.Evaluate(loanApplication, _eligibilitySettings);
            // Assert
            Assert.IsTrue(result.Passed);
            Assert.IsTrue(result.Message.Contains("is within the allowed ratio"));
        }
    }
}
