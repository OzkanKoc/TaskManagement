using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProjectTaskManagement.Filters;
using ProjectTaskManagement.Models.Context;
using ProjectTaskManagement.Models.DataAccess;
using ProjectTaskManagement.Models.DataAccess.Entity;

namespace ProjectTaskManagement.Controllers
{
    [Authorize]
    public class CustomerDemandController : Controller
    {
        private TaskManagerDbContext db = new TaskManagerDbContext();

        [AuthorizedRole(RoleType.BusinessAnalyst, RoleType.TeamLeader, RoleType.ProjectManager)]
        public ActionResult Index()
        {
            var customerDemand = db.CustomerDemand
                .Include(c => c.Project)
                .Where(c => c.IsActive == true);
            return View(customerDemand.ToList());
        }

        [AuthorizedRole(RoleType.BusinessAnalyst, RoleType.TeamLeader, RoleType.ProjectManager)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerDemand customerDemand = db.CustomerDemand.Find(id);
            if (customerDemand == null)
            {
                return HttpNotFound();
            }
            return View(customerDemand);
        }

        [AuthorizedRole(RoleType.TeamLeader, RoleType.BusinessAnalyst)]
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(db.Project, "ProjectId", "Name");
            return View();
        }

        [AuthorizedRole(RoleType.TeamLeader, RoleType.BusinessAnalyst)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,CustomerDemandId,CustomerId,Demand,DemandType")]
                CustomerDemand customerDemand)
        {
            if (ModelState.IsValid)
            {
                db.CustomerDemand.Add(customerDemand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Project, "ProjectId", "Name", customerDemand.ProjectId);
            return View(customerDemand);
        }

        [AuthorizedRole(RoleType.BusinessAnalyst, RoleType.TeamLeader)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerDemand customerDemand = db.CustomerDemand.Find(id);
            if (customerDemand == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Project, "ProjectId", "Name", customerDemand.ProjectId);
            return View(customerDemand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizedRole(RoleType.BusinessAnalyst, RoleType.TeamLeader)]
        public ActionResult Edit([Bind(Include = "CustomerDemandId,CustomerId,Demand,DemandType")]
        CustomerDemand customerDemand)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerDemand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Project, "ProjectId", "Name", customerDemand.ProjectId);
            return View(customerDemand);
        }

        [AuthorizedRole(RoleType.BusinessAnalyst, RoleType.TeamLeader)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerDemand customerDemand = db.CustomerDemand.Find(id);
            if (customerDemand == null)
            {
                return HttpNotFound();
            }
            return View(customerDemand);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizedRole(RoleType.BusinessAnalyst, RoleType.TeamLeader)]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerDemand customerDemand = db.CustomerDemand.Find(id);
            db.CustomerDemand.Remove(customerDemand);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [AuthorizedRole(RoleType.TeamLeader, RoleType.BusinessAnalyst)]
        public ActionResult CreateTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return RedirectToAction("Create", "Tasks", new { demandId = id });
        }

        [HttpGet]
        public JsonResult GetCurrentCustomerProjects(int id)
        {
            var customer = db.Customer.Include("Projects").FirstOrDefault(c => c.CustomerId == id);
            if (customer == null)
            {
                return null;
            }
            List<string> projects = new List<string>();
            foreach (var item in customer.Projects)
            {
                item.Customer = null;
                projects.Add(JsonConvert.SerializeObject(item));
            }

            return Json(projects, JsonRequestBehavior.AllowGet);
        }

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
