using LoanLogic.Enums;
using LoanLogic.Models;
using LoanLogic.Settings;

namespace LoanApplicationProcessor.Models.EligiblityRules
{
    public abstract class EligibilityRuleBase
    {
        public abstract EligibilityRuleType RuleType { get; }
        public abstract DecisionLogEntry Evaluate(LoanApplication loanApplication, EligibilitySettings eligibilitySettings);
    }
}
