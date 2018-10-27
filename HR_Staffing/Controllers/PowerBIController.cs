using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using HR_Staffing.BusinessLogicLayer;

namespace HR_Staffing.Controllers
{
    [Authorize]
    public class PowerBIController : Controller
    {
        private readonly RolesManager _rolesManager = new RolesManager();

        private string GetCurrentUserEmail()
        {
            return ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }

        // GET: PowerBI
        public ActionResult Index()
        {
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View();
                }
                
                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }
    }
}