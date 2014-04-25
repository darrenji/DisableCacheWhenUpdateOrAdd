using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using MvcApplication1.Cache;
using MvcApplication1.Models;
namespace MvcApplication1.Repository
{
    public class VehicleRepository : IVehicleRepository
    {
        protected DemoEntities DataContext { get; private set; }
        public ICacheProvider Cache { get; set; }

        public VehicleRepository() : this(new DefaultCacheProvider())
        {
            
        }

        public VehicleRepository(ICacheProvider cacheProvider)
        {
            this.DataContext = new DemoEntities();
            this.Cache = cacheProvider;
        }

        public void ClearCache()
        {
            Cache.Invalidate("vehicles");
        }

        public System.Collections.Generic.IEnumerable<Models.Vehicle> GetVehicles()
        {
            #region 第一次写法
            //IEnumerable<Vehicle> vehicles = Cache.Get("vehicles") as IEnumerable<Vehicle>;
            //if (vehicles == null)
            //{
            //    vehicles = DataContext.Vehicle.OrderBy(v => v.Id).ToList();
            //    if (vehicles.Any())
            //    {
            //        Cache.Set("vehicles", vehicles, 30);
            //    }
            //}
            //return vehicles; 
            #endregion

            var vehicles = Cache.Get("vehicles") as IDictionary<int, Vehicle>;
            if (vehicles == null)
            {
                vehicles = DataContext.Vehicle.ToDictionary(v => v.Id);
                if (vehicles.Any())
                {
                    Cache.Set("vehicles",vehicles,30);
                }
            }
            return vehicles.Values;
        }

        public void Update(Vehicle vehicle)
        {
            if (vehicle != null)
            {
                DataContext.Set<Vehicle>().Attach(vehicle);
                DataContext.Entry(vehicle).State = EntityState.Modified;
            }
        }

        public void Insert(Vehicle vehicle)
        {
            DataContext.Set<Vehicle>().Add(vehicle);
        }

        public void SaveChanges()
        {
            //获取上下文中EntityState状态为added或modified的Vehicle
             var changeobjects = DataContext.ChangeTracker.Entries<Vehicle>();

            //把变化保存到数据库
            DataContext.SaveChanges();

            //更新缓存中相关的Vehicle
            var cacheData = Cache.Get("vehicles") as Dictionary<int, Vehicle>;
            if (cacheData != null)
            {
                foreach (var item in changeobjects)
                {
                    var vehicle = item.Entity as Vehicle;
                    cacheData[vehicle.Id] = vehicle;
                }
            }
        }
    }
}