using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarFleetManagement.Data.Models
{
    public class MonthlyReport
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal FuelLiters { get; set; } // Изразходвано гориво (литри)
        public decimal FuelCost { get; set; } // Стойност на горивото
        public int TotalKilometers { get; set; } // Изминати километри
        public int TotalRepairs { get; set; } // Общ брой ремонти
        public decimal RepairCost { get; set; } // Обща стойност на ремонтите
        public int InsuranceCount { get; set; } // Брой застраховки
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public Car Car { get; set; }
    }
}
