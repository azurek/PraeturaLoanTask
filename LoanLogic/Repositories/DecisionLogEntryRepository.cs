using LoanLogic.Database;
using LoanLogic.Interfaces;
using LoanLogic.Models;

namespace LoanLogic.Repositories
{
    public class DecisionLogEntryRepository(LoanDbContext loanDbContext): IDecisionLogEntryRepository
    {
        public UnitResultWithMessage Add(IEnumerable<DecisionLogEntry> logEntries)
        {
            var result = new UnitResultWithMessage();
            try
            {
            
                loanDbContext.DecisionLogEntries.AddRange(logEntries);
                loanDbContext.SaveChanges();                
            }
            catch (Exception ex)
            {
                result.Messages.Add(string.Format(ErrorMessages.GetMessage(ErrorCode.E013), ex.Message));
            }
            return result;
        }

        /// <summary>
        /// Get all decision log entries. For testing in postman
        /// </summary>
        /// <returns></returns>
        public List<DecisionLogEntry> GetAll()
        {
            return loanDbContext.DecisionLogEntries.ToList();
        }
    }
}
