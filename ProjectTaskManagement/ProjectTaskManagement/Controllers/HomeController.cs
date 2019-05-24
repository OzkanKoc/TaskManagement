using ProjectTaskManagement.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ProjectTaskManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var roles = Enum.GetNames(typeof(RoleType));

            for (int i = 0; i < roles.Length; i++)
            {
                if (HttpContext.User.IsInRole(roles[i]))
                {
                    TempData["UserInfo"] = new List<string>()
                    {
                        HttpContext.User.Identity.Name,
                       ((RoleType)i)
                            .GetType()
                            .GetMember(roles[i])
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName(),
                       GetCurrentUserId(User.Identity as ClaimsIdentity)
                };
                    break;
                }
            }

            return View();
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
    }
}