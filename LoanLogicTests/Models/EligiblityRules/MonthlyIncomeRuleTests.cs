using LoanApplicationProcessor.Models.EligiblityRules;
using LoanLogic.Models;
using LoanLogic.Settings;

namespace LoanApplicationProcessorTests.Models.EligiblityRules
{
    [TestClass]
    public class MonthlyIncomeRuleTests
    {

        private readonly EligibilitySettings _eligibilitySettings;


        public MonthlyIncomeRuleTests()
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
        [DataRow(9d, 1999.99d, 12)]
        [DataRow(9d, 1999.99d, 1)]
        [DataRow(9d, 1999.99d, 24)]
        [DataRow(9d, 1999.99d, 99)]
        [DataRow(999999d, 1999.99d, 12)]
        [DataRow(999999d, 1999.99d, 1)]
        [DataRow(999999d, 1999.99d, 24)]
        [DataRow(999999d, 1999.99d, 99)]
        public void MonthlyIncomeToolow_RejectedRegardlessOfOtherValues(double loanAmount, double monthlyIncome, int term)
        {
            // Arrange
            var loanApplication = new LoanApplication
            {
                Name = "",
                Email = "",
                Status = "",
                MonthlyIncome = (decimal)monthlyIncome,
                RequestedAmount = (decimal)loanAmount,
                TermMonths = term
            };

            var rule = new MonthlyIncomeRule();
            // Act
            var result = rule.Evaluate(loanApplication, _eligibilitySettings);
            // Assert
            Assert.IsFalse(result.Passed);
            Assert.IsTrue(result.Message.Contains("is below"));
        }

        [TestMethod]
        [DataRow(9d, 2000.001d, 12)]
        [DataRow(9d, 2000.001d, 1)]
        [DataRow(9d, 2000.001d, 24)]
        [DataRow(9d, 2000.001d, 99)]
        [DataRow(999999d, 2000.001d, 12)]
        [DataRow(999999d, 2000.001d, 1)]
        [DataRow(999999d, 2000.001d, 24)]
        [DataRow(999999d, 2000.001d, 99)]
        public void LoanAmountIsBelowRatio_AcceptedRegardlessOfOtherValues(double loanAmount, double monthlyIncome, int term)
        {
            // Arrange
            var loanApplication = new LoanApplication
            {
                Name = "",
                Email = "",
                Status = "",
                MonthlyIncome = (decimal)monthlyIncome,
                RequestedAmount = (decimal)loanAmount,
                TermMonths = term
            };

            var rule = new MonthlyIncomeRule();
            // Act
            var result = rule.Evaluate(loanApplication, _eligibilitySettings);
            // Assert
            Assert.IsTrue(result.Passed);
            Assert.IsTrue(result.Message.Contains("meets the minimum requiremen"));
        }

    }
}
