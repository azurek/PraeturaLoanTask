using LoanLogic.Models;

namespace LoanApplicationProcessor.Interfaces
{
    public interface ILoanReviewService
    {
        UnitResultWithMessage ReviewLoanApplication(LoanApplication loanApplication);
    }
}
