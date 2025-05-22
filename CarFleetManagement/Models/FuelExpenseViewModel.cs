using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarFleetManagement.Models
{
    public class FuelExpenseViewModel
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        [BindNever]
        public string CarDisplayName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        [CustomValidation(typeof(FuelExpenseViewModel), nameof(ValidateFuelDate))]
        public DateTime Date { get; set; }

        public static ValidationResult? ValidateFuelDate(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Now)
            {
                return new ValidationResult("Не можете да въведете разход за бъдеща дата.");
            }
            return ValidationResult.Success;
        }
        public decimal Liters { get; set; }
        public decimal Amount { get; set; }
        [BindNever]
        public string Brand { get; set; }
        [BindNever]
        public string FuelType { get; set; }
    }
}
