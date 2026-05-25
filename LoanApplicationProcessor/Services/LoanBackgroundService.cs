using LoanApplicationProcessor.Interfaces;
using LoanLogic.Database;
using LoanLogic.Interfaces;
namespace LoanApplicationProcessor.Services
{
    public class LoanBackgroundService(IServiceScopeFactory scopeFactory, ILogger<LoanBackgroundService> logger) : BackgroundService
    {
        private readonly ILogger<LoanBackgroundService> _logger = logger;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(60);
        private readonly IServiceScopeFactory _scopeFactory = scopeFactory;

        /// <summary>
        /// Processes pending loan application by reviewing them and updating their status accordingly. 
        /// Each application is processed within a transaction to ensure data integrity between LoanApplication and DecisionLogEntry.         
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ProcessLoanApplications(CancellationToken cancellationToken)
        {
           using var scope = _scopeFactory.CreateScope();
            var loanDbContext = scope.ServiceProvider.GetRequiredService<LoanDbContext>();
            var loanApplicationRepository = scope.ServiceProvider.GetRequiredService<ILoanApplicationRepository>();
            var loanReviewService = scope.ServiceProvider.GetRequiredService<ILoanReviewService>();

            var pendingApplications = loanApplicationRepository.GetByStatus(LoanLogic.Enums.LoanApplicationStatus.Pending);

            _logger.LogInformation("Found {Count} pending loan applications to process.", pendingApplications.Count);

            foreach (var pendingApplication in pendingApplications)
            {

                await using var transaction = await loanDbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {                   
                    loanReviewService.ReviewLoanApplication(pendingApplication);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    
                    _logger.LogError(ex, "Error processing loan application with ID {ApplicationId}", pendingApplication.Id);
                    await transaction.RollbackAsync(cancellationToken);
                }                
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            using var timer = new PeriodicTimer(_interval);
            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                   await ProcessLoanApplications(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {                
                _logger.LogInformation("LoanBackgroundService is stopping due to cancellation.");
            }           
        }
    }
}
