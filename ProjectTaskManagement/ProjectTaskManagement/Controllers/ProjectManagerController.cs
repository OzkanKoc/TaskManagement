using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProjectTaskManagement.Filters;
using ProjectTaskManagement.Identity;
using ProjectTaskManagement.Models.Context;
using ProjectTaskManagement.Models.DataAccess;

namespace ProjectTaskManagement.Controllers
{
    [AuthorizedRole(RoleType.ProjectManager)]
    public class ProjectManagerController : Controller
    {
        private TaskManagerDbContext db = new TaskManagerDbContext();
        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;

        public ProjectManagerController()
        {
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
            userManager = new UserManager<ApplicationUser>(userStore);

            RoleStore<ApplicationRole> roleStore = new RoleStore<ApplicationRole>(db);
            roleManager = new RoleManager<ApplicationRole>(roleStore);
        }

        //// GET: ProjectManager
        //public ActionResult Index()
        //{
        //    var employees = db.Employees.Include(p => p.User);
        //    return View(employees.ToList());
        //}

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(ApplicationUser user)
        {
            var identityResult = userManager.Create(user, user.PasswordHash);

            if (identityResult.Succeeded)
            {
                userManager.AddToRole(user.Id, user.RoleType.ToString());
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("RegisterUser", "Kullanıcı ekleme işleminde hata!");
            }

            return View(user);
        }

        // GET: ProjectManager/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProjectManager projectManager = db.Employee.Find(id);
        //    if (projectManager == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(projectManager);
        //}

        // GET: ProjectManager/Create
        //public ActionResult Create()
        //{
        //    ViewBag.EmployeeId = new SelectList(db.ApplicationUsers, "Id", "Name");
        //    return View();
        //}

        // POST: ProjectManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "EmployeeId,FirstName,LastName,IsFirstLogin,Email,Address,IdentityNumber,DateOfBirth,HiringDate")] ProjectManager projectManager)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Employees.Add(projectManager);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.EmployeeId = new SelectList(db.ApplicationUsers, "Id", "Name", projectManager.EmployeeId);
        //    return View(projectManager);
        //}

        // GET: ProjectManager/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProjectManager projectManager = db.Employees.Find(id);
        //    if (projectManager == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.EmployeeId = new SelectList(db.ApplicationUsers, "Id", "Name", projectManager.EmployeeId);
        //    return View(projectManager);
        //}

        // POST: ProjectManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "EmployeeId,FirstName,LastName,IsFirstLogin,Email,Address,IdentityNumber,DateOfBirth,HiringDate")] ProjectManager projectManager)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(projectManager).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.EmployeeId = new SelectList(db.ApplicationUsers, "Id", "Name", projectManager.EmployeeId);
        //    return View(projectManager);
        //}

        // GET: ProjectManager/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProjectManager projectManager = db.Employees.Find(id);
        //    if (projectManager == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(projectManager);
        //}

        // POST: ProjectManager/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    ProjectManager projectManager = db.Employees.Find(id);
        //    db.Employees.Remove(projectManager);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
