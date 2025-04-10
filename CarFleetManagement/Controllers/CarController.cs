using CarFleetManagement.Data.Models;
using CarFleetManagement.Data;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CarFleetManagement.Controllers
{
    [Authorize]
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
                    CarModel = c.Model,
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
        public IActionResult Add(CarViewModel input)
        {
            
            var car = new Car
            {
                Model = input.CarModel,
                RegistrationNumber = input.RegistrationNumber,
                PurchaseDate = input.PurchaseDate,
                Mileage = input.Mileage
            };

            db.Cars.Add(car);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var car = db.Cars.FirstOrDefault(c => c.Id == id);
            if (car == null) return NotFound();

            var model = new CarViewModel
            {
                Id = car.Id,
                CarModel = car.Model,
                RegistrationNumber = car.RegistrationNumber,
                PurchaseDate = car.PurchaseDate,
                Mileage= car.Mileage
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(CarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var car = db.Cars.FirstOrDefault(c => c.Id == model.Id);
                if (car == null) return NotFound();

                car.Model = model.CarModel;
                car.RegistrationNumber = model.RegistrationNumber;
                car.PurchaseDate = model.PurchaseDate;
                car.Mileage = model.Mileage;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public IActionResult Delete(int id)
        {
            var car = db.Cars.Find(id);
            if (car == null) return NotFound();

            var model = new CarViewModel
            {
                Id = car.Id,
                CarModel = car.Model,
                RegistrationNumber = car.RegistrationNumber,
                PurchaseDate = car.PurchaseDate,
                Mileage= car.Mileage
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult DeletePost(int id)
        {
            var car = db.Cars.Find(id);
            if (car == null) return NotFound();

            db.Cars.Remove(car);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var car = db.Cars.Find(id);
            if (car == null) return NotFound();

            var model = new CarViewModel
            {
                Id = car.Id,
                CarModel = car.Model,
                RegistrationNumber = car.RegistrationNumber,
                PurchaseDate = car.PurchaseDate,
                Mileage = car.Mileage
            };

            return View(model);
        }
    }
}
