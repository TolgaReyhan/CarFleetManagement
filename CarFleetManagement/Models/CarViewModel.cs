using System.ComponentModel.DataAnnotations;

namespace CarFleetManagement.Models
{
    public class CarViewModel
    {
        public int Id { get; set; }
        public string Model { get; set; }
        [Display(Name = "Регистрационен номер")]
        public string RegistrationNumber { get; set; }
        [Display(Name = "Дата на закупуване")]
        public DateTime PurchaseDate { get; set; }
        [Display(Name = "Пробег (км)")]
        public int Mileage { get; set; }
    }
}
