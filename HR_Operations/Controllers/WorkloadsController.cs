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
using CoEX_HRMS.Models;
using CoEX_HRMS.BusinessLogicLayer;
using CoEX_HRMS.DataAccessLayer;
using CoEX_HRMS.Models;
using CoEX_HRMS.Utils;
using LumenWorks.Framework.IO.Csv;

namespace CoEX_HRMS.Controllers
{
    [Authorize]
    public class WorkloadsController : Controller
    {
        private readonly WorkloadDataAccess _workloadDataDataAccess = new WorkloadDataAccess();
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
                    return View(_workloadDataDataAccess.GetAllWorkloads());
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        // GET: Workloads/Details/5
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

                    var team = _workloadDataDataAccess.GetWorkloadById(id);
                    if (team == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    ViewBag.Access = accessLevel;
                    return View(team);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        // GET: Workloads/Create
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

        // POST: Workloads/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Workload workload)
        {
            if (ModelState.IsValid)
            {
                _workloadDataDataAccess.CreateWorkload(workload);

                TempData["SubmissionStatus"] = "Success";
                TempData["FlashMsg"] = "Workload created successfuly!";
                return RedirectToAction("Index");
            }

            return View(workload);
        }

        // GET: Workloads/Edit/5
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
                    var team = _workloadDataDataAccess.GetWorkloadById(id);
                    if (team == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    ViewBag.Access = "FullAccess";
                    return View(team);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        // POST: Workloads/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Workload workload)
        {
            if (ModelState.IsValid)
            {
                _workloadDataDataAccess.EditWorkload(workload);
                TempData["SubmissionStatus"] = "Success";
                TempData["FlashMsg"] = "Workload updated successfuly!";
                return RedirectToAction("Index");
            }
            return View(workload);
        }

        // GET: Workloads/Delete/5
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

                    var workload = _workloadDataDataAccess.GetWorkloadById(id);
                    if (workload == null)
                    {
                        return HttpNotFound();
                    }
                    if (workload.EmployeesCount > 0)
                    {
                        ViewBag.ErrorMsg =
                            "Some Employees already have this workload. Consider updating them first " +
                            "to their new workload, then delete this workload";
                        ViewBag.Access = "FullAccess";
                        ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                        return View("ErrorPartial");
                    }

                    _workloadDataDataAccess.DeleteWorkload(workload);

                    TempData["SubmissionStatus"] = "Success";
                    TempData["FlashMsg"] = "Workload deleted successfuly!";
                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }


        public ActionResult ImportWorkloads()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                if (_rolesManager.IdentifyRole() == "FullAccess")
                {
                    ViewBag.Access = "FullAccess";
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;

                    TempData["SubmissionStatus"] = "Success";
                    TempData["FlashMsg"] = "Workloads imported successfuly!";
                    return View();
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        [HttpPost]
        public ActionResult ImportWorkloads(HttpPostedFileBase uploadFile)
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
                        var workload = new Workload() { Name = csv[0] };

                        _workloadDataDataAccess.CreateWorkload(workload);
                    }
                }

                // Delete file from server after finishing 
                GC.Collect();
                GC.WaitForPendingFinalizers();
                System.IO.File.Delete(filePath);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _workloadDataDataAccess.DbDispose();
            }
            base.Dispose(disposing);
        }
    }
}
