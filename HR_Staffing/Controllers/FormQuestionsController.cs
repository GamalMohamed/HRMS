using HR_Staffing.Models;
using HR_Staffing.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using HR_Staffing.BusinessLogicLayer;
using HR_Staffing.DataAccessLayer;

namespace HR_Staffing.Controllers
{
    [Authorize]
    public class FormQuestionsController : Controller
    {
        private readonly FormQuestionsDataAccess _formQuestionsDataAccess = new FormQuestionsDataAccess();
        private readonly RolesManager _rolesManager = new RolesManager();

        private string GetCurrentUserEmail()
        {
            return ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }


        // GET: FormQuestions
        public ActionResult Index(string formTypeString)
        {
            FormType formType;
            if (!Enum.TryParse(formTypeString, out formType))
            {
                ViewBag.ErrorMsg = "No such Url exists";
                return View("Error");
            }
            
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var viewModel = new FormQuestionsViewModel
                    {
                        FormType = formType,
                        FormQuestions = _formQuestionsDataAccess.GetAllQuestions(formType)
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

        public ActionResult New(string formTypeString)
        {
            FormType formType;
            if (formTypeString == null || !Enum.TryParse(formTypeString, out formType))
            {
                ViewBag.ErrorMsg = "No such Url exists";
                return View("Error");
            }

            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var formQuestion = new FormQuestion
                    {
                        FormType = formType
                    };

                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View("QuestionForm", formQuestion);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(FormQuestion formQuestion)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMsg = "Something went wrong with your submission. Please try again later";
                return View("Error");
            }

            if (formQuestion.Id == 0)
            {
                _formQuestionsDataAccess.CreateQuestion(formQuestion);
            }
            else
            {
                _formQuestionsDataAccess.EditQuestion(formQuestion);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var formQuestion = _formQuestionsDataAccess.GetQuestionById(id);

                    if (formQuestion == null)
                        return HttpNotFound();

                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    return View("QuestionForm", formQuestion);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        public ActionResult Delete(int id)
        {
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var formQuestion = _formQuestionsDataAccess.GetQuestionById(id);

                    if (formQuestion == null)
                        return HttpNotFound();

                    _formQuestionsDataAccess.DeleteQuestion(formQuestion);

                    return RedirectToAction("Index");
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");

        }

        public ActionResult EditOrder(int id, FormType formType, int currentOrder, string direction)
        {
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    var otherQuestionOrder = 0;
                    if (direction == "UP") { otherQuestionOrder = currentOrder - 1; }
                    else { otherQuestionOrder = currentOrder + 1; }

                    _formQuestionsDataAccess.SwapQuestionsOrder(id, formType, currentOrder, otherQuestionOrder);

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