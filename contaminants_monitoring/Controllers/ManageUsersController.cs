using Contaminants_Monitoring.Models;
using Contaminants_Monitoring.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contaminants_Monitoring.Controllers
{
    public class ManageUsersController : Controller
    {

        public static ApplicationDbContext context = new ApplicationDbContext();

        // GET: ManageUsers
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UsersWithRoles()
        {
            var usersWithRoles = (from user in context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      Username = user.UserName,
                                      Email = user.Email,
                                      PhoneNumber = user.PhoneNumber,
                                      Center = user.Center,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new UsersViewModel()

                                  {
                                      UserId = p.UserId,
                                      Username = p.Username,
                                      Email = p.Email,
                                      PhoneNumber = p.PhoneNumber,
                                      RegionalOffice = p.Center,
                                      Role = string.Join(",", p.RoleNames)
                                  });
            return View(usersWithRoles);
        }
    }
}