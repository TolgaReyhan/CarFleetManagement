﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarFleetManagement.Data.Models;

namespace CarFleetManagement.Data.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public int Mileage { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string UserId { get; set; }
        public string Brand { get; set; }
        public string FuelType { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
