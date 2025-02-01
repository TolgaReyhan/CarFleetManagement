using System.ComponentModel.DataAnnotations;

namespace CarFleetManagement.Models
{
    public class InsuranceExpenseViewModel
    {
        public int Id { get; set; }
        public string Provider { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CarId { get; set; }
    }
}
