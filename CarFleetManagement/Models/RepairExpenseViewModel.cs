using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarFleetManagement.Models
{
    public class RepairExpenseViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Изберете автомобил.")]
        public int CarId { get; set; }
        [Required(ErrorMessage = "Въведете дата.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(RepairExpenseViewModel), nameof(ValidateRepairDate))]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Въведете описание.")]
        [StringLength(200, ErrorMessage = "Описанието е задължително.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Въведете сума.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумата трябва да е положително число.")]
        public decimal Cost { get; set; }
        [BindNever]
        public string CarDisplayName { get; set; }
        [BindNever]
        public string Brand { get; set; }
        [BindNever]
        public string FuelType { get; set; }

        public static ValidationResult? ValidateRepairDate(DateTime date, ValidationContext context)
        {
            if (date > DateTime.Now)
            {
                return new ValidationResult("Не можете да въведете ремонт за бъдеща дата.");
            }
            return ValidationResult.Success;
        }
    }
}
