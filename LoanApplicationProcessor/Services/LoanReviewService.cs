using LoanApplicationProcessor.Models.EligiblityRules;
using LoanLogic.Enums;
using LoanLogic.Interfaces;
using LoanLogic.Models;
using LoanLogic.Settings;

namespace LoanApplicationProcessor.Services
{
    public class LoanReviewService(EligibilitySettings eligibilitySettings
        , ILoanApplicationRepository loanApplicationRepository
        , IDecisionLogEntryRepository decisionLogEntryRepository): ILoanReviewService
    {
        private readonly List<EligibilityRuleBase> _eligibilityRules =
        [
            new MaximumLoanAmountRule(),
            new MonthlyIncomeRule(),
            new TermPeriodRule()
        ];

        public UnitResultWithMessage ReviewLoanApplication(LoanApplication loanApplication)
        {

            var reviewResult = new UnitResultWithMessage(); 

            var decisionLogEntries = EvaluateEligibilityRules(loanApplication);

            var updateAndSaveResult = UpdateAndSaveLoanApplication(loanApplication, decisionLogEntries);
            if(!updateAndSaveResult.IsValid)
            {
                reviewResult.Messages.AddRange(updateAndSaveResult.Messages);
                return reviewResult;
            }
            
            var decisionLogSaveReult = decisionLogEntryRepository.Add(decisionLogEntries);
            decisionLogSaveReult.Messages.AddRange(decisionLogSaveReult.Messages);

            return reviewResult;
        }       

        private UnitResultWithMessage UpdateAndSaveLoanApplication(LoanApplication loanApplication, List<DecisionLogEntry> decisions)
        {            
            var finalDecision = decisions.Any(r => !r.Passed)
                ? LoanApplicationStatus.Rejected
                : LoanApplicationStatus.Approved;

            loanApplication.Status = finalDecision.ToString();
            loanApplication.ReviewedAt = DateTime.UtcNow;

            return loanApplicationRepository.Update(loanApplication);
        }

        public  List<DecisionLogEntry> EvaluateEligibilityRules(LoanApplication loanApplication)
        {

            var decisionLogEntries = new List<DecisionLogEntry>();

            foreach (var rule in _eligibilityRules)
            {
                decisionLogEntries.Add(rule.Evaluate(loanApplication, eligibilitySettings));
            }
           
            return decisionLogEntries;
        }



    }
}
