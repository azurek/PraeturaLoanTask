using LoanLogic.Models;

namespace LoanLogic.Interfaces
{
    public interface IDecisionLogEntryRepository
    {
        UnitResultWithMessage Add(IEnumerable<DecisionLogEntry> logEntries);
        List<DecisionLogEntry> GetAll();
    }
}
