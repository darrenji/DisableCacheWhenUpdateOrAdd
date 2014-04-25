using System.Collections.Generic;
using MvcApplication1.Models;

namespace MvcApplication1.Repository
{
    public interface IVehicleRepository
    {
        void ClearCache();
        IEnumerable<Vehicle> GetVehicles();
        void Insert(Vehicle vehicle); 
        void Update(Vehicle vehicle);
        void SaveChanges();
    }
}