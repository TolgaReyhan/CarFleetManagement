using CarFleetManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CarFleetManagement.Data
{
    public static class DatabaseSeeder
    {
        public static void Seed(ApplicationDbContext db, string userId)
        {
            // Seed Cars
            if (!db.Cars.Any())
            {
                db.Cars.AddRange(
                    new Car { Model = "C220", RegistrationNumber = "BP 4024 CX", Mileage = 120000, PurchaseDate = new DateTime(2020, 5, 1), UserId = userId, Brand = "Mercedes", FuelType = "Diesel" },
                    new Car { Model = "Focus", RegistrationNumber = "CA 1234 AB", Mileage = 90000, PurchaseDate = new DateTime(2019, 3, 15), UserId = userId, Brand = "Ford", FuelType = "Petrol" }
                );
                db.SaveChanges();
            }

            // Seed FuelExpenses
            if (!db.FuelExpenses.Any())
            {
                var car = db.Cars.First();
                db.FuelExpenses.Add(new FuelExpense
                {
                    CarId = car.Id,
                    Liters = 50,
                    Amount = 120,
                    Date = DateTime.Now.AddDays(-10)
                });
                db.SaveChanges();
            }

            // Seed InsuranceExpenses
            if (!db.InsuranceExpenses.Any())
            {
                var car = db.Cars.First();
                db.InsuranceExpenses.Add(new InsuranceExpense
                {
                    CarId = car.Id,
                    Provider = "SDI",
                    Amount = 140,
                    StartDate = DateTime.Now.AddMonths(-1),
                    EndDate = DateTime.Now.AddMonths(11)
                });
                db.SaveChanges();
            }

            // Seed RepairExpenses
            if (!db.RepairExpenses.Any())
            {
                var car = db.Cars.First();
                db.RepairExpenses.Add(new RepairExpense
                {
                    CarId = car.Id,
                    Description = "Oil change",
                    Cost = 80,
                    Date = DateTime.Now.AddDays(-20)
                });
                db.SaveChanges();
            }
        }
    }
} 