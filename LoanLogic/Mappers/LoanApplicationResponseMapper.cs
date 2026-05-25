using LoanLogic.Enums;
using LoanLogic.Models;
namespace LoanLogic.Mappers
{
    public static class LoanApplicationResponseMapper
    {
        public static LoanApplicationResult MapToLoanApplicationResult(LoanApplication loanApplication)
        {
            return new LoanApplicationResult
            {
                Id = loanApplication.Id
            ,   Status = Enum.TryParse<LoanApplicationStatus>(loanApplication.Status, out var status) ? status : LoanApplicationStatus.Rejected
            ,   CreatedAt = loanApplication.CreatedAt
            };
        }
    }
}
