using System.ComponentModel.DataAnnotations;

namespace CarFleetManagement.Models
{
    public class RepairExpenseViewModel
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string CarDisplayName { get; set; }
    }
}
