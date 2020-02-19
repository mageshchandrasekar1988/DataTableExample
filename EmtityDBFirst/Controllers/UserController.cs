using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmtityDBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}