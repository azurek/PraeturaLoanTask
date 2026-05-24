using LoanLogic.Enums;
using LoanLogic.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLogic.Models.EligiblityRules
{
    public abstract class EligibilityRuleBase
    {
        public abstract EligibilityRuleType RuleType { get; }
        public abstract DecisionLogEntry Evaluate(LoanApplication loanApplication, EligibilitySettings eligibilitySettings);
    }
}
