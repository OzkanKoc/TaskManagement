using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ProjectTaskManagement.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectTaskManagement
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Rol tanımlama
            var db = new Models.Context.TaskManagerDbContext();
            RoleStore<ApplicationRole> roleStore = new RoleStore<ApplicationRole>(db);
            RoleManager<ApplicationRole> roleManager = new RoleManager<ApplicationRole>(roleStore);

            if (!roleManager.RoleExists("ProjectManager"))
            {
                ApplicationRole role = new ApplicationRole("ProjectManager", "Proje Yöneticisi");
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("BusinessAnalyst"))
            {
                ApplicationRole role = new ApplicationRole("BusinessAnalyst", "İş Analisti");
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("SoftwareDeveloper"))
            {
                ApplicationRole role = new ApplicationRole("SoftwareDeveloper", "Yazılım Geliştiricisi");
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("TeamLeader"))
            {
                ApplicationRole role = new ApplicationRole("TeamLeader", "Takım Lideri");
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("TestSpecialist"))
            {
                ApplicationRole role = new ApplicationRole("TestSpecialist", "Test Uzmanı");
                roleManager.Create(role);
            }
        }
    }
}
