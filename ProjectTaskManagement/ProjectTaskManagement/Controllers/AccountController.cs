using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using ProjectTaskManagement.Identity;
using ProjectTaskManagement.Models.Context;
using ProjectTaskManagement.Models.DataAccess.Entity;
using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ProjectTaskManagement.Controllers
{
    public class AccountController : Controller
    {
        private TaskManagerDbContext db = new TaskManagerDbContext();
        private UserManager<ApplicationUser> userManager;
        private RoleManager<ApplicationRole> roleManager;

        public AccountController()
        {
            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
            userManager = new UserManager<ApplicationUser>(userStore);

            RoleStore<ApplicationRole> roleStore = new RoleStore<ApplicationRole>(db);
            roleManager = new RoleManager<ApplicationRole>(roleStore);
        }
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = userManager.Find(model.UserName, model.PasswordHash);

                if (user != null)
                {
                    IAuthenticationManager autManager = HttpContext.GetOwinContext().Authentication;
                    ClaimsIdentity identity = userManager.CreateIdentity(user, "ApplicationCookie");
                    AuthenticationProperties authProps = new AuthenticationProperties();
                    autManager.SignIn(authProps, identity);

                    var emp = db.Employee.Find(user.Id);

                    if (emp != null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Create", "Employee", new { EmployeeId = user.Id });
                    }
                }
                else
                {
                    ModelState.AddModelError("LoginUser", "Böyle bir kullanıcı bulunamadı");
                }
            }
            return View("Login");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            IAuthenticationManager authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            TempData.Clear();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(ApplicationUser user)
        {
            user.RoleType = Models.DataAccess.RoleType.ProjectManager;

            UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

            RoleStore<ApplicationRole> roleStore = new RoleStore<ApplicationRole>(db);
            RoleManager<ApplicationRole> roleManager = new RoleManager<ApplicationRole>(roleStore);

            var identityResult = userManager.Create(user, user.PasswordHash);

            if (identityResult.Succeeded)
            {
                userManager.AddToRole(user.Id, user.RoleType.ToString());
                CreateEmployee(user);
            }

            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db?.Dispose();
            }
        }

        //Private Parts
        private void CreateEmployee(ApplicationUser user)
        {
            var db = new Models.Context.TaskManagerDbContext();

            var emp = new ProjectManager()
            {
                EmployeeId = user.Id,
                Email = user.Email,
                FirstName = user.Name,
                LastName = user.Surname
            };
            db.Entry(emp).State = EntityState.Added;
            db.SaveChanges();
        }
    }
}