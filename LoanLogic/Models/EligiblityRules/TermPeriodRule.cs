using LoanLogic.Enums;
using LoanLogic.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLogic.Models.EligiblityRules
{
    public class TermPeriodRule : EligibilityRuleBase
    {
        public override EligibilityRuleType RuleType => EligibilityRuleType.TermPeriod;

        public override DecisionLogEntry Evaluate(LoanApplication loanApplication, EligibilitySettings eligibilitySettings)
        {
            var decision = (loanApplication.TermMonths < eligibilitySettings.MinimumTermMonths) || (loanApplication.TermMonths > eligibilitySettings.MaximumTermMonths)
                 ? LoanApplicationStatus.Rejected
                 : LoanApplicationStatus.Approved;

            return new DecisionLogEntry()
            {
                Id = Guid.NewGuid(),
                LoanApplicationId = loanApplication.Id,
                RuleName = RuleType.ToString(),
                Passed = decision == LoanApplicationStatus.Approved,
                Message = decision == LoanApplicationStatus.Approved
                    ? $"Term period of {loanApplication.TermMonths} months is within the allowed range."
                    : $"Term period of {loanApplication.TermMonths} months is outside the allowed range of {eligibilitySettings.MinimumTermMonths} to {eligibilitySettings.MaximumTermMonths} months.",
                EvaluatedAt = DateTime.UtcNow
            };
        }
    }
}
