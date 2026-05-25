

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Loan.Common.BuilderDependencies
{
    public static class AddLocalDbContext
    {
        public static void AddSqlLite(this IServiceCollection services)
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
