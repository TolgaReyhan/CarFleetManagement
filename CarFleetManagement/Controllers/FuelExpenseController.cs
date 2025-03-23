using CarFleetManagement.Data.Models;
using CarFleetManagement.Data;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using CarFleetManagement.Contracts;

namespace CarFleetManagement.Controllers
{
    public class FuelExpenseController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ICarService carService;

        public FuelExpenseController(ApplicationDbContext db,ICarService carService)
        {
            this.db = db;
            this.carService = carService;
        }

        public IActionResult Index()
        {
            var expenses = db.FuelExpenses
                .Select(e => new FuelExpenseViewModel
                {
                    Id = e.Id,
                    CarId = e.CarId,
                    Liters = e.Liters,
                    Amount = e.Amount,
                    Date = e.Date
                }).ToList();

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
    }
}
