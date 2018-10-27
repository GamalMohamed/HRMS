using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using CoEX_HRMS.BusinessLogicLayer;
using CoEX_HRMS.DataAccessLayer;

namespace CoEX_HRMS.Controllers
{
    [Authorize]
    public class PowerBIController : Controller
    {
        private readonly RolesManager _rolesManager = new RolesManager();
        private readonly EmployeeDataAccess _employeeDataAccess = new EmployeeDataAccess();

        private string GetCurrentUserEmail()
        {
            return ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }

        // GET: PowerBI
        public ActionResult Index()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                var accessLevel = _rolesManager.IdentifyRole();
                if (accessLevel == "FullAccess" || accessLevel == "FullView" || accessLevel == "TeamView")
                {
                    ViewBag.Access = accessLevel;
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    return View();
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered in our system";
            return View("Error");
        }
    }
}