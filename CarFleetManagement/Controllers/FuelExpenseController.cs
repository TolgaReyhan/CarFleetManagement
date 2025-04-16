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
    public class FuelExpenseController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ICarService carService;

        public FuelExpenseController(ApplicationDbContext db, ICarService carService)
        {
            this.db = db;
            this.carService = carService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<FuelExpenseViewModel> expenses;
            bool isAdmin = User.IsInRole("Admin");
            if (isAdmin)
            {
                expenses = db.FuelExpenses
                .Include(e => e.Car)
                .Select(e => new FuelExpenseViewModel
                {
                    Id = e.Id,
                    CarId = e.CarId,
                    CarDisplayName = e.Car.Model + " (" + e.Car.RegistrationNumber + ")",
                    Liters = e.Liters,
                    Amount = e.Amount,
                    Date = e.Date
                }).ToList();
            }
            else
            {
                expenses = db.FuelExpenses
                .Where(e => e.Car.UserId == userId)
                .Include(e => e.Car)
                .Select(e => new FuelExpenseViewModel
                {
                    Id = e.Id,
                    CarId = e.CarId,
                    CarDisplayName = e.Car.Model + " (" + e.Car.RegistrationNumber + ")",
                    Liters = e.Liters,
                    Amount = e.Amount,
                    Date = e.Date
                }).ToList();
            }
            return View(expenses);
        }

        public IActionResult Add()
        {
            Dictionary<int, string> carIdNames = carService.GetCarNamesAndIds();
            ViewData["car-info"] = carIdNames;
            return View();
        }

        [HttpPost]
        public IActionResult Add(FuelExpenseViewModel model)
        {

            var expense = new FuelExpense
            {
                CarId = model.CarId,
                Liters = model.Liters,
                Amount = model.Amount,
                Date = model.Date
            };

            db.FuelExpenses.Add(expense);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var expense = db.FuelExpenses.FirstOrDefault(f => f.Id == id);
            if (expense == null) return NotFound();

            var model = new FuelExpenseViewModel
            {
                Id = expense.Id,
                CarId = expense.CarId,
                Liters = expense.Liters,
                Amount = expense.Amount,
                Date = expense.Date
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
        public IActionResult Edit(FuelExpenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var fuel = db.FuelExpenses.Find(model.Id);
                if (fuel == null) return NotFound();

                fuel.CarId = model.CarId;
                fuel.Liters = model.Liters;
                fuel.Amount = model.Amount;
                fuel.Date = model.Date;
                db.FuelExpenses.Update(fuel);
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
            var fuel = db.FuelExpenses.Include(f => f.Car).FirstOrDefault(f => f.Id == id);
            if (fuel == null) return NotFound();

            var model = new FuelExpenseViewModel
            {
                Id = fuel.Id,
                CarId = fuel.CarId,
                CarDisplayName = fuel.Car.Model + " (" + fuel.Car.RegistrationNumber + ")",
                Liters = fuel.Liters,
                Amount = fuel.Amount,
                Date = fuel.Date
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePost(int id)
        {
            var fuel = db.FuelExpenses.Find(id);
            if (fuel == null) return NotFound();

            db.FuelExpenses.Remove(fuel);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var fuel = db.FuelExpenses.Include(f => f.Car).FirstOrDefault(f => f.Id == id);
            if (fuel == null) return NotFound();
            var model = new FuelExpenseViewModel
            {
                Id = fuel.Id,
                CarId = fuel.CarId,
                CarDisplayName = fuel.Car.Model + " (" + fuel.Car.RegistrationNumber + ")",
                Liters = fuel.Liters,
                Amount = fuel.Amount,
                Date = fuel.Date
            };
            return View(model);
        }
    }
}
