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
    public class ApplicantsQuestionsController : Controller
    {
        private readonly ApplicantsQuestionsDataAccess _applicantsQuestionsDataAccess = new ApplicantsQuestionsDataAccess();
        private readonly ApplicantsDataAccess _applicantsDataAccess = new ApplicantsDataAccess();
        private readonly RolesManager _rolesManager = new RolesManager();

        private string GetCurrentUserEmail()
        {
            return ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }

        // GET: ApplicantsQuestions
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
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View(_applicantsQuestionsDataAccess.GetAllQuestions((int)applicantId));
                }
                
                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
 
        }

        public ActionResult New(int? applicantId)
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

                    var applicantQuestion = new ApplicantQuestion
                    {
                        ApplicantId = (int)applicantId
                    };

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View("QuestionForm", applicantQuestion);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ApplicantQuestion applicantQuestion)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            if (applicantQuestion.Id == 0)
            {
                _applicantsQuestionsDataAccess.CreateQuestion(applicantQuestion);
            }
            else
            {
                _applicantsQuestionsDataAccess.EditQuestion(applicantQuestion);
            }

            return RedirectToAction("Index");
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
                    var applicantQuestion = _applicantsQuestionsDataAccess.GetQuestionById(id);

                    if (applicantQuestion == null)
                        return HttpNotFound();

                    ViewBag.ApplicantName = _applicantsDataAccess.GetApplicantById(applicantQuestion.ApplicantId).Name;
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View("QuestionForm", applicantQuestion);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
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
                    var applicantQuestion = _applicantsQuestionsDataAccess.GetQuestionById(id);

                    if (applicantQuestion == null)
                        return HttpNotFound();

                    _applicantsQuestionsDataAccess.DeleteQuestion(applicantQuestion);

                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        public ActionResult EditOrder(int? id, int? applicantId, int? currentOrder, string direction)
        {
            if (id == null || applicantId ==null || currentOrder == null || direction==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var otherQuestionOrder = 0;
                    if (direction == "UP") { otherQuestionOrder = (int)currentOrder - 1; }
                    else { otherQuestionOrder = (int)currentOrder + 1; }

                    _applicantsQuestionsDataAccess.SwapQuestionsOrder((int)id, (int)applicantId,
                        (int)currentOrder, otherQuestionOrder);

                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }
    }
}