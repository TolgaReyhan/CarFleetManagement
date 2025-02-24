﻿using CarFleetManagement.Data.Models;
using CarFleetManagement.Data;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarFleetManagement.Controllers
{
    public class MonthlyReportController : Controller
    {
        private readonly ApplicationDbContext db;

        public MonthlyReportController(ApplicationDbContext db)
        {
            this.db = db;
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
                    Month = r.Month,
                    Year = r.Year
                }).ToList();

            return View(reports);
        }

        public IActionResult Add()
        {
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
                    Month = model.Month,
                    Year = model.Year
                };

                db.MonthlyReports.Add(report);
                db.SaveChanges();

                return RedirectToAction("Index");
        }
    }
}
