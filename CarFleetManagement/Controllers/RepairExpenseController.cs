using CarFleetManagement.Data;
using CarFleetManagement.Data.Models;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarFleetManagement.Controllers
{
    public class RepairExpenseController : Controller
    {
        private readonly ApplicationDbContext db;

        public RepairExpenseController(ApplicationDbContext db)
        {
            this.db = db;
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
