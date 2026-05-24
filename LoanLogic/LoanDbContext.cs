using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanLogic
{
    public class LoanDbContext  : DbContext    
    {

        public LoanDbContext(DbContextOptions<LoanDbContext> options): base(options)
        {
        }
        public DbSet<Models.LoanApplication> LoanApplications { get; set; }
        public DbSet<Models.DecisionLogEntry> DecisionLogEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=LoanDb.db");
            }            
        }
    }
}
