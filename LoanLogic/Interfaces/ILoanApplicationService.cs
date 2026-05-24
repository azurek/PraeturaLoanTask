using LoanLogic.Models;

namespace LoanLogic.Interfaces
{
    public interface ILoanApplicationService
    {
        ResultWithMessage<bool> Validate(LoanApplicationRequest request);
        ResultWithMessage<LoanApplicationResult> SaveNewApplication(LoanApplicationRequest request, string idempotencyKey);
        ResultWithMessage<LoanApplicationResult> GetById(Guid id);
    }
}
