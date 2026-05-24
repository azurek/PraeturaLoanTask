using LoanLogic.Interfaces;
using LoanLogic.Mappers;
using LoanLogic.Models;
using System.Net.Mail;

namespace LoanLogic.Services
{
    public class LoanApplicationService(ILoanApplicationRepository loanApplicationRepository) : ILoanApplicationService
    {
        public ResultWithMessage<LoanApplicationResult> SaveNewApplication(LoanApplicationRequest request, string idempotentKey)
        {            
            var applicationSaveResult = new ResultWithMessage<LoanApplicationResult>();

            if (!string.IsNullOrWhiteSpace(idempotentKey))
            {
                var loanApplication = loanApplicationRepository.GetByIdempotentKey(idempotentKey);
                if (loanApplication != null)
                {
                    applicationSaveResult.Result = LoanApplicationResponseMapper.MapToLoanApplicationResult(loanApplication);
                    return applicationSaveResult;
                }
            }
            

            var newLoanAppliaction = LoanApplicationMapper.MapToLoanApplication(request);
            newLoanAppliaction.IdempotentKey = idempotentKey;
            var addResult = loanApplicationRepository.Add(newLoanAppliaction);

            if(addResult.IsValid)
            {
                applicationSaveResult.Result = LoanApplicationResponseMapper.MapToLoanApplicationResult(addResult.Result);
            }
            else
            {
                applicationSaveResult.Messages.AddRange(addResult.Messages);
            }


            return applicationSaveResult;
        }

        public ResultWithMessage<LoanApplicationResult> GetById(Guid id)
        {
            var getResult = new ResultWithMessage<LoanApplicationResult>();
            var repositoryResult = loanApplicationRepository.GetById(id);
            if(!repositoryResult.IsValid)
            {
                getResult.Messages.AddRange(repositoryResult.Messages);
                return getResult;
            }

            getResult.Result = LoanApplicationResponseMapper.MapToLoanApplicationResult(repositoryResult.Result);
            return getResult;
        }

        public ResultWithMessage<bool> Validate(LoanApplicationRequest request)
        {
            var validationResult = new ResultWithMessage<bool>();
            validationResult.Result = true;

            var nameValidationResult = ValidateName(request.Name);
            validationResult.Messages.AddRange(nameValidationResult.Messages);


            var emailValidationResult = ValidateEmail(request.Email);
            validationResult.Messages.AddRange(emailValidationResult.Messages);

            var monthlyIncomeValidationResult = ValidateMonthlyIncome(request.MonthlyIncome);
            validationResult.Messages.AddRange(monthlyIncomeValidationResult.Messages);

            var requestedAmountValidationResult = ValidateRequestedAmount(request.RequestedAmount);
            validationResult.Messages.AddRange(requestedAmountValidationResult.Messages);

            var termMonthsValidationResult = ValidateTermMonths(request.TermMonths);
            validationResult.Messages.AddRange(termMonthsValidationResult.Messages);

            return validationResult;
        }

        internal ResultWithMessage<bool> ValidateName(string name)
        {
            var nameValidationResult = new ResultWithMessage<bool>();
            nameValidationResult.Result = true;
            if (string.IsNullOrEmpty(name))
            {
                nameValidationResult.Messages.Add(ErrorMessages.GetMessage(ErrorCode.E001));
            }
            else
            {
                var nameParts = name.Split(' ');
                if (nameParts.Length < 2)
                {
                    nameValidationResult.Messages.Add(ErrorMessages.GetMessage(ErrorCode.E004));
                }
            }
            return nameValidationResult;
        }

        internal ResultWithMessage<bool> ValidateEmail(string email)
        {
            var emailValidationResult = new ResultWithMessage<bool>();
            emailValidationResult.Result = true;
            if (string.IsNullOrEmpty(email))
            {
                emailValidationResult.Messages.Add(ErrorMessages.GetMessage(ErrorCode.E002));
            }
            else if (!MailAddress.TryCreate(email, out var _))
            {
                emailValidationResult.Messages.Add(ErrorMessages.GetMessage(ErrorCode.E003));
            }
            return emailValidationResult;
        }

        internal ResultWithMessage<bool> ValidateMonthlyIncome(decimal? monthlyIncome)
        {
            var monthlyIncomeValidationResult = new ResultWithMessage<bool>();
            monthlyIncomeValidationResult.Result = true;
            if (monthlyIncome == null)
            {
                monthlyIncomeValidationResult.Messages.Add(ErrorMessages.GetMessage(ErrorCode.E005));
            }
            else if(monthlyIncome <= 0)
            {
                monthlyIncomeValidationResult.Messages.Add(ErrorMessages.GetMessage(ErrorCode.E006));
            }

            return monthlyIncomeValidationResult;
        }

        internal ResultWithMessage<bool> ValidateRequestedAmount(decimal? requestedAmount)
        {
            var requestedAmountValidationResult = new ResultWithMessage<bool>();
            requestedAmountValidationResult.Result = true;
            if (requestedAmount == null)
            {
                requestedAmountValidationResult.Messages.Add(ErrorMessages.GetMessage(ErrorCode.E007));
            }
            else if (requestedAmount <= 0)
            {
                requestedAmountValidationResult.Messages.Add(ErrorMessages.GetMessage(ErrorCode.E008));
            }

            return requestedAmountValidationResult;
        }

        internal ResultWithMessage<bool> ValidateTermMonths(decimal? termMonths)
        {
            var termMonthsValidationResult = new ResultWithMessage<bool>();
            termMonthsValidationResult.Result = true;
            if (termMonths  == null)
            {
                termMonthsValidationResult.Messages.Add(ErrorMessages.GetMessage(ErrorCode.E009));
            }
            else if (termMonths <= 0)
            {
                termMonthsValidationResult.Messages.Add(ErrorMessages.GetMessage(ErrorCode.E010));
            }

            return termMonthsValidationResult;
        }

    }
}
