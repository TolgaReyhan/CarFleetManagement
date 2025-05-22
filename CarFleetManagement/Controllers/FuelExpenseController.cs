using CarFleetManagement.Data.Models;
using CarFleetManagement.Data;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using CarFleetManagement.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin");
            ViewBag.Cars = db.Cars
                .Where(c => isAdmin || c.UserId == userId)
                .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Model} ({c.RegistrationNumber})"
                })
                .ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Add(FuelExpenseViewModel model)
        {
            ModelState.Remove("CarDisplayName");
            ModelState.Remove("Brand");
            ModelState.Remove("FuelType");
            if (!ModelState.IsValid)
            {
                var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                bool isAdmin = User.IsInRole("Admin");
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
        public IActionResult Edit(int id)
        {
            var expense = db.FuelExpenses.Include(f => f.Car).FirstOrDefault(f => f.Id == id);
            if (expense == null) return NotFound();

            bool isAdmin = User.IsInRole("Admin");
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!isAdmin && expense.Car.UserId != userId) return Forbid();

            var model = new FuelExpenseViewModel
            {
                Id = expense.Id,
                CarId = expense.CarId,
                CarDisplayName = expense.Car.Model + " (" + expense.Car.RegistrationNumber + ")",
                Liters = expense.Liters,
                Amount = expense.Amount,
                Date = expense.Date
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
        public IActionResult Edit(FuelExpenseViewModel model)
        {
            ModelState.Remove("CarDisplayName");
            ModelState.Remove("Brand");
            ModelState.Remove("FuelType");
            if (!ModelState.IsValid)
            {
                var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
                bool isAdmin = User.IsInRole("Admin");
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

            var item = db.FuelExpenses.Include(f => f.Car).FirstOrDefault(f => f.Id == model.Id);
            if (item == null) return NotFound();
            bool isAdminEdit = User.IsInRole("Admin");
            var userIdEdit = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!isAdminEdit && item.Car.UserId != userIdEdit) return Forbid();

            item.CarId = model.CarId;
            item.Liters = model.Liters;
            item.Amount = model.Amount;
            item.Date = model.Date;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var fuel = db.FuelExpenses.Include(f => f.Car).FirstOrDefault(f => f.Id == id);
            if (fuel == null) return NotFound();
            bool isAdmin = User.IsInRole("Admin");
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!isAdmin && fuel.Car.UserId != userId) return Forbid();

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
        public IActionResult DeletePost(int id)
        {
            var fuel = db.FuelExpenses.Include(f => f.Car).FirstOrDefault(f => f.Id == id);
            if (fuel == null) return NotFound();
            bool isAdmin = User.IsInRole("Admin");
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!isAdmin && fuel.Car.UserId != userId) return Forbid();

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
                Date = fuel.Date,
                Brand = fuel.Car.Brand,
                FuelType = fuel.Car.FuelType
            };
            return View(model);
        }
    }
}
