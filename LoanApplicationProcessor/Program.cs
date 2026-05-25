using LoanApplicationProcessor.BuilderDependencies;
using LoanApplicationProcessor.Interfaces;
using LoanApplicationProcessor.Services;
using LoanLogic.Database;
using LoanLogic.Interfaces;
using LoanLogic.Repositories;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<LoanBackgroundService>();

builder.Services.AddTransient<ILoanApplicationRepository, LoanApplicationRepository>();
builder.Services.AddTransient<IDecisionLogEntryRepository, DecisionLogEntryRepository>();
builder.Services.AddTransient<ILoanReviewService, LoanReviewService>();


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();


DbDepencyInjector.AddDatabaseDependencies(builder.Services);

builder.Services.AddEligiblitySettigns(configuration);

builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.SetMinimumLevel(LogLevel.Warning);
});

var host = builder.Build();
host.Run();
