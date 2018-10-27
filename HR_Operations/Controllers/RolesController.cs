using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using CoEX_HRMS.BusinessLogicLayer;
using CoEX_HRMS.DataAccessLayer;
using CoEX_HRMS.Models;
using CoEX_HRMS.Utils;
using LumenWorks.Framework.IO.Csv;

namespace CoEX_HRMS.Controllers
{
    [Authorize]
    public class RolesController : Controller
    {
        private readonly RoleDataAccess _roleDataAccess = new RoleDataAccess();
        private readonly EmployeeDataAccess _employeeDataAccess = new EmployeeDataAccess();
        private readonly RolesManager _rolesManager = new RolesManager();

        private string GetCurrentUserEmail()
        {
            return ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }

        public ActionResult Index()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                var accessLevel = _rolesManager.IdentifyRole();
                if (accessLevel == "FullAccess" || accessLevel == "FullView")
                {
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    ViewBag.Access = accessLevel;
                    return View(_roleDataAccess.GetAllRoles());
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                var accessLevel = _rolesManager.IdentifyRole();
                if (accessLevel == "FullAccess" || accessLevel == "FullView")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    var role = _roleDataAccess.GetRoleById(id);
                    if (role == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    ViewBag.Access = accessLevel;
                    return View(role);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                if (_rolesManager.IdentifyRole() == "FullAccess")
                {
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    ViewBag.Access = "FullAccess";
                    return View();
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title")] Role role)
        {
            if (ModelState.IsValid)
            {
                _roleDataAccess.CreateRole(role);
                TempData["SubmissionStatus"] = "Success";
                TempData["FlashMsg"] = "Role created successfuly!";
                return RedirectToAction("Index");
            }

            return View(role);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                if (_rolesManager.IdentifyRole() == "FullAccess")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    var role = _roleDataAccess.GetRoleById(id);
                    if (role == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    ViewBag.Access = "FullAccess";
                    return View(role);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] Role role)
        {
            if (ModelState.IsValid)
            {
                _roleDataAccess.EditRole(role);
                TempData["SubmissionStatus"] = "Success";
                TempData["FlashMsg"] = "Role updated successfuly!";
                return RedirectToAction("Index");
            }
            return View(role);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                if (_rolesManager.IdentifyRole() == "FullAccess")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    var role = _roleDataAccess.GetRoleById(id);
                    if (role == null)
                    {
                        return HttpNotFound();
                    }

                    if (_employeeDataAccess.GetEmployeesByRoleId(role.Id).Count > 0)
                    {
                        ViewBag.ErrorMsg =
                            "Some Employees already have this Role. Consider updating them first " +
                            "to their new Role, then delete this Role";
                        ViewBag.Access = "FullAccess";
                        ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                        return View("ErrorPartial");
                    }

                    _roleDataAccess.DeleteRole(role);
                    TempData["SubmissionStatus"] = "Success";
                    TempData["FlashMsg"] = "Role deleted successfuly!";

                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }


        public ActionResult ImportRoles()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                if (_rolesManager.IdentifyRole() == "FullAccess")
                {
                    ViewBag.Access = "FullAccess";
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    return View();
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        [HttpPost]
        public ActionResult ImportRoles(HttpPostedFileBase uploadFile)
        {
            if (uploadFile != null)
            {
                // Download the file
                var serverPath = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }
                var filePath = serverPath + Path.GetFileName(uploadFile.FileName);
                var extension = Path.GetExtension(uploadFile.FileName);
                if (extension != ".csv")
                {
                    ViewBag.ErrorMsg = "Invalid file type-Need to be CSV";
                    return View("Error");
                }
                uploadFile.SaveAs(filePath);

                using (var csv = new CsvReader(new StreamReader(filePath), true))
                {
                    while (csv.ReadNextRecord())
                    {
                        var role = new Role() { Title = csv[0] };

                        _roleDataAccess.CreateRole(role);
                    }
                }

                // Delete file from server after finishing 
                GC.Collect();
                GC.WaitForPendingFinalizers();
                System.IO.File.Delete(filePath);
            }

            TempData["SubmissionStatus"] = "Success";
            TempData["FlashMsg"] = "Roles imported successfuly!";
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _roleDataAccess.DbDispose();
            }
            base.Dispose(disposing);
        }
    }
}
