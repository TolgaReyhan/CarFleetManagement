using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarFleetManagement.Data.Models
{
    public class FuelExpense
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Liters { get; set; } 
        public DateTime Date { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
