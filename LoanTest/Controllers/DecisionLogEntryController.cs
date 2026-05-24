using LoanLogic.Interfaces;
using LoanLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace PraeturaLoanTask.Controllers
{
    [ApiController]
    [Route("api/decision-log-entries")]
    public class DecisionLogEntryController(ILogger<DecisionLogEntryController> logger, IDecisionLogEntryRepository decisionLogEntryRepository) : ControllerBase
    {

        [HttpGet]
        public ActionResult<List<DecisionLogEntry>> Get()
        {
            var entries = decisionLogEntryRepository.GetAll();
            return Ok(entries);
        }
    }
}
