using LoanLogic.Enums;
using LoanLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLogic.Mappers
{
    public static class LoanApplicationResponseMapper
    {
        public static LoanApplicationResult MapToLoanApplicationResult(this Models.LoanApplication loanApplication)
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
