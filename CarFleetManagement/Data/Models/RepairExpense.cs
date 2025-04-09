using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarFleetManagement.Data.Models
{
    public class RepairExpense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
