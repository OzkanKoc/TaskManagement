using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using ProjectTaskManagement.Filters;
using ProjectTaskManagement.Identity;
using ProjectTaskManagement.Models;
using ProjectTaskManagement.Models.Context;
using ProjectTaskManagement.Models.DataAccess;
using ProjectTaskManagement.Models.DataAccess.Entity;

namespace ProjectTaskManagement.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private TaskManagerDbContext db = new TaskManagerDbContext();

        // GET: Tasks
        public ActionResult Index()
        {
            var tasks = db.Task
                .Include(t => t.Project)
                .Include(t => t.Employee)
                .Include(t => t.Employee.User)
                .Where(t => t.Project.IsActive == true).ToList();

            var emp = db.Employee.Include("User").First(e => e.User.UserName == HttpContext.User.Identity.Name);
            ViewBag.UserFirstName = emp.FirstName;
            ViewBag.CurrentRole = GetCurrentRole();

            return View(tasks);
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Task.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        [AuthorizedRole(RoleType.TeamLeader, RoleType.BusinessAnalyst)]
        public ActionResult Create(int demandId = -1)
        {
            ViewBag.ProjectId = new SelectList(db.Project, "ProjectId", "Name");

            List<Employee> employees = new List<Employee>();
            var roles = new List<string>();
            foreach (var item in db.Employee.Include("User").ToList())
            {
                if (!(item is TeamLeader || item is ProjectManager))
                {
                    employees.Add(item);
                    roles.Add(item.User.RoleType.ToString());
                }
            }
            ViewBag.EmployeeRoles = roles;
            ViewBag.EmployeeList = employees;
            ViewBag.Employees = new SelectList(employees, "EmployeeId", "FirstName");

            var customerDemand = db.CustomerDemand.Find(demandId);
            if (customerDemand == null)
            {
                return View();
            }

            Task task = new Task()
            {
                ProjectId = customerDemand.ProjectId,
                Description = customerDemand.Demand,
                DemandId = demandId,
            };

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizedRole(RoleType.TeamLeader, RoleType.BusinessAnalyst)]
        public ActionResult Create(Task task)
        {
            if (ModelState.IsValid)
            {
                task = CheckDemendIsExist(task);

                if (!IsProjectActive(ref task))
                {
                    task.Project.IsActive = true;
                }

                db.Task.Add(task);
                db.SaveChanges();

                UpdateProjectEndingDate(task);

                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Project, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [AuthorizedRole(RoleType.TeamLeader)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Task.Include("Employee").FirstOrDefault(t => t.TaskId == id);
            if (task == null)
            {
                return HttpNotFound();
            }
            List<Employee> employees = new List<Employee>();
            var roles = new List<string>();
            foreach (var item in db.Employee.Include("User").ToList())
            {
                if (!(item is TeamLeader || item is ProjectManager))
                {
                    employees.Add(item);
                    roles.Add(item.User.RoleType.ToString());
                }
            }
            ViewBag.EmployeeRoles = roles;
            ViewBag.EmployeeList = employees;
            ViewBag.ProjectId = new SelectList(db.Project, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeId,TaskId,ProjectId,Name,Description,CreationDate,StartingDate,EndingDate,Status")] Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Project, "ProjectId", "Name", task.ProjectId);
            return View(task);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Task.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizedRole(RoleType.TeamLeader)]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Task.Find(id);
            db.Task.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AssingToEmployee(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var task = db.Task.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            List<Employee> employees = new List<Employee>();
            var roles = new List<string>();
            foreach (var item in db.Employee.Include("User").ToList())
            {
                if (!(item is TeamLeader || item is ProjectManager))
                {
                    employees.Add(item);
                    roles.Add(item.User.RoleType.ToString());
                }
            }
            ViewBag.EmployeeRoles = roles;
            ViewBag.EmployeeList = employees;

            return View(task);
        }

        [HttpPost]
        public ActionResult AssingToEmployee(Task task)
        {
            if (task == null)
            {
                return HttpNotFound();
            }
            var willEditTask = db.Task.Find(task.TaskId);
            willEditTask.EmployeeId = task.EmployeeId;
            db.Entry(willEditTask).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult BeginTheTask(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var task = db.Task.Include("Project").FirstOrDefault(t => t.TaskId == id);
            if (task == null)
            {
                return HttpNotFound();
            }
            if (task.StartingDate == null)
            {
                task.StartingDate = DateTime.Now;
            }
            task.Status = TaskStatus.Started;

            if (task.IsFirstTask(id))
            {
                task.Project.StartingDate = task.StartingDate;
            }
            //TempData["IsStarted"] = true;
            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
            CreateTaskAssigmentHistory(task);

            return RedirectToAction("Index");
        }

        public ActionResult EndTheTask(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var task = db.Task.Include("Project").FirstOrDefault(t => t.TaskId == id);
            if (task == null)
            {
                return HttpNotFound();
            }

            if (GetCurrentRole() == RoleType.BusinessAnalyst.ToString())
            {
                task.EndingDate = DateTime.Now;
                var isLastTaskOfProject = task.IsLastTaskCompleted(task.TaskId);

                if (isLastTaskOfProject)
                {
                    task.Project.EndingDate = task.EndingDate;
                }
                db.Entry(task).State = EntityState.Modified;
            }
            else
            {
                task.EmployeeId = null;
                db.Entry(task).State = EntityState.Modified;
            }
            task = UpdateTaskStatus(task);
            UpdateTaskAssigmentHistory(task);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [AuthorizedRole(RoleType.TestSpecialist)]
        public ActionResult BackTheTask(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var task = db.Task.Include("Project").FirstOrDefault(t => t.TaskId == id);
            if (task == null)
            {
                return HttpNotFound();
            }

            task.EmployeeId = null;
            db.Entry(task).State = EntityState.Modified;

            task = UpdateTaskStatus(task, true);
            UpdateTaskAssigmentHistory(task);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        [AuthorizedRole(RoleType.BusinessAnalyst, RoleType.TeamLeader)]
        public ActionResult EditStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var task = db.Task.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task); ;
        }

        [HttpPost]
        [AuthorizedRole(RoleType.BusinessAnalyst, RoleType.TeamLeader)]
        public ActionResult EditStatus(Task task)
        {
            if (task == null)
            {
                return HttpNotFound();
            }
            var taskFromDb = db.Task.Find(task.TaskId);
            taskFromDb.Status = task.Status;
            db.Entry(taskFromDb).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult TakeTheTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var task = db.Task.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            var userId = GetCurrentUserId(User.Identity as ClaimsIdentity);

            if (IsAvailableForAssigment(task))
            {
                //ViewBag.IsStarted = false;
                task.EmployeeId = userId;
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        private bool IsAvailableForAssigment(Task task)
        {
            var userId = GetCurrentUserId(User.Identity as ClaimsIdentity);
            if (userId == null)
            {
                return false;
            }
            TaskStatus taskStatus = TaskStatus.None;
            string role = GetCurrentRole();

            switch (role)
            {
                case "SoftwareDeveloper":
                    taskStatus = TaskStatus.WillBeDeveloped;
                    break;
                case "BusinessAnalyst":
                    taskStatus = TaskStatus.WillBeAnalized;
                    break;
                case "TestSpecialist":
                    taskStatus = TaskStatus.WillBeTested;
                    break;
                default:
                    break;
            }

            if (task.Status != taskStatus)
            {
                return false;
            }

            return true;
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

        private string GetCurrentUserId(ClaimsIdentity claimsIdentity)
        {
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    return userIdClaim.Value;
                }
            }

            return null;
        }

        private void UpdateProjectEndingDate(Task task)
        {
            var isLastTask = task.IsLastTaskCompleted(task.TaskId);
            if (isLastTask)
            {
                var project = db.Project.Find(task.ProjectId);
                project.EndingDate = null;
                project.IsActive = true;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        private Task UpdateTaskStatus(Task task, bool willBack = false)
        {
            var userRole = GetCurrentRole();
            switch (userRole)
            {
                case "SoftwareDeveloper":
                    task.Status = TaskStatus.WillBeTested;
                    break;
                case "TestSpecialist":
                    task.Status = !willBack ? TaskStatus.WillBeAnalized : TaskStatus.WillBeDeveloped;
                    break;
                case "BusinessAnalyst":
                    task.Status = TaskStatus.Completed;
                    break;
                default:
                    break;
            }

            return task;
        }

        private void CreateTaskAssigmentHistory(Task task)
        {
            var taskAssigmentHistory = new TaskAssigmentHistory()
            {
                StartedDate = DateTime.Now,
                TaskId = task.TaskId,
                UserName = HttpContext.User.Identity.Name
            };
            db.Entry(taskAssigmentHistory).State = EntityState.Added;
            db.SaveChanges();
        }

        private void UpdateTaskAssigmentHistory(Task task)
        {
            var userAssigmentHistory = (from history in db.TaskAssigmentHistory
                                        where history.TaskId == task.TaskId
                                             && history.UserName == HttpContext.User.Identity.Name
                                        select history).FirstOrDefault();

            userAssigmentHistory.EndedDate = DateTime.Now;
            db.Entry(userAssigmentHistory).State = EntityState.Modified;
            db.SaveChanges();
        }

        private bool IsProjectActive(ref Task task)
        {
            task.Project = db.Project.Find(task.ProjectId);
            if (!task.Project.IsActive)
            {
                return false;
            }
            task.Project = null;

            return true;
        }

        private Task CheckDemendIsExist(Task task)
        {
            if (task.DemandId != null)
            {
                var demend = db.CustomerDemand.Find(task.DemandId);
                if (demend != null)
                {
                    demend.IsActive = false;
                    db.Entry(demend).State = EntityState.Modified;
                }
            }

            return task;
        }
    }
}
