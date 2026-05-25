using LoanLogic.Interfaces;
using LoanLogic.Mappers;
using LoanLogic.Models;
using System.Net.Mail;

namespace LoanLogic.Services
{
    public class LoanApplicationService(ILoanApplicationRepository loanApplicationRepository) : ILoanApplicationService
    {
        /// <summary>
        /// Save new loan application or return existing one if idempotent key matches any records
        /// </summary>
        /// <param name="request"></param>
        /// <param name="idempotentKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ResultWithMessage<LoanApplicationResult>> SaveNewApplication(LoanApplicationRequest request, string idempotentKey, CancellationToken cancellationToken)
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

            if(cancellationToken.IsCancellationRequested)
            {
                applicationSaveResult.Messages.Add(ErrorMessages.GetMessage(ErrorCode.E015));
                return applicationSaveResult;
            }

            var addResult = await loanApplicationRepository.Add(newLoanAppliaction);

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

        /// <summary>
        /// Basic validation of loan application request. No business specific rules applied, yet.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ResultWithMessage<bool> Validate(LoanApplicationRequest request, CancellationToken cancellationToken)
        {
            var validationResult = new ResultWithMessage<bool>
            {
                Result = true
            };

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

            if(cancellationToken.IsCancellationRequested)
            {
                validationResult.Messages.Add(ErrorMessages.GetMessage(ErrorCode.E015));
            }

            return validationResult;
        }

        private static ResultWithMessage<bool> ValidateName(string? name)
        {
            var nameValidationResult = new ResultWithMessage<bool>
            {
                Result = true
            };
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

        private static ResultWithMessage<bool> ValidateEmail(string? email)
        {
            var emailValidationResult = new ResultWithMessage<bool>
            {
                Result = true
            };
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

        private static ResultWithMessage<bool> ValidateMonthlyIncome(decimal? monthlyIncome)
        {
            var monthlyIncomeValidationResult = new ResultWithMessage<bool>
            {
                Result = true
            };

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

        private static ResultWithMessage<bool> ValidateRequestedAmount(decimal? requestedAmount)
        {
            var requestedAmountValidationResult = new ResultWithMessage<bool>
            {
                Result = true
            };

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

        private static ResultWithMessage<bool> ValidateTermMonths(decimal? termMonths)
        {
            var termMonthsValidationResult = new ResultWithMessage<bool>
            {
                Result = true
            };

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
