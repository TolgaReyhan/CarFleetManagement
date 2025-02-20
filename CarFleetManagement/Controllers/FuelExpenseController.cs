using CarFleetManagement.Data.Models;
using CarFleetManagement.Data;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarFleetManagement.Controllers
{
    public class FuelExpenseController : Controller
    {
        private readonly ApplicationDbContext db;

        public FuelExpenseController(ApplicationDbContext db)
        {
            this.db = db;
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
