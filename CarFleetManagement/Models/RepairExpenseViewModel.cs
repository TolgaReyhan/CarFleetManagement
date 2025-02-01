using System.ComponentModel.DataAnnotations;

namespace CarFleetManagement.Models
{
    public class RepairExpenseViewModel
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string CarModel { get; set; } // За по-удобен изглед
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
    }
}
