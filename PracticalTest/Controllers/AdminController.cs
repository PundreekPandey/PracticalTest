using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using PracticalTest.Models;
using PracticalTest.Controllers;
using System.IO;

namespace PracticalTest.Controllers
{
    public class AdminController : Controller
    {
        PracticeTestEntities db = new PracticeTestEntities();
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(Login model)
        {
            if (ModelState.IsValid)
            {
                string userId = model.UserId;
                string password = model.Password;

                using (PracticeTestEntities db = new PracticeTestEntities())
                {
                    Login obj = db.Logins.Where(u => u.UserId == userId && u.Password == password).FirstOrDefault();

                    if (obj != null)
                    {
                        return RedirectToAction("EmployeeList", "Admin");
                    }
                    else
                    {
                        ViewBag.Error = "Login failed";
                        return View();
                    }
                }
            }
            return View();
        }

        public ActionResult EmployeeList()
        {
            var employees = db.EmployeeMasters.ToList();

            return View(employees);
        }
        public ActionResult AddEdit(int? id)
        {          
            var model = id.HasValue ? db.EmployeeMasters.Find(id) : new EmployeeMaster();
            return View(model);
        }
        [HttpPost]
        public ActionResult Save(EmployeeMaster employee, HttpPostedFileBase PhotoFile)
        {
            if (ModelState.IsValid)
            {
                if (employee.EmployeeId == 0)
                {
                    employee.EmployeeId = GetNextEmployeeId();
                    db.EmployeeMasters.Add(employee);
                }
                else
                {
                    var existingEmployee = db.EmployeeMasters.Find(employee.EmployeeId);
                    if (existingEmployee != null)
                    {
                        existingEmployee.FirstName = employee.FirstName;
                        existingEmployee.LastName = employee.LastName;
                        existingEmployee.DesignationId = employee.DesignationId;
                        existingEmployee.DateOfJoining = employee.DateOfJoining;
                        existingEmployee.Salary = employee.Salary;
                        existingEmployee.PhotoPath = employee.PhotoPath;
                    }
                }
                if (PhotoFile != null && PhotoFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(PhotoFile.FileName);
                    // Construct the full server path to where you want to save the file
                    string imagePath = Path.Combine(Server.MapPath("~/Content/Image"), fileName);
                    string imagePath1 = "~/Content/Image" + fileName;

                    // Save the file to the constructed path
                    PhotoFile.SaveAs(imagePath);
                    ViewBag.photo = imagePath1;
                    // Update employee's photo path in the database

                    //employee.PhotoPath = "~/Content/Image/" + fileName;
                    employee.PhotoPath = @"C:\Users\os5\source\repos\PracticalTest\PracticalTest\Content\Image\" + fileName;
                }


                db.SaveChanges();
                return RedirectToAction("EmployeeList");
            }
            return View("AddEdit", employee);
        }

        private int GetNextEmployeeId()
        {
            // Find the maximum existing EmployeeId in the database
            int maxEmployeeId = db.EmployeeMasters.Max(e => (int?)e.EmployeeId) ?? 0;

            // Increment to get the next available EmployeeId
            return maxEmployeeId + 1;
        }



        public ActionResult Delete(int id)
        {
            var employee = db.EmployeeMasters.Find(id);
            if (employee != null)
            {
                // Delete associated photo file
                if (!string.IsNullOrEmpty(employee.PhotoPath))
                {
                    string filePath = Server.MapPath(employee.PhotoPath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                db.EmployeeMasters.Remove(employee);
                db.SaveChanges();
            }
            return RedirectToAction("EmployeeList");
        }
    }
}
