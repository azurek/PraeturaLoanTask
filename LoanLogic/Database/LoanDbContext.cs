using Microsoft.EntityFrameworkCore;

namespace LoanLogic.Database
{
    public class LoanDbContext  : DbContext    
    {

        public LoanDbContext(DbContextOptions<LoanDbContext> options): base(options)
        {
        }
        public DbSet<Models.LoanApplication> LoanApplications { get; set; }
        public DbSet<Models.DecisionLogEntry> DecisionLogEntries { get; set; }       
    }
}
