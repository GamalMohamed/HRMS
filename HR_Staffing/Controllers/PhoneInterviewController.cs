using HR_Staffing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using HR_Staffing.BusinessLogicLayer;
using HR_Staffing.DataAccessLayer;
using HR_Staffing.Models;

namespace HR_Staffing.Controllers
{
    [Authorize]
    public class PhoneInterviewController : Controller
    {
        private readonly ApplicantsDataAccess _applicantsDataAccess = new ApplicantsDataAccess();
        private readonly FormQuestionsDataAccess _formQuestionsDataAccess = new FormQuestionsDataAccess();
        private readonly PhoneInterviewDataAccess _phoneInterviewDataAccess = new PhoneInterviewDataAccess();
        private readonly RolesManager _rolesManager = new RolesManager();

        private string GetCurrentUserEmail()
        {
            return ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }

        // GET: PhoneInterview
        public ActionResult Index(int? applicantId)
        {
            if (applicantId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    ViewBag.ApplicantName = _applicantsDataAccess.GetApplicantById(applicantId).Name;

                    var formQuestions = from d in _formQuestionsDataAccess.GetAllQuestions(FormType.PhoneInterview)
                                        orderby d.OrderInForm
                                        select d;

                    var phoneInterview = new PhoneInterview
                    {
                        ApplicantId = (int)applicantId
                    };

                    var viewModel = new PhoneInterviewViewModel
                    {
                        FormQuestions = formQuestions.ToList(),
                        PhoneInterview = phoneInterview
                    };

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View(viewModel);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        public ActionResult Responses()
        {
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var phoneInterviewResponses = from d in _phoneInterviewDataAccess.GetAllPhoneInterviews()
                                                  orderby d.SubmissionDate descending
                                                  select d;

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View(phoneInterviewResponses.ToList());
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");

        }

        public ActionResult Details(int? applicantId)
        {
            if (applicantId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing() || _rolesManager.IsManager())
                {
                    var applicant = new Applicant();
                    if (!_rolesManager.GetApplicantDetails(_applicantsDataAccess, ref applicant, applicantId))
                    {
                        ViewBag.ErrorMsg = "You are not authorized to view this page";
                        return View("Error");
                    }
                    if (applicant == null)
                    {
                        return HttpNotFound();
                    }

                    var phoneInterviewResponse = _phoneInterviewDataAccess
                                                    .GetPhoneInterviewByApplicantId((int)applicantId);

                    if (phoneInterviewResponse == null)
                        return HttpNotFound();

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View(phoneInterviewResponse);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(PhoneInterviewViewModel phoneInterviewViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMsg = "Failed to create Phone Interview. Check the correctness of your input format";
                return View("Error");
            }

            phoneInterviewViewModel.PhoneInterview.SubmissionDate = DateTime.Now;
            _phoneInterviewDataAccess.CreatePhoneInterview(phoneInterviewViewModel.PhoneInterview);

            var applicant = _applicantsDataAccess.GetApplicantById(phoneInterviewViewModel.PhoneInterview.ApplicantId);
            if (phoneInterviewViewModel.PhoneInterview.Status == PhoneIntervewStatus.Passed)
            {
                _applicantsDataAccess.EditApplicantStatus(applicant, ApplicantStatus.PhoneInterviewPassed);
            }
            else if (phoneInterviewViewModel.PhoneInterview.Status == PhoneIntervewStatus.NotPassed)
            {
                _applicantsDataAccess.EditApplicantStatus(applicant, ApplicantStatus.PhoneInterviewRejected);
            }

            return RedirectToAction("Index", "Applicants");
        }
    }
}