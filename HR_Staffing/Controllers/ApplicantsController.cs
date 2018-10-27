using HR_Staffing.DataAccessLayer;
using HR_Staffing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using HR_Staffing.BusinessLogicLayer;

namespace HR_Staffing.Controllers
{
    [Authorize]
    public class ApplicantsController : Controller
    {
        private readonly ApplicantsDataAccess _applicantsDataAccess = new ApplicantsDataAccess();
        private readonly FormQuestionsDataAccess _formQuestionsDataAccess = new FormQuestionsDataAccess();
        private readonly ApplicantsQuestionsDataAccess _applicantQuestionsDataAccess = new ApplicantsQuestionsDataAccess();
        private readonly RolesManager _rolesManager = new RolesManager();

        private string GetCurrentUserEmail()
        {
            return ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }

        private List<string> GetAllHiringManagers()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(RolesManager.ApiBaseUri);
                var responseTask = client.GetAsync("EmployeesApi?reportingManagers=" + true);
                responseTask.Wait();

                var result = responseTask.Result;
                if (!result.IsSuccessStatusCode)
                    return null;

                var readTask = result.Content.ReadAsAsync<List<string>>();
                readTask.Wait();

                return readTask.Result;
            }
        }

        // GET: Applicants
        public ActionResult Index()
        {
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsManager() || _rolesManager.IsStaffing())
                {
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View(_rolesManager.ListRespectiveApplicants(_applicantsDataAccess));
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        public ActionResult New()
        {
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    ViewBag.HiringManagers = new SelectList(GetAllHiringManagers());
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View("ApplicantForm", new Applicant());
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");

        }

        public ActionResult Edit(int? id)
        {
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var applicant = _applicantsDataAccess.GetApplicantById(id);

                    if (applicant == null)
                        return HttpNotFound();

                    ViewBag.HiringManagers = new SelectList(GetAllHiringManagers());
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View("ApplicantForm", applicant);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Applicant applicant)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            if (applicant.Id == 0)
            {
                var applicantId = _applicantsDataAccess.CreateApplicant(applicant);

                var formQuestions = _formQuestionsDataAccess.GetAllQuestions(FormType.ApplicationForm);
                foreach (var formQuestion in formQuestions)
                {
                    var applicantQuestion = new ApplicantQuestion
                    {
                        ApplicantId = applicantId,
                        OrderInForm = formQuestion.OrderInForm,
                        Title = formQuestion.Title,
                        Description = formQuestion.Description
                    };
                    _applicantQuestionsDataAccess.CreateQuestion(applicantQuestion);
                }
            }
            else
            {
                _applicantsDataAccess.EditApplicant(applicant);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var applicant = _applicantsDataAccess.GetApplicantById(id);

                    if (applicant == null)
                        return HttpNotFound();
                    if (applicant.Status == ApplicantStatus.Invited)
                    {
                        _applicantsDataAccess.DeleteApplicant(applicant);
                        return RedirectToAction("Index");
                    }

                    ViewBag.ErrorMsg = "Can't delete applicant as there's dependant data on it in the system";
                    return View("ErrorPartial");
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing() || _rolesManager.IsManager())
                {
                    var applicant = new Applicant();
                    if (!_rolesManager.GetApplicantDetails(_applicantsDataAccess, ref applicant, id))
                    {
                        ViewBag.ErrorMsg = "You are not authorized to view this page";
                        return View("Error");
                    }
                    if (applicant == null)
                    {
                        return HttpNotFound();
                    }

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View(applicant);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }
    }
}