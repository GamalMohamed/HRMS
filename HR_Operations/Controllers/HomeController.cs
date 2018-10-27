using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using CoEX_HRMS.BusinessLogicLayer;
using CoEX_HRMS.DataAccessLayer;
using CoEX_HRMS.Models;
using CoEX_HRMS.Utils;

namespace CoEX_HRMS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly EmployeeDataAccess _employeeDataAccess = new EmployeeDataAccess();
        private readonly RolesManager _rolesManager = new RolesManager();

        private string GetCurrentUserEmail()
        {
            return ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }

        public ActionResult About()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess,
                ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value))
            {
                var accessLevel = _rolesManager.IdentifyRole();
                if (accessLevel == "FullAccess" || accessLevel == "FullView" || accessLevel == "TeamView")
                {
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    ViewBag.Access = accessLevel;
                    return View("About");
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }
    }
}