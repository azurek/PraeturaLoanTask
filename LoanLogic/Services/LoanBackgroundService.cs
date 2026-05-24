using LoanLogic.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LoanLogic.Services
{
    public class LoanBackgroundService: BackgroundService
    {
        private readonly ILogger<LoanBackgroundService> _logger;
        private readonly TimeSpan _interval;
        private readonly IServiceScopeFactory _scopeFactory;

        public LoanBackgroundService(IServiceScopeFactory scopeFactory, ILogger<LoanBackgroundService> logger)
           {
            _logger = logger;
           _interval = TimeSpan.FromSeconds(60);
            _scopeFactory = scopeFactory;
        }

        private async Task ProcessLoanApplications(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var loanDbContext = scope.ServiceProvider.GetRequiredService<LoanDbContext>();
            var loanApplicationRepository = scope.ServiceProvider.GetRequiredService<ILoanApplicationRepository>();
            var loanReviewService = scope.ServiceProvider.GetRequiredService<ILoanReviewService>();

            var pendingApplications = loanApplicationRepository.GetByStatus(Enums.LoanApplicationStatus.Pending);

            _logger.LogInformation("Found {Count} pending loan applications to process.", pendingApplications.Count);

            foreach (var app in pendingApplications)
            {

                await using var transaction = await loanDbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {                   
                    loanReviewService.ReviewLoanApplication(app);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    
                    _logger.LogError(ex, "Error processing loan application with ID {ApplicationId}", app.Id);
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
