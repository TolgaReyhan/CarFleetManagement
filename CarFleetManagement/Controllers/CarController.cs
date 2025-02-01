using CarFleetManagement.Data.Models;
using CarFleetManagement.Data;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarFleetManagement.Controllers
{
    public class CarController : Controller
    {
        private readonly ApplicationDbContext db;

        public CarController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var cars = db.Cars
                .Select(c => new CarViewModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    RegistrationNumber = c.RegistrationNumber,
                    PurchaseDate = c.PurchaseDate,
                    Mileage = c.Mileage
                }).ToList();

            return View(cars);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(CarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var car = new Car
                {
                    Model = model.Model,
                    RegistrationNumber = model.RegistrationNumber,
                    PurchaseDate = model.PurchaseDate,
                    Mileage = model.Mileage
                };

                db.Cars.Add(car);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}
