using LoanLogic.Enums;
using LoanLogic.Settings;

namespace LoanLogic.Models.EligiblityRules
{
    public class MonthlyIncomeRule : EligibilityRuleBase
    {
        public override EligibilityRuleType RuleType => EligibilityRuleType.MinimumMonthlyIncome;

        public override DecisionLogEntry Evaluate(LoanApplication loanApplication, EligibilitySettings eligibilitySettings)
        {
            var decision = loanApplication.MonthlyIncome < eligibilitySettings.MinimumMonthlyIncome
                ? LoanApplicationStatus.Rejected
                : LoanApplicationStatus.Approved;

            return new DecisionLogEntry()
            {
                Id = Guid.NewGuid(),
                LoanApplicationId = loanApplication.Id,
                RuleName = RuleType.ToString(),
                Passed = decision == LoanApplicationStatus.Approved,
                Message = decision == LoanApplicationStatus.Approved
                        ? $"Monthly income of {loanApplication.MonthlyIncome} meets the minimum requirement."
                        : $"Monthly income of {loanApplication.MonthlyIncome} is below the minimum requirement of {eligibilitySettings.MinimumMonthlyIncome}.",
                EvaluatedAt = DateTime.UtcNow
            };
        }
    }
}
