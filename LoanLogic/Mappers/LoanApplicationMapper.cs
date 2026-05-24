using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLogic.Mappers
{
    public static class LoanApplicationMapper
    {
            public static Models.LoanApplication MapToLoanApplication(Models.LoanApplicationRequest request)
            {
                return new Models.LoanApplication
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Email = request.Email,
                    MonthlyIncome = request.MonthlyIncome.Value,
                    RequestedAmount = request.RequestedAmount.Value,                    
                    TermMonths = request.TermMonths.Value,
                    Status = Enums.LoanApplicationStatus.Pending.ToString()
                };
        }
    }
}
