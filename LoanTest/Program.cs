using LoanLogic;
using LoanLogic.Interfaces;
using LoanLogic.Repositories;
using LoanLogic.Services;
using PraeturaLoanTask.BuilderDependencies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ILoanApplicationRepository, LoanApplicationRepository>();
builder.Services.AddTransient<IDecisionLogEntryRepository, DecisionLogEntryRepository>();

builder.Services.AddTransient<ILoanApplicationService, LoanApplicationService>();
builder.Services.AddTransient<ILoanReviewService, LoanReviewService>();

//builder.Services.AddDbContext<LoanDbContext>(opt=> opt.UseInMemoryDatabase("LoanDb"));   
builder.Services.AddDbContext<LoanDbContext>(opt => opt.UseSqlite(@"Data Source=LoanDb.db"));

builder.Services.AddHostedService<LoanLogic.Services.LoanBackgroundService>();



var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var configuration = configBuilder.Build();

builder.Services.AddEligiblitySettigns(configuration);


builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.SetMinimumLevel(LogLevel.Warning);
});

var app = builder.Build();

// Apply pending migrations (creates tables)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LoanDbContext>();
    db.Database.EnsureCreated();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
