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
        public IActionResult Add(CarViewModel input)
        {
            
            var car = new Car
            {
                Model = input.Model,
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
                Model = car.Model,
                RegistrationNumber = car.RegistrationNumber,
                PurchaseDate = car.PurchaseDate
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

                car.Model = model.Model;
                car.RegistrationNumber = model.RegistrationNumber;
                car.PurchaseDate = model.PurchaseDate;

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
                Model = car.Model,
                RegistrationNumber = car.RegistrationNumber,
                PurchaseDate = car.PurchaseDate
            };

            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
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
                Model = car.Model,
                RegistrationNumber = car.RegistrationNumber,
                PurchaseDate = car.PurchaseDate,
            };

            return View(model);
        }
    }
}
