using CarFleetManagement.Contracts;
using CarFleetManagement.Data;
using CarFleetManagement.Data.Models;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarFleetManagement.Controllers
{
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
    }
}
