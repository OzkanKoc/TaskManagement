using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProjectTaskManagement.Filters;
using ProjectTaskManagement.Models.Context;
using ProjectTaskManagement.Models.DataAccess;
using ProjectTaskManagement.Models.DataAccess.Entity;

namespace ProjectTaskManagement.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private TaskManagerDbContext db = new TaskManagerDbContext();

        // GET: Project
        public ActionResult Index()
        {
            var project = db.Project.Include(p => p.Customer);
            return View(project.ToList());
        }

        [AuthorizedRole(RoleType.ProjectManager, RoleType.TeamLeader, RoleType.BusinessAnalyst)]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Project.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [AuthorizedRole(RoleType.ProjectManager)]
        // GET: Project/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customer, "CustomerId", "CompanyName");
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "ProjectId,CustomerId,Name,Description,PlannedStartingDate,PlannedEndingDate,StartingDate,EndingDate")]
            Project project)
        {
            if (ModelState.IsValid)
            {
                db.Project.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customer, "CustomerId", "CompanyName", project.CustomerId);
            return View(project);
        }

        [AuthorizedRole(RoleType.ProjectManager)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Project.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customer, "CustomerId", "CompanyName", project.CustomerId);
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizedRole(RoleType.ProjectManager)]
        public ActionResult Edit([Bind(Include = "ProjectId,CustomerId,Name,Description,PlannedStartingDate,PlannedEndingDate,StartingDate,EndingDate")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customer, "CustomerId", "CompanyName", project.CustomerId);
            return View(project);
        }

        [AuthorizedRole(RoleType.ProjectManager)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Project.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizedRole(RoleType.ProjectManager)]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Project.Find(id);
            db.Project.Remove(project);
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
    }
}
