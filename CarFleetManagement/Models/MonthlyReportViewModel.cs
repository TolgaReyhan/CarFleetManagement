namespace CarFleetManagement.Models
{
    public class MonthlyReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string CarModel { get; set; }

        public double TotalFuelLiters { get; set; }
        public decimal TotalFuelAmount { get; set; }


        public int RepairCount { get; set; }
        public decimal TotalRepairCost { get; set; }

        public int InsuranceCount { get; set; }
        public decimal TotalInsuranceCost { get; set; }
        public string CarDisplayName { get; set; }
    }
}
