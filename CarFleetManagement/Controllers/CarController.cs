using CarFleetManagement.Data.Models;
using CarFleetManagement.Data;
using CarFleetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.ComponentModel.DataAnnotations;

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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<CarViewModel> cars;
            bool isAdmin = User.IsInRole("Admin");
            if (isAdmin)
            {
                cars = db.Cars
                                       .Select(c => new CarViewModel
                                       {
                                           Id = c.Id,
                                           CarModel = c.Model,
                                           RegistrationNumber = c.RegistrationNumber,
                                           PurchaseDate = c.PurchaseDate,
                                           Mileage = c.Mileage
                                       }).ToList();
                                                }
            else
            {
                cars = db.Cars
                  .Where(c => c.UserId == userId)
                  .Select(c => new CarViewModel
                  {
                      Id = c.Id,
                      CarModel = c.Model,
                      RegistrationNumber = c.RegistrationNumber,
                      PurchaseDate = c.PurchaseDate,
                      Mileage = c.Mileage
                  }).ToList();

            }

            return View(cars);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CarViewModel model)
        {
            

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var car = new Car
            {
                Model = model.CarModel,
                RegistrationNumber = model.RegistrationNumber,
                PurchaseDate = model.PurchaseDate,
                Mileage = model.Mileage,
                UserId = userId,
                Brand = model.Brand,
                FuelType = model.FuelType,
            };
            db.Cars.Add(car);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
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
                Mileage = car.Mileage,
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(CarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var car = db.Cars.FirstOrDefault(c => c.Id == model.Id);
                if (car == null) return NotFound();
                car.Model = model.CarModel;
                car.RegistrationNumber = model.RegistrationNumber;
                car.PurchaseDate = model.PurchaseDate;
                car.Mileage = model.Mileage;
                car.Brand = model.Brand;
                car.FuelType = model.FuelType;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [Authorize(Roles = "Admin")]
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
                Mileage = car.Mileage
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
                Brand = car.Brand,
                FuelType = car.FuelType,
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
