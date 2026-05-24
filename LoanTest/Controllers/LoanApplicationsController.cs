using Microsoft.AspNetCore.Mvc;
using LoanLogic.Models;
using LoanLogic.Interfaces;
using LoanLogic;
using System.Threading.Tasks;

namespace PraeturaLoanTask.Controllers
{
    [ApiController]
    [Route("api/loan-applications")]
    public class LoanApplicationsController(ILoanApplicationService loanValidationService, ILogger<LoanApplicationsController> logger) : ControllerBase
    {


        [HttpPost]        
        public ActionResult<LoanApplicationResult> Post([FromBody] LoanApplicationRequest loanApplicationRequest)
        {
            var idempotentKey = Request.Headers["Idempotency-Key"].FirstOrDefault();
            var validationResult = loanValidationService.Validate(loanApplicationRequest);

           

            if (!validationResult.IsValid)
            {
                logger.LogWarning("Loan application validation failed: {Messages}", string.Join(", ", validationResult.Messages));
                return BadRequest(new { Errors = validationResult.Messages });
            }

            var saveApplicationResult = loanValidationService.SaveNewApplication(loanApplicationRequest, idempotentKey);
            if (!saveApplicationResult.IsValid)
            {
                logger.LogError("Failed to save loan application: {Messages}", string.Join(", ", saveApplicationResult.Messages));
                return StatusCode(500, new { Errors = saveApplicationResult.Messages });
            }

            return Created("api/loan-applications", saveApplicationResult.Result);

        }

        [HttpGet("{id}")]
        public ActionResult<LoanApplicationResult> Get(Guid id)
        {
            var application = loanValidationService.GetById(id);
            if (!application.IsValid)
            {
                return NotFound();
            }

            return Ok(application.Result);
        }
    }
}
