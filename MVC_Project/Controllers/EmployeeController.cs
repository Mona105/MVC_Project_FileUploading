using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC_Project.Controllers;
using MVC_Project.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MVC_Project.Controllers
{
    public class EmployeeController : Controller
    {
        private ApplicationDbContext dbContext;
        private IHostingEnvironment Environment;
        public EmployeeController(ApplicationDbContext dbContext, 
            IHostingEnvironment environment)
        {
            this.dbContext = dbContext;
            Environment = environment;
        }
        public IActionResult Index()
        {
           List<Employee> employees = dbContext.Employees.ToList();
            return View(employees);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee v)
        {
            string name = Request.Form["Name"];
            string email = Request.Form["Email"];
            string gender = Request.Form["Gender"];
            string mobile = Request.Form["Mobile"];
            var file = Request.Form.Files;
            string dbpath = string.Empty;
            if(file.Count>0)
            {
                var files = file[0];
                string path = Environment.WebRootPath;
                string fullPath = Path.Combine(path, "Image", files.FileName);
                FileStream stream = new FileStream(fullPath,FileMode.Create);
                files.CopyTo(stream);
                dbpath = files.FileName;
            }
            var emp = new Employee()
            {
                Name = name,
                Email = email,
                Gender = gender,
                Mobile = mobile,
                Image = dbpath
            };
            dbContext.Employees.Add(emp);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
