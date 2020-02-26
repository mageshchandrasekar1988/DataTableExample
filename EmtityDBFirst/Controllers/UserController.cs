using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using EmtityDBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace EmtityDBFirst.Controllers
{
    public class UserController : Controller
    {
        private readonly StoreContext _context;

        public UserController(StoreContext context)
        {
            _context = context;
        }
       
        public IActionResult Index()
        {
            List<User> users = _context.User.ToList();
            return View(users);
        }
        [HttpGet]
        public ViewResult Create()
        {
           return View();
        }

        [HttpPost]
        public IActionResult Create(User model)
        {
            if (ModelState.IsValid)
            {
                List<User> duplicateCheck = _context.User.ToList();
                var res = from item in duplicateCheck where item.FirstName == model.FirstName & item.LastName == model.LastName select item;
                if(res.Count() == 0)
                {
                    User user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName
                    };
                    _context.Add(user);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            
                User user = _context.User.Find(id);
                User newUser = new User
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
                return View(newUser);
            
            
        }
        [HttpPost]
        public IActionResult Edit(User model)
        {
            if (ModelState.IsValid)
            {
                List<User> duplicateCheck = _context.User.ToList();
                var check = from item in duplicateCheck
                            where model.UserId != item.UserId & 
                            model.FirstName == item.FirstName & 
                            model.LastName == item.LastName
                            select item;
                if (check.Count() == 0)
                {
                    User user = _context.User.Find(model.UserId);
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;

                    _context.Update(user);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        [ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            User user = _context.User.Find(id);
            _context.User.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
       [HttpPost]
        public IActionResult Testing(int id)
        {
            User user = _context.User.Find(id);
            _context.User.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult GetData()
        {
            List<User> users = _context.User.ToList();
            return Json(new { data = users });
        }

        public IActionResult ExcelDownload()
        {
            //step1: create array to holder header labels
            string[] col_name = new string[]
            {
                "First Name",
                "Last Name"
            };
            //step2: create result byte array
            byte[] result;
            //step3: create a new package using memory safe structure
            using (var package = new ExcelPackage())
            {
                //step4: create a new worksheet
                var workSheet = package.Workbook.Worksheets.Add("Testing Sheet");
                //step5: fill in header row
                //worksheet.Cells[row,col].  {Style, Value}
                for (int i=0; i<col_name.Length; i++)
                {
                    workSheet.Cells[1, i + 1].Style.Font.Size = 14;
                    workSheet.Cells[1, i + 1].Value = col_name[i];
                    workSheet.Cells[1, i + 1].Style.Font.Bold = true;
                    //border the cell
                    workSheet.Cells[1, i + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    //set background color for each sell
                    workSheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 243, 214));

                }
                int row = 2;
                //step6: loop through query result and fill in cells
                foreach (var item in _context.User.ToList())
                {
                    for (int col = 0; col <= 2; col++)
                    {
                        workSheet.Cells[row, 1].Style.Font.Size = 14;
                        workSheet.Cells[row, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }
                    workSheet.Cells[row, 1].Value = item.FirstName;
                    workSheet.Cells[row, 2].Value = item.LastName;

                    //toggle background color
                    //even row with ribbon style
                    if (row % 2 == 0)
                    {
                        workSheet.Cells[row, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[row, 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(154, 211, 157));

                        workSheet.Cells[row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        workSheet.Cells[row, 2].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(154, 211, 157));

                    }
                    row++;
                }
                //step7: auto fit columns
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                //step8: convert the package as byte array
                result = package.GetAsByteArray();

            }
            //step9: return byte array as a file
            return File(result, "application/vnd.ms-excel", "test.xls");
        }
    }
}