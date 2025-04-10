using CarFleetManagement.Contracts;
using CarFleetManagement.Data;
using CarFleetManagement.Data.Models;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var insurances = db.InsuranceExpenses
                .Select(i => new InsuranceExpenseViewModel
                {
                    Id = i.Id,
                    CarId = i.CarId,
                    Provider = i.Provider,
                    Amount = i.Amount,
                    EndDate = i.EndDate
                }).ToList();

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
                EndDate = model.EndDate
            };

            db.InsuranceExpenses.Add(insurance);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
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
        public IActionResult Edit(InsuranceExpenseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var item = db.InsuranceExpenses.Find(model.Id);
                if (item == null) return NotFound();

                item.CarId = model.CarId;
                item.Provider = model.Provider;
                item.Amount = model.Amount;
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
        public IActionResult Delete(int id)
        {
            var item = db.InsuranceExpenses.Include(i => i.Car).FirstOrDefault(i => i.Id == id);
            if (item == null) return NotFound();

            var model = new InsuranceExpenseViewModel
            {
                Id = item.Id,
                CarId = item.CarId,
                Provider = item.Provider,
                Amount = item.Amount,
                EndDate = item.EndDate
            };

            return View(model);
        }
        [HttpPost]
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
                Provider = item.Provider,
                Amount = item.Amount,
                EndDate = item.EndDate
            };
            return View(model);
        }
    }
}
