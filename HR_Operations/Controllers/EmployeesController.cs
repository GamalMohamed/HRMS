using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using CoEX_HRMS.Models;
using CoEX_HRMS.BusinessLogicLayer;
using CoEX_HRMS.DataAccessLayer;
using CoEX_HRMS.Utils;
using CoEX_HRMS.ViewModels;
using LumenWorks.Framework.IO.Csv;

namespace CoEX_HRMS.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly EmployeeDataAccess _employeeDataAccess = new EmployeeDataAccess();
        private readonly RoleDataAccess _roleDataAccess = new RoleDataAccess();
        private readonly WorkloadDataAccess _workloadDataAccess = new WorkloadDataAccess();
        private readonly RolesManager _rolesManager = new RolesManager();

        private string GetCurrentUserEmail()
        {
            return ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }

        public List<Employee> SearchActiveEmployees(SearchFormViewModel searchForm)
        {
            var employeesList = _employeeDataAccess.GetActiveEmployees();

            if (searchForm.Gender != null)
            {
                var emps = _employeeDataAccess.GetEmployeesByGender(searchForm.Gender);
                employeesList = employeesList.Intersect(emps).ToList();
            }
            if (searchForm.StartingBirthDate != null)
            {
                if (searchForm.EndingBirthDate == null)
                {
                    searchForm.EndingBirthDate = DateTime.Now;
                }
                var emps = _employeeDataAccess.GetEmployeesByBirthDateInterval((DateTime)searchForm.StartingBirthDate, (DateTime)searchForm.EndingBirthDate);
                employeesList = employeesList.Intersect(emps).ToList();
            }
            if (searchForm.StartingHiringDate != null)
            {
                if (searchForm.EndingHiringDate == null)
                {
                    searchForm.EndingHiringDate = DateTime.Now;
                }
                var emps = _employeeDataAccess.GetEmployeesByHiringDateInterval((DateTime)searchForm.StartingHiringDate, (DateTime)searchForm.EndingHiringDate);
                employeesList = employeesList.Intersect(emps).ToList();
            }
            if (searchForm.GraduationYear != null)
            {
                var emps = _employeeDataAccess.GetAllEmployees().Where(employee => searchForm.GraduationYear.Any(condition => employee.GraduationYear == condition)).ToList();
                employeesList = employeesList.Intersect(emps).ToList();
            }

            if (searchForm.MajorUniversity != null)
            {
                var emps = _employeeDataAccess.GetAllEmployees().Where(employee => searchForm.MajorUniversity.Any(condition => employee.MajorUniversity == condition)).ToList();
                employeesList = employeesList.Intersect(emps).ToList();
            }
            if (searchForm.WorkloadId != null)
            {
                var emps = _employeeDataAccess.GetAllEmployees().Where(employee => searchForm.WorkloadId.Any(condition => employee.WorkloadId == condition)).ToList();
                employeesList = employeesList.Intersect(emps).ToList();
            }
            if (searchForm.RoleId != null)
            {
                var emps = _employeeDataAccess.GetAllEmployees().Where(employee => searchForm.RoleId.Any(condition => employee.RoleId == condition)).ToList();
                employeesList = employeesList.Intersect(emps).ToList();
            }
            if (searchForm.Subsidiary != null)
            {
                var emps = _employeeDataAccess.GetAllEmployees().Where(employee => searchForm.Subsidiary.Any(condition => employee.Subsidiary == condition)).ToList();
                employeesList = employeesList.Intersect(emps).ToList();
            }
            if (searchForm.Vendor != null)
            {
                var emps = _employeeDataAccess.GetAllEmployees().Where(employee => searchForm.Vendor.Any(condition => employee.Vendor == condition)).ToList();
                employeesList = employeesList.Intersect(emps).ToList();
            }
            if (searchForm.BasedOut != null)
            {
                var emps = _employeeDataAccess.GetAllEmployees().Where(employee => searchForm.BasedOut.Any(condition => employee.BasedOut == condition)).ToList();
                employeesList = employeesList.Intersect(emps).ToList();
            }
            if (searchForm.ReportingManager != null)
            {
                var emps = _employeeDataAccess.GetAllEmployees().Where(employee => searchForm.ReportingManager.Any(condition => employee.ReportingManager == condition)).ToList();
                employeesList = employeesList.Intersect(emps).ToList();
            }
            if (searchForm.ExperienceYears != null)
            {
                var emps = _employeeDataAccess.GetAllEmployees().Where(employee => searchForm.ExperienceYears.Any(condition => employee.ExperienceYears == condition)).ToList();
                employeesList = employeesList.Intersect(emps).ToList();

            }
            if (searchForm.PreviousEmployer != null)
            {
                var emps = _employeeDataAccess.GetAllEmployees().Where(employee => searchForm.PreviousEmployer.Any(condition => employee.PreviousEmployer == condition)).ToList();
                employeesList = employeesList.Intersect(emps).ToList();
            }

            return employeesList;
        }

        public List<Employee> SearchResignedEmployees(ResignedViewModel resignedSearchViewModel)
        {
            var resignedList = _employeeDataAccess.GetResignedEmployees();
            if (resignedSearchViewModel.Attrition != 0 && resignedSearchViewModel.Attrition != null)
            {
                var emps = _employeeDataAccess.GetResignedEmployeesByAttrition((Attrition)resignedSearchViewModel.Attrition);
                resignedList = resignedList.Intersect(emps).ToList();
            }
            if (resignedSearchViewModel.StartingResignationDate != null)
            {
                if (resignedSearchViewModel.EndingResignationDate == null)
                {
                    resignedSearchViewModel.EndingResignationDate = DateTime.Now;
                }
                var emps = _employeeDataAccess.GetEmployeesByResignationDateInterval(
                    (DateTime)resignedSearchViewModel.StartingResignationDate,
                    (DateTime)resignedSearchViewModel.EndingResignationDate);

                resignedList = resignedList.Intersect(emps).ToList();
            }
            if (resignedSearchViewModel.Movement != null)
            {
                var emps = _employeeDataAccess.GetAllEmployees().Where(employee => resignedSearchViewModel.Movement.Any(condition => employee.Movement == condition)).ToList();
                resignedList = resignedList.Intersect(emps).ToList();
            }

            return resignedList;
        }

        public AutoCompleteViewModel GetAllautoCompleteInfo()
        {
            var autocomplete = new AutoCompleteViewModel
            {
                ReportingMangersList = _employeeDataAccess.GetAllReportingManagers(),
                BasedOutList = _employeeDataAccess.GetAllBasedOuts(),
                SubsidaryList = _employeeDataAccess.GetAllSubsidiaries(),
                MajorUniversitiesList = _employeeDataAccess.GetAllMajorUniversity(),
                VendorsList = _employeeDataAccess.GetAllVendors(),
                PreviousEmployersList = _employeeDataAccess.GetAllPreviousEmployers(),
                GraduationyearsList = _employeeDataAccess.GetAllGraduationYears(),
                ExperiencesList = _employeeDataAccess.GetAllExperienceYears()
            };
            return autocomplete;
        }


        [HttpPost]
        public JsonResult ReportingManagerAutoComplete(string prefix)
        {
            var reportingManagers = _employeeDataAccess.GetAllReportingManagers()
                                    .Select(rm => new Employee() { Name = rm })
                                    .ToList();

            reportingManagers.AddRange(_employeeDataAccess.GetEmployeeByAccessLevel(AccessLevel.TeamView)
                                       .Where(m => reportingManagers.All(y => y.Name != m.Name)));

            var lst = reportingManagers.Where(n => n.Name.StartsWith(prefix, true, new CultureInfo("en-US")))
                                       .Select(n => new { n.Name });

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MajorUniversityAutoComplete(string prefix)
        {
            var majorUniversities = _employeeDataAccess.GetAllMajorUniversity()
                                    .Select(rm => new Employee() { MajorUniversity = rm })
                                    .ToList();

            var lst = majorUniversities.Where(n => n.MajorUniversity.StartsWith(prefix, true, new CultureInfo("en-US")))
                                       .Select(n => new { n.MajorUniversity });

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubsidiaryAutoComplete(string prefix)
        {
            var subs = _employeeDataAccess.GetAllSubsidiaries()
                                    .Select(rm => new Employee() { Subsidiary = rm })
                                    .ToList();

            var lst = subs.Where(n => n.Subsidiary.StartsWith(prefix, true, new CultureInfo("en-US")))
                                       .Select(n => new { n.Subsidiary });

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult VendorAutoComplete(string prefix)
        {
            var vendors = _employeeDataAccess.GetAllVendors()
                                    .Select(rm => new Employee() { Vendor = rm })
                                    .ToList();

            var lst = vendors.Where(n => n.Vendor.StartsWith(prefix, true, new CultureInfo("en-US")))
                                       .Select(n => new { n.Vendor });

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BasedOutAutoComplete(string prefix)
        {
            var basedouts = _employeeDataAccess.GetAllBasedOuts()
                                    .Select(rm => new Employee() { BasedOut = rm })
                                    .ToList();

            var lst = basedouts.Where(n => n.BasedOut.StartsWith(prefix, true, new CultureInfo("en-US")))
                                       .Select(n => new { n.BasedOut });

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PreviousEmployerAutoComplete(string prefix)
        {
            var previousEmployers = _employeeDataAccess.GetAllPreviousEmployers()
                                    .Select(rm => new Employee() { PreviousEmployer = rm })
                                    .ToList();

            var lst = previousEmployers.Where(n => n.PreviousEmployer.StartsWith(prefix, true, new CultureInfo("en-US")))
                                       .Select(n => new { n.PreviousEmployer });

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MovementAutoComplete(string prefix)
        {
            var movements = _employeeDataAccess.GetAllMovements()
                                    .Select(rm => new Employee() { Movement = rm })
                                    .ToList();

            var lst = movements.Where(n => n.Movement.StartsWith(prefix, true, new CultureInfo("en-US")))
                                       .Select(n => new { n.Movement });

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        // GET: Employees
        public ActionResult Index()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                var accessLevel = _rolesManager.IdentifyRole();
                if (accessLevel == "FullAccess" || accessLevel == "FullView" || accessLevel == "TeamView")
                {
                    var employees = _rolesManager.ListRespectiveActiveEmployees(_employeeDataAccess);
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    ViewBag.Access = _rolesManager.IdentifyRole();
                    return View(employees);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                var accessLevel = _rolesManager.IdentifyRole();
                if (accessLevel == "FullAccess" || accessLevel == "FullView" || accessLevel == "TeamView")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
                    {

                        var employee = new Employee();
                        if (!_rolesManager.GetEmployeeDetails(_employeeDataAccess, ref employee, id))
                        {
                            ViewBag.ErrorMsg = "You are not authorized to view this page";
                            return View("Error");
                        }
                        if (employee == null) //When HR, and returns no results!
                        {
                            return HttpNotFound();
                        }

                        //found!
                        ViewBag.Access = _rolesManager.IdentifyRole();
                        ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                        return View(employee);
                    }
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                if (_rolesManager.IdentifyRole() == "FullAccess")
                {
                    ViewBag.WorkloadsList = new SelectList(_workloadDataAccess.GetAllWorkloads(), "Id", "Name");
                    ViewBag.RolesList = new SelectList(_roleDataAccess.GetAllRoles(), "Id", "Title");
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    ViewBag.Access = TempData["AccessLevel"] = "FullAccess";
                    TempData["ProfilePic"] = _rolesManager.LoggedInEmployee.Profile.ProfilePic;

                    return View();
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,PhoneNumber,Gender,BirthDate," +
                                                   "GraduationYear,ExperienceYears,MajorUniversity," +
                                                   "AccessLevel,WorkloadId,RoleId,HiringDate,Subsidiary," +
                                                   "Vendor,BasedOut,ReportingManager,PreviousEmployer")]Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Profile = new Profile() { Id = employee.Id };

                if (employee.WorkloadId != null)
                {
                    //Update Workload count
                    _workloadDataAccess.GetWorkloadById(employee.WorkloadId).EmployeesCount++;
                    _workloadDataAccess.SaveChangesToDb();
                }

                _employeeDataAccess.CreateEmployee(employee);

                TempData["SubmissionStatus"] = "Success";
                TempData["FlashMsg"] = "Employee created successfully!";
                if (employee.Status == Status.Resigned)
                {
                    return RedirectToAction("Resigned");
                }
                return RedirectToAction("Index");
            }

            ViewBag.Access = TempData["AccessLevel"].ToString();
            ViewBag.ProfilePic = TempData["ProfilePic"].ToString();
            ViewBag.ErrorMsg = "Failed to create employee. Check the correctness of your input format";
            return View("ErrorPartial");
        }

        // GET: ImportEmployees
        public ActionResult ImportEmployees()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                if (_rolesManager.IdentifyRole() == "FullAccess")
                {
                    ViewBag.Access = "FullAccess";
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    return View();
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page.";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        [HttpPost]
        public ActionResult ImportEmployees(HttpPostedFileBase uploadFile)
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
                var extension = Path.GetExtension(uploadFile.FileName)?.ToLower();
                if (extension != ".csv")
                {
                    ViewBag.ErrorMsg = "Invalid file type-Need to be CSV";
                    return View("Error");
                }
                uploadFile.SaveAs(filePath);

                using (var csv = new CsvReader(new StreamReader(filePath), true))
                {
                    var fieldCount = csv.FieldCount;
                    var headers = csv.GetFieldHeaders();
                    while (csv.ReadNextRecord())
                    {
                        var employee = new Employee();
                        for (int i = 0; i < fieldCount; i++)
                        {
                            switch (headers[i])
                            {
                                case "Name":
                                    employee.Name = csv[i];
                                    break;
                                case "Gender":
                                    employee.Gender = csv[i];
                                    break;
                                case "HiringDate":
                                    employee.HiringDate = DateTime.ParseExact(csv[i], "d", null);
                                    break;
                                case "RoleId":
                                    employee.RoleId = Convert.ToInt32(csv[i]);
                                    break;
                                case "WorkloadId":
                                    employee.WorkloadId = Convert.ToInt32(csv[i]);
                                    break;
                                case "Subsidiary":
                                    employee.Subsidiary = csv[i];
                                    break;
                                case "Vendor":
                                    employee.Vendor = csv[i];
                                    break;
                                case "BasedOut":
                                    employee.BasedOut = csv[i];
                                    break;
                                case "ReportingManager":
                                    employee.ReportingManager = csv[i];
                                    break;
                                case "BirthDate":
                                    employee.BirthDate = DateTime.ParseExact(csv[i], "d", null);
                                    break;
                                case "GraduationYear":
                                    employee.GraduationYear = csv[i];
                                    break;
                                case "ExperienceYears":
                                    employee.ExperienceYears = Convert.ToInt32(csv[i]);
                                    break;
                                case "AccessLevel":
                                    employee.AccessLevel = (AccessLevel)Enum.Parse(typeof(AccessLevel), csv[i]);
                                    break;
                                case "Email":
                                    employee.Email = csv[i];
                                    break;
                                case "PhoneNumber":
                                    employee.PhoneNumber = csv[i];
                                    break;
                                case "MajorUniversity":
                                    employee.MajorUniversity = csv[i];
                                    break;
                                case "PreviousEmployer":
                                    employee.PreviousEmployer = csv[i];
                                    break;
                                case "Movement":
                                    employee.Movement = csv[i];
                                    break;
                                case "Attrition":
                                    employee.Attrition = (Attrition)Enum.Parse(typeof(Attrition), csv[i]);
                                    break;
                                case "Status":
                                    employee.Status = (Status)Enum.Parse(typeof(Status), csv[i]);
                                    break;
                                case "ResignationDate":
                                    employee.ResignationDate = DateTime.ParseExact(csv[i], "d", null);
                                    break;
                                case "ResignationStatus":
                                    employee.ResignationStatus = csv[i];
                                    break;
                            }
                        }

                        if (employee.WorkloadId != null)
                        {
                            //Update Workload count
                            _workloadDataAccess.GetWorkloadById(employee.WorkloadId).EmployeesCount++;
                            _workloadDataAccess.SaveChangesToDb();
                        }
                        employee.Profile=new Profile(){Id = employee.Id};
                        _employeeDataAccess.CreateEmployee(employee);
                    }
                }

                // Delete file from server after finishing 
                GC.Collect();
                GC.WaitForPendingFinalizers();
                System.IO.File.Delete(filePath); 
            }

            TempData["SubmissionStatus"] = "Success";
            TempData["FlashMsg"] = "Employees imported successfuly!";
            return RedirectToAction("Index");
        }

        // GET: Employees/Edit/5
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

                    var employee = _employeeDataAccess.GetEmployeeById(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }

                    TempData["existingWorkloadId"] = employee.WorkloadId;
                    ViewBag.WorkloadsList = new SelectList(_workloadDataAccess.GetAllWorkloads(), "Id", "Name");
                    ViewBag.RolesList = new SelectList(_roleDataAccess.GetAllRoles(), "Id", "Title");
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    TempData["ProfilePic"] = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    ViewBag.Access = "FullAccess";
                    TempData["AccessLevel"] = "FullAccess";

                    return View(employee);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page.";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,PhoneNumber,Gender,BirthDate," +
                                                 "GraduationYear,ExperienceYears,MajorUniversity," +
                                                 "AccessLevel,WorkloadId,RoleId,HiringDate,Subsidiary," +
                                                 "Vendor,BasedOut,ReportingManager,PreviousEmployer,Status,Attrition," +
                                                 "ResignationDate,Movement,ResignationStatus")]Employee employee)
        {
            if (ModelState.IsValid)
            {
                _employeeDataAccess.EditEmployee(employee);

                var existingWorkloadId = (int?)TempData["existingWorkloadId"];
                var possibleEditedWorkloadId = employee.WorkloadId;
                if (existingWorkloadId != possibleEditedWorkloadId)
                {
                    if (existingWorkloadId != null)
                    {
                        _workloadDataAccess.GetWorkloadById(existingWorkloadId).EmployeesCount--;
                    }
                    if (possibleEditedWorkloadId != null)
                    {
                        _workloadDataAccess.GetWorkloadById(possibleEditedWorkloadId).EmployeesCount++;
                    }
                    _workloadDataAccess.SaveChangesToDb();
                }

                TempData["SubmissionStatus"] = "Success";
                TempData["FlashMsg"] = "Employee updated successfully!";
                if (employee.Status == Status.Resigned)
                {
                    return RedirectToAction("Resigned");
                }
                return RedirectToAction("Index");
            }

            ViewBag.ErrorMsg = "Failed to edit employee. Check the correctness of your input format";
            ViewBag.ProfilePic = TempData["ProfilePic"].ToString();
            ViewBag.Access = TempData["AccessLevel"].ToString();
            return View("ErrorPartial");
        }

        // GET: Employees/Delete/5
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

                    var employee = _employeeDataAccess.GetEmployeeById(id);
                    if (employee == null)
                    {
                        return HttpNotFound();
                    }
                    if (employee.WorkloadId != null)
                    {
                        //Update Workload count
                        _workloadDataAccess.GetWorkloadById(employee.WorkloadId).EmployeesCount--;
                        _workloadDataAccess.SaveChangesToDb();
                    }
                    _employeeDataAccess.DeleteEmployee(employee);

                    TempData["SubmissionStatus"] = "Success";
                    TempData["FlashMsg"] = "Employee deleted successfully!";
                    if (employee.Status==Status.Resigned)
                    {
                        return RedirectToAction("Resigned");
                    }
                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page.";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        // GET: Employees/Search
        public ActionResult AdvancedSearch()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                var accessLevel = _rolesManager.IdentifyRole();
                if (accessLevel == "FullAccess" || accessLevel == "FullView")
                {
                    ViewBag.WorkloadsList = new SelectList(_workloadDataAccess.GetAllWorkloads(), "Id", "Name");
                    ViewBag.RolesList = new SelectList(_roleDataAccess.GetAllRoles(), "Id", "Title");
                    ViewBag.Access = accessLevel;
                    TempData["AccessLevel"] = accessLevel;
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    TempData["ProfilePic"] = _rolesManager.LoggedInEmployee.Profile.ProfilePic;

                    var searchpackage = GetAllautoCompleteInfo();
                    return View(searchpackage);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page.";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdvancedSearchResult(AutoCompleteViewModel searchForm)
        {
            if (ModelState.IsValid)
            {
                ViewBag.ProfilePic = TempData["ProfilePic"].ToString();
                ViewBag.Access = TempData["AccessLevel"].ToString();

                var searchResults = SearchActiveEmployees(searchForm.SearchFormViewModel);
                return View(searchResults);
            }

            ViewBag.ErrorMsg = "Failed to search. Check the correctness of your input format";
            ViewBag.ProfilePic = TempData["ProfilePic"].ToString();
            ViewBag.Access = TempData["AccessLevel"].ToString();
            return View("ErrorPartial");
        }

        //GET: Employees/Resigned
        public ActionResult Resigned()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                var accessLevel = _rolesManager.IdentifyRole();
                if (accessLevel == "FullAccess" || accessLevel == "FullView" || accessLevel == "TeamView")
                {
                    var employees = _rolesManager.ListRespectiveResignedEmployees(_employeeDataAccess);
                    ViewBag.Access = accessLevel;
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;

                    return View(employees);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page.";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        public ActionResult ResignedSearch()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                var accessLevel = _rolesManager.IdentifyRole();
                if (accessLevel == "FullAccess" || accessLevel == "FullView")
                {
                    ViewBag.Access = accessLevel;
                    TempData["AccessLevel"] = accessLevel;
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                    TempData["ProfilePic"] = _rolesManager.LoggedInEmployee.Profile.ProfilePic;

                    var searchpackage = new ResignedViewModel()
                    {
                        MovementsList = _employeeDataAccess.GetAllMovements()
                    };
                    return View(searchpackage);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page.";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResignedSearchResults(ResignedViewModel resignedSearchViewModel)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Access = TempData["AccessLevel"].ToString();
                ViewBag.ProfilePic = TempData["ProfilePic"].ToString();

                var resignedEmployees = SearchResignedEmployees(resignedSearchViewModel);
                return View(resignedEmployees);
            }

            ViewBag.ErrorMsg = "Failed to search. Check the correctness of your input format";
            ViewBag.Access = TempData["AccessLevel"].ToString();
            ViewBag.ProfilePic = TempData["ProfilePic"].ToString();
            return View("ErrorPartial");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _employeeDataAccess.DbDispose(true);
            }
            base.Dispose(disposing);
        }

    }
}
