using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json.Linq;
using ProjectTaskManagement.Filters;
using ProjectTaskManagement.Models.Context;
using ProjectTaskManagement.Models.DataAccess;
using ProjectTaskManagement.Models.DataAccess.Entity;

namespace ProjectTaskManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private TaskManagerDbContext db = new TaskManagerDbContext();

        // GET: Employee
        public ActionResult Index()
        {
            var employee = db.Employee.Include(e => e.User);
            return View(employee.ToList());
        }

        // GET: Employee/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        //GET: Employee/Create
        public ActionResult Create(string EmployeeId)
        {
            return View(new Employee { EmployeeId = EmployeeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,FirstName,LastName,Email,Address,IdentityNumber,DateOfBirth,HiringDate")]
                                    Employee employee)
        {
            if (ModelState.IsValid)
            {
                var typeOfUser = GetUserType();

                var obj = Activator.CreateInstance(typeOfUser);

                FillEmployeeProperties(obj as Employee, employee);

                if (obj != null)
                {
                    db.Entry(obj).State = EntityState.Added;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        //GET: Employee/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Include("User").FirstOrDefault(e => e.EmployeeId == id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,FirstName,LastName,Email,Address,IdentityNumber,DateOfBirth,HiringDate")]
            Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        [AuthorizedRole(RoleType.ProjectManager)]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employee.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        [AuthorizedRole(RoleType.ProjectManager)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employee.Find(id);
            db.Employee.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Private Parts
        private Type GetUserType()
        {
            string role = GetCurrentRole();

            if (role != null)
            {
                foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    if (role == type.Name)
                    {
                        Type t = Type.GetType(type.FullName);
                        return t;
                    }
                }
            }

            return null;
        }

        private string GetCurrentRole()
        {
            var roles = Enum.GetNames(typeof(RoleType));

            for (int i = 0; i < roles.Length; i++)
            {
                if (HttpContext.User.IsInRole(roles[i]))
                {
                    return roles[i];
                }
            }

            return null;
        }

        private void FillEmployeeProperties(Employee newEmployee, Employee oldEmployee)
        {
            if (newEmployee != null && oldEmployee != null)
            {
                newEmployee.Address = oldEmployee.Address;
                newEmployee.DateOfBirth = oldEmployee.DateOfBirth;
                newEmployee.Email = oldEmployee.Email;
                newEmployee.EmployeeId = oldEmployee.EmployeeId;
                newEmployee.FirstName = oldEmployee.FirstName;
                newEmployee.HiringDate = oldEmployee.HiringDate;
                newEmployee.IdentityNumber = oldEmployee.IdentityNumber;
                newEmployee.LastName = oldEmployee.LastName;
                newEmployee.Tasks = oldEmployee.Tasks;
                newEmployee.User = oldEmployee.User;
            }
        }
    }
}
