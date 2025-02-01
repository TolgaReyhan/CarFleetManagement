using System.ComponentModel.DataAnnotations;

namespace CarFleetManagement.Data.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public int Mileage { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
