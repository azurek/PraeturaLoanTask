using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LoanLogic.Database
{
    public static class DbDepencyInjector
    {
        public static void AddDatabaseDependencies(this IServiceCollection services)
        {
            string folder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Praetura");
            Directory.CreateDirectory(folder);
            string dbPath = Path.Combine(folder, "loanDb.db");

            services.AddDbContext<LoanDbContext>(opt => opt.UseSqlite($"Data Source={dbPath}"));
        }
    }
}
