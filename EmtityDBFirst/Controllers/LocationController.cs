using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmtityDBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EmtityDBFirst.Controllers
{
    public class LocationController : Controller
    {
        private readonly StoreContext context;

        public LocationController(StoreContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetData() 
        {
            List<Location> locations = context.Location.ToList();
            return Json(new { data = locations }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        public IActionResult Datatable()
        {
            return View();
        }
    }
}