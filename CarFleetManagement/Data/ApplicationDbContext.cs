using CarFleetManagement.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarFleetManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.Database.Migrate();
        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<FuelExpense> FuelExpenses { get; set; }
        public DbSet<InsuranceExpense> InsuranceExpenses { get; set; }
        public DbSet<RepairExpense> RepairExpenses { get; set; }
        public DbSet<MonthlyReport> MonthlyReports { get; set; }
    }
}
