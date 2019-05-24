using Microsoft.AspNet.Identity.EntityFramework;
using ProjectTaskManagement.Identity;
using ProjectTaskManagement.Migrations;
using ProjectTaskManagement.Models.DataAccess.Entity;
using ProjectTaskManagement.Models.Mappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Models.Context
{
    public class TaskManagerDbContext : IdentityDbContext<ApplicationUser>
    {
        public TaskManagerDbContext() : base("TaskManagerConnectionString")
        { }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<BusinessAnalyst> BusinessAnalyst { get; set; }
        public DbSet<CustomerContact> CustomerContact { get; set; }
        public DbSet<CustomerDemand> CustomerDemand { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectManager> ProjectManager { get; set; }
        public DbSet<SoftwareDeveloper> SoftwareDeveloper { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<TeamLeader> TeamLeader { get; set; }
        public DbSet<TestSpecialist> TestSpecialist { get; set; }
        public DbSet<TaskAssigmentHistory> TaskAssigmentHistory { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new CustomerContactMapping());
            modelBuilder.Configurations.Add(new CustomerDemandMapping());
            modelBuilder.Configurations.Add(new CustomerMapping());
            modelBuilder.Configurations.Add(new EmployeeMapping());
            modelBuilder.Configurations.Add(new ProjectMapping());
            modelBuilder.Configurations.Add(new TaskMapping());

            modelBuilder.Entity<IdentityUserLogin>().HasKey(iul => iul.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey(ir => ir.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(iur => new { iur.RoleId, iur.UserId });
            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(au => au.Employee)
                .WithRequired(e => e.User);
        }
    }
}