using LoanLogic.Enums;
using LoanLogic.Interfaces;
using LoanLogic.Mappers;
using LoanLogic.Models;
namespace LoanLogic.Repositories
{
    public class LoanApplicationRepository(LoanDbContext loanDbContext) : ILoanApplicationRepository
    {
        public ResultWithMessage<LoanApplication> Add(LoanApplication loanAppliaction)
        {
            var applicationSaveResult = new ResultWithMessage<LoanApplication>();
            try
            {
                loanAppliaction.CreatedAt = DateTime.UtcNow;
                loanDbContext.LoanApplications.Add(loanAppliaction);
                loanDbContext.SaveChanges();

                applicationSaveResult.Result = loanAppliaction;
            }
            catch (Exception ex)
            {
                applicationSaveResult.Messages.Add(string.Format(ErrorMessages.GetMessage(ErrorCode.E011), ex.Message));
            }

            return applicationSaveResult;
        }


        public ResultWithMessage<LoanApplication> GetById(Guid id)
        {
            var getResult = new ResultWithMessage<LoanApplication>();
            var loanApplication = loanDbContext.LoanApplications.FirstOrDefault(la => la.Id == id);
            if (loanApplication == null)
            {
                getResult.Messages.Add(string.Format(ErrorMessages.GetMessage(ErrorCode.E012), id));
            }
            else
            {
                getResult.Result = loanApplication;
            }

            return getResult;
        }

        public List<LoanApplication> GetByStatus(LoanApplicationStatus loanApplicationStatus, int batchSize = 0)
        {
            var all = loanDbContext.LoanApplications.Where(la => la.Status == loanApplicationStatus.ToString());
            if(batchSize > 0)
            {
                all = all.Take(batchSize);
            }

            return all.ToList();
        }

        public UnitResultWithMessage Update(LoanApplication loanApplication)
        {
            var updateResult = new UnitResultWithMessage();
            try
            {
                loanDbContext.LoanApplications.Update(loanApplication);
                loanDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                updateResult.Messages.Add(string.Format(ErrorMessages.GetMessage(ErrorCode.E013), ex.Message));
            }
            return updateResult;
        }

        public LoanApplication? GetByIdempotentKey(string idempotentKey)
        {            
            return loanDbContext.LoanApplications.FirstOrDefault(la => la.IdempotentKey == idempotentKey);                        
        }
    }
}
