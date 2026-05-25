namespace LoanLogic.Mappers
{
    public static class LoanApplicationMapper
    {
            public static Models.LoanApplication MapToLoanApplication(Models.LoanApplicationRequest request)
            {
                return new Models.LoanApplication
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name ?? "",
                    Email = request.Email ?? "",
                    MonthlyIncome = request.MonthlyIncome ?? 0,
                    RequestedAmount = request.RequestedAmount ?? 0,                    
                    TermMonths = request.TermMonths ?? 0,
                    Status = Enums.LoanApplicationStatus.Pending.ToString()
                };
        }
    }
}
