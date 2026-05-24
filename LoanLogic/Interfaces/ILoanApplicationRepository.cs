using LoanLogic.Enums;
using LoanLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLogic.Interfaces
{
    public interface ILoanApplicationRepository
    {
        ResultWithMessage<LoanApplication> Add(LoanApplication loanAppliaction);
        ResultWithMessage<LoanApplication> GetById(Guid id);
        List<LoanApplication> GetByStatus(LoanApplicationStatus loanApplicationStatus, int batchSize = 0);
        UnitResultWithMessage Update(LoanApplication loanApplication);
    }
}
