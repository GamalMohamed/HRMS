using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using CoEX_HRMS.Models;
using CoEX_HRMS.DataAccessLayer;
using CoEX_HRMS.Models;
using CoEX_HRMS.Utils;
using CoEX_HRMS.ViewModels;
using CoEX_HRMS.Services;
using System.Net;
using System.IO;
using CoEX_HRMS.BusinessLogicLayer;

namespace CoEX_HRMS.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly EmployeeDataAccess _employeeDataAccess = new EmployeeDataAccess();
        private readonly ProfileDataAccess _profileDataAccess = new ProfileDataAccess();
        private readonly RolesManager _rolesManager = new RolesManager();

        // GET: Dashboard
        public ActionResult Index()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess,
                ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value))
            {
                var accessLevel = _rolesManager.IdentifyRole();
                switch (accessLevel)
                {
                    case "FullAccess":
                    case "FullView":
                        var notificationVeiwModel = new NotificationViewModel
                        {
                            ResignedEmployees = GetEmployeesByNearResigndate(),
                            HiringEmployees = GetEmployeesByNearHiredate(),
                            UncompletedProfiles = GetProfilesByMissingItems()
                        };
                        ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                        ViewBag.Access = accessLevel;
                        return View(notificationVeiwModel);
                    case "TeamView":
                        ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                        ViewBag.Access = accessLevel;
                        return View();
                    case "EmployeeView":
                        return RedirectToAction("Index", "Profile");
                    default:
                        ViewBag.ErrorMsg = "You are not authorized to view this page";
                        return View("Error");
                }
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        public List<Employee> GetEmployeesByNearResigndate()
        {
            return _employeeDataAccess.GetAllEmployees().Where(e => e.ResignationDate != null &&
                                            e.ResignationDate.Value.Year == DateTime.Now.Year &&
                                            e.ResignationDate.Value.Month == DateTime.Now.Month &&
                                            (e.ResignationDate.Value.Day - DateTime.Now.Day) <= 7 &&
                                            (e.ResignationDate.Value.Day - DateTime.Now.Day) >= 0).ToList();
        }

        public List<Employee> GetEmployeesByNearHiredate()
        {
            return _employeeDataAccess.GetAllEmployees().Where(e => e.HiringDate != null &&
                                            e.HiringDate.Value.Year == DateTime.Now.Year &&
                                            e.HiringDate.Value.Month == DateTime.Now.Month &&
                                            (e.HiringDate.Value.Day - DateTime.Now.Day) <= 7 &&
                                            (e.HiringDate.Value.Day - DateTime.Now.Day) >= 0).ToList();
        }

        public List<Profile> GetProfilesByMissingItems()
        {
            return _profileDataAccess.GetAllProfiles()
                .Where(p => (p.BirthCertificate == null ||
                p.GraduationCertificate == null || p.IdCard == null ||
                p.MilitaryCertificate == null || p.Passport == null) && p.Employee.Status == Status.Active)
                .ToList();
        }
    }
}
