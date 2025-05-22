using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarFleetManagement.Models
{
    public class RepairExpenseViewModel
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        [BindNever]
        public string CarDisplayName { get; set; }
        [BindNever]
        public string Brand { get; set; }
        [BindNever]
        public string FuelType { get; set; }
    }
}
