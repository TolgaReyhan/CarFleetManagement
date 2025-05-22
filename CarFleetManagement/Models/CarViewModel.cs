using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CarFleetManagement.Models
{
    public class CarViewModel
    {
        public int Id { get; set; }
        public string CarModel { get; set; }
        [Display(Name = "Регистрационен номер")]
        public string RegistrationNumber { get; set; }
        [Display(Name = "Дата на закупуване")]
        public DateTime PurchaseDate { get; set; }
        [Display(Name = "Пробег (км)")]
        public int Mileage { get; set; }
        [Display(Name = "Технически преглед до")]
        [DataType(DataType.Date)]
        public DateTime? TechnicalCheckDate { get; set; }

        [Display(Name = "Следваща смяна на масло")]
        [DataType(DataType.Date)]
        public DateTime? NextOilChangeDate { get; set; }

        [Display(Name = "Марка")]
        public string Brand { get; set; }
        [Display(Name = "Вид гориво")]
        public string FuelType { get; set; }
    }
}
