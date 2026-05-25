using LoanLogic.Enums;
using LoanLogic.Models;

namespace LoanLogic.Interfaces
{
    public interface ILoanApplicationRepository
    {
        Task<ResultWithMessage<LoanApplication>> Add(LoanApplication loanAppliaction);
        
        List<LoanApplication> GetByStatus(LoanApplicationStatus loanApplicationStatus, int batchSize = 0);
        UnitResultWithMessage Update(LoanApplication loanApplication);

        ResultWithMessage<LoanApplication> GetById(Guid id);

        LoanApplication? GetByIdempotentKey(string idempotentKey);
    }
}
