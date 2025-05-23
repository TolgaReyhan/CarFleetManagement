﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CarFleetManagement.Models
{
    public class InsuranceExpenseViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Изберете автомобил.")]
        public int CarId { get; set; }
        [BindNever]
        public string CarDisplayName { get; set; }
        [BindNever]
        public string Brand { get; set; }
        [BindNever]
        public string FuelType { get; set; }
        [Required(ErrorMessage = "Въведете компания.")]
        public string Provider { get; set; }
        [Required(ErrorMessage = "Въведете сума.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумата трябва да е положително число.")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Въведете начална дата.")]
        [DataType(DataType.Date)]
        [Display(Name = "Начална дата")]
        [CustomValidation(typeof(InsuranceExpenseViewModel), nameof(ValidateStartDate))]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Въведете крайна дата.")]
        [DataType(DataType.Date)]
        [Display(Name = "Крайна дата")]
        [CustomValidation(typeof(InsuranceExpenseViewModel), nameof(ValidateEndDate))]
        public DateTime EndDate { get; set; }

        public static ValidationResult? ValidateStartDate(DateTime startDate, ValidationContext context)
        {
            if (startDate > DateTime.Now.AddDays(30)) // Ограничаваме да не е твърде далеч в бъдещето
            {
                return new ValidationResult("Началната дата не може да бъде твърде далеч в бъдещето.");
            }

            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateEndDate(DateTime endDate, ValidationContext context)
        {
            var instance = (InsuranceExpenseViewModel)context.ObjectInstance;

            if (endDate <= instance.StartDate)
            {
                return new ValidationResult("Крайната дата трябва да е след началната дата.");
            }

            return ValidationResult.Success;
        }
    }
}
