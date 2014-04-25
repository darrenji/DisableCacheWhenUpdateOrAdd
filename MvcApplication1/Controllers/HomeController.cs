using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using MvcApplication1.Repository;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IVehicleRepository Repository { get; set; }

        public HomeController(IVehicleRepository repository)
        {
            this.Repository = repository;
        }

        public HomeController() : this(new VehicleRepository())
        {
            
        }
        public ActionResult Index()
        {
            return View(Repository.GetVehicles());
        }

        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            Repository.ClearCache();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var vehicle = Repository.GetVehicles().Single(v => v.Id == id);
            return View(vehicle);
        }

        [HttpPost]
        public ActionResult Edit(Vehicle vehicle)
        {
            Repository.Update(vehicle);
            Repository.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {
            return View(new Vehicle());
        }

        [HttpPost]
        public ActionResult Create(Vehicle vehicle)
        {
            Repository.Insert(vehicle);
            Repository.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
