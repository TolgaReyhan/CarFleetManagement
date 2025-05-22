using CarFleetManagement.Contracts;
using CarFleetManagement.Data;
using CarFleetManagement.Data.Models;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
        public IActionResult Add(InsuranceExpenseViewModel model)
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
        public IActionResult Edit(int id)
        {
            var item = db.InsuranceExpenses.Include(i => i.Car).FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();

            bool isAdmin = User.IsInRole("Admin");
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!isAdmin && item.Car.UserId != userId) return Forbid();

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
        public IActionResult Edit(InsuranceExpenseViewModel model)
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

            var item = db.InsuranceExpenses.Include(i => i.Car).FirstOrDefault(i => i.Id == model.Id);
            if (item == null) return NotFound();
            bool isAdminEdit = User.IsInRole("Admin");
            var userIdEdit = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!isAdminEdit && item.Car.UserId != userIdEdit) return Forbid();

            item.CarId = model.CarId;
            item.Provider = model.Provider;
            item.Amount = model.Amount;
            item.StartDate = model.StartDate;
            item.EndDate = model.EndDate;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var item = db.InsuranceExpenses.Include(i => i.Car).FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();
            bool isAdmin = User.IsInRole("Admin");
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!isAdmin && item.Car.UserId != userId) return Forbid();

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
        public IActionResult DeletePost(int id)
        {
            var item = db.InsuranceExpenses.Include(i => i.Car).FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();
            bool isAdmin = User.IsInRole("Admin");
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (!isAdmin && item.Car.UserId != userId) return Forbid();

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
                EndDate = item.EndDate,
                Brand = item.Car.Brand,
                FuelType = item.Car.FuelType
            };
            return View(model);
        }
    }
}
