using LoanLogic.Models;

namespace LoanLogic.Interfaces
{
    public interface ILoanReviewService
    {
        UnitResultWithMessage ReviewLoanApplication(LoanApplication loanApplication);
    }
}
