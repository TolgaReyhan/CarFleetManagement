using System.ComponentModel.DataAnnotations;

namespace CarFleetManagement.Models
{
    public class FuelExpenseViewModel
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string CarModel { get; set; }
        public DateTime Date { get; set; }
        public decimal Liters { get; set; }
        public decimal Amount { get; set; }
    }
}
