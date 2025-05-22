using CarFleetManagement.Data.Models;
using CarFleetManagement.Data;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using CarFleetManagement.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CarFleetManagement.Controllers
{
    [Authorize]
    public class MonthlyReportController : Controller
    {
        private readonly ApplicationDbContext db;

        public MonthlyReportController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Generate(DateTime startDate, DateTime endDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cars = db.Cars
                .Where(c => c.UserId == userId)
                .ToList();

            var reports = cars.Select(car => {
                var totalFuelAmount = db.FuelExpenses
                    .Where(f => f.CarId == car.Id && f.Date >= startDate && f.Date <= endDate)
                    .Sum(f => (decimal?)f.Amount) ?? 0;
                int reportingDays = (endDate.Date - startDate.Date).Days + 1;
                decimal avgPerDay = reportingDays > 0 ? totalFuelAmount / reportingDays : 0;
                return new MonthlyReportViewModel
                {
                    CarModel = $"{car.Model} ({car.RegistrationNumber})",
                    StartDate = startDate,
                    EndDate = endDate,
                    TotalFuelLiters = db.FuelExpenses
                        .Where(f => f.CarId == car.Id && f.Date >= startDate && f.Date <= endDate)
                        .Sum(f => (double?)f.Liters) ?? 0,
                    TotalFuelAmount = totalFuelAmount,
                    RepairCount = db.RepairExpenses
                        .Count(r => r.CarId == car.Id && r.Date >= startDate && r.Date <= endDate),
                    TotalRepairCost = db.RepairExpenses
                        .Where(r => r.CarId == car.Id && r.Date >= startDate && r.Date <= endDate)
                        .Sum(r => (decimal?)r.Cost) ?? 0,
                    InsuranceCount = db.InsuranceExpenses
                        .Count(i => i.CarId == car.Id && i.StartDate >= startDate && i.StartDate <= endDate),
                    TotalInsuranceCost = db.InsuranceExpenses
                        .Where(i => i.CarId == car.Id && i.StartDate >= startDate && i.StartDate <= endDate)
                        .Sum(i => (decimal?)i.Amount) ?? 0,
                    ReportingDays = reportingDays,
                    AverageFuelCostPerDay = avgPerDay
                };
            }).ToList();

            return View("ReportResult", reports);
        }
            private readonly ICarService carService;
    }
}
