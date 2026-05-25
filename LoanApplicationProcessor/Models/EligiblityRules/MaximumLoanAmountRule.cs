using LoanLogic.Enums;
using LoanLogic.Models;
using LoanLogic.Settings;

namespace LoanApplicationProcessor.Models.EligiblityRules
{
    public class MaximumLoanAmountRule : EligibilityRuleBase
    {
        public override EligibilityRuleType RuleType => EligibilityRuleType.MaximumLoanAmount;

        public override DecisionLogEntry Evaluate(LoanApplication loanApplication, EligibilitySettings eligibilitySettings)
        {
            var decision = (loanApplication.RequestedAmount / loanApplication.MonthlyIncome) > eligibilitySettings.MaximumRequestedAmountToMonthIncomeRatio
                ? LoanApplicationStatus.Rejected
                : LoanApplicationStatus.Approved;

            return new DecisionLogEntry()
            {
                Id = Guid.NewGuid(),
                LoanApplicationId = loanApplication.Id,
                RuleName = RuleType.ToString(),
                Passed = decision == LoanApplicationStatus.Approved,
                Message = decision == LoanApplicationStatus.Approved
                    ? $"Requested amount {loanApplication.RequestedAmount} is within the allowed ratio of monthly income."
                    : $"Requested amount {loanApplication.RequestedAmount} exceeds {eligibilitySettings.MaximumRequestedAmountToMonthIncomeRatio} of applicant's monthly income.",
            };
        }
    }
}
