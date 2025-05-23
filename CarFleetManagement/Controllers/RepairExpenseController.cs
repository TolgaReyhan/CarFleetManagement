﻿using CarFleetManagement.Contracts;
using CarFleetManagement.Data;
using CarFleetManagement.Data.Models;
using CarFleetManagement.Models;
using CarFleetManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarFleetManagement.Controllers
{
    [Authorize]
    public class RepairExpenseController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ICarService carService;

        public RepairExpenseController(ApplicationDbContext db, ICarService carService)
        {
            this.db = db;
            this.carService = carService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<RepairExpenseViewModel> repairs;
            bool isAdmin = User.IsInRole("Admin");
            if (isAdmin)
            {
                repairs = db.RepairExpenses
                .Include(r => r.Car)
                .Select(r => new RepairExpenseViewModel
                {
                    Id = r.Id,
                    CarId = r.CarId,
                    CarDisplayName = r.Car.Model + " (" + r.Car.RegistrationNumber + ")",
                    Description = r.Description,
                    Cost = r.Cost,
                    Date = r.Date
                }).ToList();
            }
            else
            {
                repairs = db.RepairExpenses
                .Where(r => r.Car.UserId == userId)
                .Include(r => r.Car)
                .Select(r => new RepairExpenseViewModel
                {
                    Id = r.Id,
                    CarId = r.CarId,
                    CarDisplayName = r.Car.Model + " (" + r.Car.RegistrationNumber + ")",
                    Description = r.Description,
                    Cost = r.Cost,
                    Date = r.Date
                }).ToList();
            }         
            return View(repairs);
        }

        public IActionResult Add()
        {
            Dictionary<int, string> carIdNames = carService.GetCarNamesAndIds();
            ViewData["car-info"] = carIdNames;
            return View();
        }

        [HttpPost]
        public IActionResult Add(RepairExpenseViewModel model)
        {
            var repair = new RepairExpense
            {
                CarId = model.CarId,
                Description = model.Description,
                Cost = model.Cost,
                Date = model.Date
            };

            db.RepairExpenses.Add(repair);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var item = db.RepairExpenses.Include(r => r.Car).FirstOrDefault(r => r.Id == id);
            if (item == null) return NotFound();

            bool isAdmin = User.IsInRole("Admin");
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!isAdmin && item.Car.UserId != userId) return Forbid();

            var model = new RepairExpenseViewModel
            {
                Id = item.Id,
                CarId = item.CarId,
                Description = item.Description,
                Cost = item.Cost,
                Date = item.Date
            };

            ViewBag.Cars = db.Cars
                .Where(c => isAdmin || c.UserId == userId)
                .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Model} ({c.RegistrationNumber})"
                })
                .ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(RepairExpenseViewModel model)
        {
            ModelState.Remove("CarDisplayName");
            ModelState.Remove("Brand");
            ModelState.Remove("FuelType");
            if (ModelState.IsValid)
            {
                var item = db.RepairExpenses.Find(model.Id);
                if (item == null) return NotFound();

                item.CarId = model.CarId;
                item.Description = model.Description;
                item.Cost = model.Cost;
                item.Date = model.Date;

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
            var item = db.RepairExpenses.Include(r => r.Car).FirstOrDefault(r => r.Id == id);
            if (item == null) return NotFound();

            var model = new RepairExpenseViewModel
            {
                Id = item.Id,
                CarId = item.CarId,
                CarDisplayName = item.Car.Model + " (" + item.Car.RegistrationNumber + ")",
                Description = item.Description,
                Cost = item.Cost,
                Date = item.Date
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            var item = db.RepairExpenses.Find(id);
            if (item == null) return NotFound();

            db.RepairExpenses.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var repair = db.RepairExpenses.Include(r => r.Car).FirstOrDefault(r => r.Id == id);
            if (repair == null) return NotFound();
            var model = new RepairExpenseViewModel
            {
                Id = repair.Id,
                CarId = repair.CarId,
                CarDisplayName = repair.Car.Model + " (" + repair.Car.RegistrationNumber + ")",
                Description = repair.Description,
                Cost = repair.Cost,
                Date = repair.Date,
                Brand = repair.Car.Brand,
                FuelType = repair.Car.FuelType
            };
            return View(model);
        }
    }
}
