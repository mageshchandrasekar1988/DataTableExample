using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmtityDBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq.Dynamic;
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
        public JsonResult GetData1() 
        {
            List<Location> locations = context.Location.ToList();
            return Json(new { data = locations }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpGet]
        public JsonResult GetData()
        {
            //Get parameters

            //Get start (paging start index) and length (page size for paging)

            int draw = Convert.ToInt32(Request.Query["draw"]);
            int start = Convert.ToInt32(Request.Query["start"]); 
            int length = Convert.ToInt32(Request.Query["length"]);

            //Get sort columns value 
            
            int sortColumnIdx = Convert.ToInt32(Request.Query["order[0][column]"]);
            string sortColumnName = Request.Query["columns[" + sortColumnIdx + "][name]"];
            //    Request.Form["columns[" + order + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Query["order[0][dir]"];

            string searchValue = Request.Query["search[value]"].FirstOrDefault()?.Trim();
            // Records Count matching search criteria
            int recordFilteredCount = context.Location.Where(a => a.LocationName.Contains(searchValue)).Count();
            // Total Records Count
            int recordsTotalCount = context.Location.Count();
            //Filter & sortred & paged data to  be sent to form server to view
            List<Location> filteredData = null;
            if(sortColumnDir == "asc")
            {
                filteredData = context.Location
                    .Where(a => a.LocationName.Contains(searchValue))
                    .OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                    .Skip(start)
                    .Take(length)
                    .ToList<Location>();
            }
            else
            {
                filteredData = context.Location.ToList();
                /*filteredData = context.Location
                   .Where(a => a.LocationName.Contains(searchValue))
                   .OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x))
                   .Skip(start)
                   .Take(length)
                   .ToList<Location>();*/
            }

            return Json(new
            {
                data = filteredData,
                draw = Request.Query["draw"],
                recordsFiltered = recordFilteredCount,
                recordsTotal = recordsTotalCount
            });
            /*int pageSize = 0;//length != null ? length : 0;
            int skip = 0;//start != null ? start : 0;
            int totalRecord = 0;

            var v = (from a in context.Location select a);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {
                v = v.OrderBy(sortColumn + " " + sortColumnDir);
            }
            totalRecord = v.Count();
            var data = v.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = totalRecord, recordsTotal = totalRecord, data = data }, new Newtonsoft.Json.JsonSerializerSettings());
            */
            //List<Location> locations = context.Location.ToList();
            // return Json(new { data = locations }, new Newtonsoft.Json.JsonSerializerSettings());
        }
        public IActionResult Datatable()
        {
            return View();
        }
    }
}