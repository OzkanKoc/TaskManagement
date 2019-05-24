using Microsoft.AspNet.Identity;
using ProjectTaskManagement.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectTaskManagement.Filters
{
    public class AuthorizedRoleAttribute : AuthorizeAttribute
    {
        private RoleType[] _allowedRoles;
        public AuthorizedRoleAttribute(params RoleType[] roles)
        {
            _allowedRoles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.Request.IsAuthenticated)
            {
                httpContext.Response.Redirect("/Account/Login");
            }

            foreach (var allowedRole in _allowedRoles)
            {
                if (httpContext.User.IsInRole(allowedRole.ToString()))
                {
                    return true;
                }
            }
            httpContext.Response.Redirect("/Home/Index");
            return false;
        }
    }
}