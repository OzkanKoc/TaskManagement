using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectTaskManagement.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }

        public ApplicationRole()
        {

        }

        public ApplicationRole(string roleName, string description) : base(roleName)
        {
            this.Description = description;
        }
    }
}