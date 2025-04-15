using CarFleetManagement.Contracts;
using CarFleetManagement.Data;

namespace CarFleetManagement.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext db;

        public CarService(ApplicationDbContext db)
        {
            this.db = db;
        }
        public Dictionary<int, string> GetCarNamesAndIds()
        {
            var result = db.Cars.ToDictionary(x => x.Id, x => x.Model);
            return result;
        }
    }
}
