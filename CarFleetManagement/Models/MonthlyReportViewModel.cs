namespace CarFleetManagement.Models
{
    public class MonthlyReportViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CarId { get; set; }
        public string CarModel { get; set; }
        public decimal FuelLiters { get; set; }
        public decimal FuelCost { get; set; }
        public int TotalKilometers { get; set; }
        public int TotalRepairs { get; set; }
        public decimal RepairCost { get; set; }
        public int InsuranceCount { get; set; }
    }
}
