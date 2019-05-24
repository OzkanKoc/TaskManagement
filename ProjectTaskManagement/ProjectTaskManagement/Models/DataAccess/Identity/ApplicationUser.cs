using Microsoft.AspNet.Identity.EntityFramework;
using ProjectTaskManagement.Models.DataAccess;
using ProjectTaskManagement.Models.DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public RoleType RoleType { get; set; }

        public Employee Employee { get; set; }
    }
}