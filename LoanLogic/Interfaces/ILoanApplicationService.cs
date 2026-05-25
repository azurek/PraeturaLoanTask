using LoanLogic.Models;

namespace LoanLogic.Interfaces
{
    public interface ILoanApplicationService
    {
        ResultWithMessage<bool> Validate(LoanApplicationRequest request, CancellationToken cancellationToken);
        Task<ResultWithMessage<LoanApplicationResult>> SaveNewApplication(LoanApplicationRequest request, string idempotencyKey, CancellationToken cancellationToken);
        ResultWithMessage<LoanApplicationResult> GetById(Guid id);
    }
}
