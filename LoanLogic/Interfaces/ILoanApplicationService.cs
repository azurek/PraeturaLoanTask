using LoanLogic.Models;

namespace LoanLogic.Interfaces
{
    public interface ILoanApplicationService
    {
        ResultWithMessage<bool> Validate(LoanApplicationRequest request);
        ResultWithMessage<LoanApplicationResult> SaveNewApplication(LoanApplicationRequest request);
        ResultWithMessage<LoanApplicationResult> GetById(Guid id);
    }
}
