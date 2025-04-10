using CarFleetManagement.Data.Models;
using CarFleetManagement.Data;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using CarFleetManagement.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace CarFleetManagement.Controllers
{
    [Authorize]
    public class MonthlyReportController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ICarService carService;

        public MonthlyReportController(ApplicationDbContext db, ICarService carService)
        {
            this.db = db;
            this.carService = carService;
        }

        public IActionResult Index()
        {
            var reports = db.MonthlyReports
                .Select(r => new MonthlyReportViewModel
                {
                    Id = r.Id,
                    CarId = r.CarId,
                    FuelLiters = r.FuelLiters,
                    FuelCost = r.FuelCost,
                    TotalKilometers = r.TotalKilometers,
                    TotalRepairs = r.TotalRepairs,
                    RepairCost = r.RepairCost,
                    InsuranceCount = r.InsuranceCount,
                    Date = r.Date
                }).ToList();

            return View(reports);
        }

        public IActionResult Add()
        {
            Dictionary<int, string> carIdNames = carService.GetCarNamesAndIds();
            ViewData["car-info"] = carIdNames;
            return View();
        }

        [HttpPost]
        public IActionResult Add(MonthlyReportViewModel model)
        {
            var report = new MonthlyReport
            {
                CarId = model.CarId,
                FuelLiters = model.FuelLiters,
                FuelCost = model.FuelCost,
                TotalKilometers = model.TotalKilometers,
                TotalRepairs = model.TotalRepairs,
                RepairCost = model.RepairCost,
                InsuranceCount = model.InsuranceCount,
                Date = model.Date
            };

            db.MonthlyReports.Add(report);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var report = db.MonthlyReports.Find(id);
            if (report == null) return NotFound();

            var model = new MonthlyReportViewModel
            {
                Id = report.Id,
                CarId = report.CarId,
                TotalKilometers = report.TotalKilometers,
                FuelLiters = report.FuelLiters,
                FuelCost = report.FuelCost,
                TotalRepairs = report.TotalRepairs,
                RepairCost = report.RepairCost,
                InsuranceCount = report.InsuranceCount,
                Date = report.Date
            };

            ViewBag.Cars = db.Cars
    .Select(c => new SelectListItem
    {
        Value = c.Id.ToString(),
        Text = $"{c.Model} ({c.RegistrationNumber})"
    })
    .ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(MonthlyReportViewModel model)
        {
            if (ModelState.IsValid)
            {
                var report = db.MonthlyReports.Find(model.Id);
                if (report == null) return NotFound();

                report.CarId = model.CarId;
                report.TotalKilometers = model.TotalKilometers;
                report.FuelLiters = model.FuelLiters;
                report.FuelCost = model.FuelCost;
                report.TotalRepairs = model.TotalRepairs;
                report.RepairCost = model.RepairCost;
                report.InsuranceCount = model.InsuranceCount;
                report.Date = model.Date;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Cars = db.Cars
    .Select(c => new SelectListItem
    {
        Value = c.Id.ToString(),
        Text = $"{c.Model} ({c.RegistrationNumber})"
    })
    .ToList();
            return View(model);
        }
        public IActionResult Delete(int id)
        {
            var report = db.MonthlyReports.Include(r => r.Car).FirstOrDefault(r => r.Id == id);
            if (report == null) return NotFound();

            var model = new MonthlyReportViewModel
            {
                Id = report.Id,
                CarId = report.CarId,
                TotalKilometers = report.TotalKilometers,
                FuelLiters = report.FuelLiters,
                FuelCost = report.FuelCost,
                TotalRepairs = report.TotalRepairs,
                RepairCost = report.RepairCost,
                InsuranceCount = report.InsuranceCount,
                Date = report.Date
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            var report = db.MonthlyReports.Find(id);
            if (report == null) return NotFound();

            db.MonthlyReports.Remove(report);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var report = db.MonthlyReports.Include(r => r.Car).FirstOrDefault(r => r.Id == id);
            if (report == null) return NotFound();
            var model = new MonthlyReportViewModel
            {
                Id = report.Id,
                CarId = report.CarId,
                TotalKilometers = report.TotalKilometers,
                FuelLiters = report.FuelLiters,
                FuelCost = report.FuelCost,
                TotalRepairs = report.TotalRepairs,
                RepairCost = report.RepairCost,
                InsuranceCount = report.InsuranceCount,
                Date = report.Date
            };
            return View(model);
        }
    }
}
