namespace ProjectTaskManagement.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ProjectTaskManagement.Identity;
    using ProjectTaskManagement.Models.DataAccess.Entity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProjectTaskManagement.Models.Context.TaskManagerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        private void CreateEmployee(string userId)
        {
            var db = new Models.Context.TaskManagerDbContext();

            var emp = new ProjectManager()
            {
                EmployeeId = userId,
                Address = "istanbul",
                DateOfBirth = new DateTime(2001, 01, 10),
                Email = "erdem.koc1@gmail.com",
                FirstName = "erdem",
                LastName = "koc",
                HiringDate = new DateTime(2005, 01, 01),
                IdentityNumber = "14075499492",
            };
            db.Entry(emp).State = EntityState.Added;
            db.SaveChanges();
        }
    }
}
