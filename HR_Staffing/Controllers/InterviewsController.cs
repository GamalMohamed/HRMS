using HR_Staffing.DataAccessLayer;
using HR_Staffing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using HR_Staffing.BusinessLogicLayer;
using HR_Staffing.ViewModels;

namespace HR_Staffing.Controllers
{
    [Authorize]
    public class InterviewsController : Controller
    {
        private readonly InterviewsDataAccess _interviewsDataAccess = new InterviewsDataAccess();
        private readonly FormQuestionsDataAccess _formQuestionsDataAccess = new FormQuestionsDataAccess();
        private readonly FeedbacksDataAccess _feedbacksDataAccess = new FeedbacksDataAccess();
        private readonly ApplicantsDataAccess _applicantsDataAccess = new ApplicantsDataAccess();
        private readonly RolesManager _rolesManager = new RolesManager();

        private string GetCurrentUserEmail()
        {
            return ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }

        // GET: Interviews
        public ActionResult Index()
        {
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View(_interviewsDataAccess.GetAllInterviews());
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        public ActionResult InterviewDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var interview = _interviewsDataAccess.GetInterviewById(id);

                    if (interview == null)
                        return HttpNotFound();

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View("Details", interview);

                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }
        
        public ActionResult New(int applicantId, InterviewType interviewType)
        {
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var interview = new Interview
                    {
                        ApplicantId = applicantId,
                        InterviewType = interviewType
                    };

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View("InterviewForm", interview);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var interview = _interviewsDataAccess.GetInterviewById(id);

                    if (interview == null)
                        return HttpNotFound();

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View("InterviewForm", interview);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Interview interview)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMsg = "Failed to create employee. Check the correctness of your input format";
                return View("ErrorPartial");
            }

            if (interview.Id == 0)
            {
                _interviewsDataAccess.CreateInterview(interview);
            }
            else
            {
                _interviewsDataAccess.EditInterview(interview);
            }

            return RedirectToAction("Index", "Applicants");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var interview = _interviewsDataAccess.GetInterviewById(id);

                    if (interview == null)
                        return HttpNotFound();
                    if (interview.Feedback==null)
                    {
                        _interviewsDataAccess.DeleteInterview(interview);
                        return RedirectToAction("Index");
                    }

                    ViewBag.ErrorMsg = "Can't delete interview as there's dependant data on it in the system";
                    return View("ErrorPartial");

                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        public ActionResult Details(int? applicantId, InterviewType interviewType)
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

                    var interview = _interviewsDataAccess.GetInterviewByApplicantId(applicantId, interviewType);

                    if (interview == null)
                        return HttpNotFound();

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View(interview);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        public ActionResult Feedback(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsManager() || _rolesManager.IsStaffing())
                {

                    var formQuestions = _formQuestionsDataAccess.GetAllQuestions(FormType.FeedbackForm);
                    var interview = _interviewsDataAccess.GetInterviewById(id);

                    if (interview.InterviewType == InterviewType.HR && _rolesManager.IsManager())
                    {
                        ViewBag.ErrorMsg = "You are not authorized to view this page";
                        return View("Error");
                    }
                    var feedback = new Feedback
                    {
                        InterviewId = (int)id,
                        Interview = interview,
                        FeedbackContents = new List<FeedbackContent>()
                    };
                    var viewModel = new FeedbackFormViewModel
                    {
                        FormQuestions = formQuestions.ToList(),
                        Feedback = feedback
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveFeedback(FeedbackFormViewModel feedbackFormViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            var interview = feedbackFormViewModel.Feedback.Interview;
            feedbackFormViewModel.Feedback.Interview = null;

            feedbackFormViewModel.Feedback.SubmissionDate = DateTime.Now;
            _feedbacksDataAccess.CreateFeedback(feedbackFormViewModel.Feedback);

            var applicant = _applicantsDataAccess.GetApplicantById(interview.ApplicantId);
            if (interview.InterviewType == InterviewType.HR)
            {
                if (feedbackFormViewModel.Feedback.Hired)
                {
                    _applicantsDataAccess.EditApplicantStatus(applicant, ApplicantStatus.HRPassed);
                }
                else
                {
                    _applicantsDataAccess.EditApplicantStatus(applicant, ApplicantStatus.HRRejected);
                }
            }
            else if (interview.InterviewType == InterviewType.Technical)
            {
                if (feedbackFormViewModel.Feedback.Hired)
                {
                    _applicantsDataAccess.EditApplicantStatus(applicant, ApplicantStatus.TechPassed);
                }
                else
                {
                    _applicantsDataAccess.EditApplicantStatus(applicant, ApplicantStatus.TechRejected);
                }
            }
            else if (interview.InterviewType == InterviewType.HiringManager)
            {
                if (feedbackFormViewModel.Feedback.Hired)
                {
                    _applicantsDataAccess.EditApplicantStatus(applicant, ApplicantStatus.HiringManagerAccepted);
                }
                else
                {
                    _applicantsDataAccess.EditApplicantStatus(applicant, ApplicantStatus.HiringManagerRejected);
                }
            }

            return RedirectToAction("Details", new { applicantId = interview.ApplicantId, interviewType = interview.InterviewType });
        }
    }
}