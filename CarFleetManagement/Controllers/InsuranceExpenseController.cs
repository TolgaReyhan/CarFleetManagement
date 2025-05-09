﻿using CarFleetManagement.Contracts;
using CarFleetManagement.Data;
using CarFleetManagement.Data.Models;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarFleetManagement.Controllers
{
    [Authorize]
    public class InsuranceExpenseController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly ICarService carService;

        public InsuranceExpenseController(ApplicationDbContext db, ICarService carService)
        {
            this.db = db;
            this.carService = carService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<InsuranceExpenseViewModel> insurances;
            bool isAdmin = User.IsInRole("Admin");
            if (isAdmin)
            {
                insurances = db.InsuranceExpenses
                .Include(i => i.Car)
                .Select(i => new InsuranceExpenseViewModel
                {
                    Id = i.Id,
                    CarId = i.CarId,
                    CarDisplayName = i.Car.Model + " (" + i.Car.RegistrationNumber + ")",
                    Provider = i.Provider,
                    Amount = i.Amount,
                    StartDate = i.StartDate,
                    EndDate = i.EndDate
                }).ToList();
            }
            else
            {
                insurances = db.InsuranceExpenses
               .Where(i => i.Car.UserId == userId)
               .Include(i => i.Car)
               .Select(i => new InsuranceExpenseViewModel
               {
                   Id = i.Id,
                   CarId = i.CarId,
                   CarDisplayName = i.Car.Model + " (" + i.Car.RegistrationNumber + ")",
                   Provider = i.Provider,
                   Amount = i.Amount,
                   StartDate = i.StartDate,
                   EndDate = i.EndDate
               }).ToList();
            }
            return View(insurances);
        }

        public IActionResult Add()
        {
            Dictionary<int, string> carIdNames = carService.GetCarNamesAndIds();
            ViewData["car-info"] = carIdNames;
            return View();
        }

        [HttpPost]
        public IActionResult Add(InsuranceExpenseViewModel model)
        {
            var insurance = new InsuranceExpense
            {
                CarId = model.CarId,
                Provider = model.Provider,
                Amount = model.Amount,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            db.InsuranceExpenses.Add(insurance);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var item = db.InsuranceExpenses.Find(id);
            if (item == null) return NotFound();

            var model = new InsuranceExpenseViewModel
            {
                Id = item.Id,
                CarId = item.CarId,
                Provider = item.Provider,
                Amount = item.Amount,
                StartDate = item.StartDate,
                EndDate = item.EndDate
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
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(InsuranceExpenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var item = db.InsuranceExpenses.Find(model.Id);
                if (item == null) return NotFound();

                item.CarId = model.CarId;
                item.Provider = model.Provider;
                item.Amount = model.Amount;
                item.StartDate = model.StartDate;
                item.EndDate = model.EndDate;

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
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var item = db.InsuranceExpenses.Include(i => i.Car).FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();

            var model = new InsuranceExpenseViewModel
            {
                Id = item.Id,
                CarId = item.CarId,
                CarDisplayName = item.Car.Model + " (" + item.Car.RegistrationNumber + ")",
                Provider = item.Provider,
                Amount = item.Amount,
                StartDate = item.StartDate,
                EndDate = item.EndDate
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePost(int id)
        {
            var item = db.InsuranceExpenses.Find(id);
            if (item == null) return NotFound();

            db.InsuranceExpenses.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var item = db.InsuranceExpenses.Include(i => i.Car).FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();
            var model = new InsuranceExpenseViewModel
            {
                Id = item.Id,
                CarId = item.CarId,
                CarDisplayName = item.Car.Model + " (" + item.Car.RegistrationNumber + ")",
                Provider = item.Provider,
                Amount = item.Amount,
                StartDate = item.StartDate,
                EndDate = item.EndDate
            };
            return View(model);
        }
    }
}
