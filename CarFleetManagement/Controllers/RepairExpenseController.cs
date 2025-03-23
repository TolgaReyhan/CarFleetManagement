using CarFleetManagement.Contracts;
using CarFleetManagement.Data;
using CarFleetManagement.Data.Models;
using CarFleetManagement.Models;
using CarFleetManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarFleetManagement.Controllers
{
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
            var repairs = db.RepairExpenses
                .Select(r => new RepairExpenseViewModel
                {
                    Id = r.Id,
                    CarId = r.CarId,
                    Description = r.Description,
                    Cost = r.Cost,
                    Date = r.Date
                }).ToList();

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
    }
}
