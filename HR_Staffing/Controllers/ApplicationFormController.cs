using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using HR_Staffing.BusinessLogicLayer;
using HR_Staffing.DataAccessLayer;
using HR_Staffing.Models;
using HR_Staffing.Services;
using HR_Staffing.ViewModels;


namespace HR_Staffing.Controllers
{
    [Authorize]
    public class ApplicationFormController : Controller
    {
        private readonly ApplicantsDataAccess _applicantsDataAccess = new ApplicantsDataAccess();
        private readonly ApplicantsQuestionsDataAccess _applicantsQuestionsDataAccess = new ApplicantsQuestionsDataAccess();
        private readonly FormResponsesDataAccess _formResponsesDataAccess = new FormResponsesDataAccess();
        private readonly BlobServices _blobServices = new BlobServices();
        private readonly RolesManager _rolesManager = new RolesManager();

        private string GetCurrentUserEmail()
        {
            return ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }

        // GET: ApplicationForm
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (TempData.ContainsKey("accessToken"))
            {
                if (TempData["accessToken"].ToString() == "Granted")
                {
                    var applicantId = Convert.ToInt32(TempData["applicantId"]);
                    var applicantQuestions = _applicantsQuestionsDataAccess.GetAllQuestions(applicantId);

                    var formResponse = new FormResponse
                    {
                        ApplicantId = applicantId,
                        FormResponseContents = new List<FormResponseContent>()
                    };
                    var viewModel = new ApplicationFormViewModel
                    {
                        ApplicantQuestions = applicantQuestions,
                        FormResponse = formResponse
                    };
                    return View(viewModel);
                }
            }
            return RedirectToAction("Auth");
        }

        public ActionResult Responses()
        {
            if (_rolesManager.SetCurrentEmployee(GetCurrentUserEmail()))
            {
                if (_rolesManager.IsStaffing())
                {
                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View(_formResponsesDataAccess.GetAllResponses());
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ApplicationFormViewModel applicationFormViewModel, HttpPostedFileBase uploadFile)
        {
            ////I commented it coz it will always give an error as resume field is required
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.ErrorMsg = "Something went wrong with your submission. Please try again later";
            //    return View("Error");
            //}

            applicationFormViewModel.FormResponse.SubmissionDate = DateTime.Now;
            applicationFormViewModel.FormResponse.Status = ApplicationFormResponseStatus.NotViewed;
            applicationFormViewModel.FormResponse.ApplicantResume = "#"; // HACK: coz resume is required and not yet created
            _formResponsesDataAccess.CreateResponse(applicationFormViewModel.FormResponse);

            var applicant = _applicantsDataAccess.GetApplicantById(applicationFormViewModel.FormResponse.ApplicantId);
            if (applicant != null)
            {
                applicant.Status = ApplicantStatus.ApplicationSubmitted;
                if (uploadFile != null)
                {
                    var serverPath = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }
                    var extension = Path.GetExtension(uploadFile.FileName)?.ToLower();
                    if (extension != ".pdf")
                    {
                        ViewBag.ErrorMsg = "Invalid file type - Need to be PDF";
                        return View("Error");
                    }
                    var filePath = Path.Combine(serverPath + Path.GetFileName(uploadFile.FileName));
                    uploadFile.SaveAs(filePath);

                    var applicantName = string.Join("", applicant.Name.Split(' '));
                    var fileName = applicant.Id.ToString() + '.' + applicantName + ".pdf";
                    applicant.FormResponse.ApplicantResume = _blobServices.BlobUrl + "resumes/" + fileName;
                    _applicantsDataAccess.SaveChangesToDb();

                    _blobServices.BlobMediaUpload(fileName, filePath, "resumes");


                    // Delete file from server after finishing 
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(filePath);
                }
                _applicantsDataAccess.SaveChangesToDb();
            }
            else
            {
                ViewBag.ErrorMsg = "Can't find applicant data in our system to submit it. " +
                                   "Please contact system administrator if you think this is a mistake";
                return View("Error");
            }




            return View("Success");
        }

        
        public ActionResult ApplicantResponse(int? applicantId)
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

                    var formResponse = _formResponsesDataAccess.GetResponseByApplicantId(applicantId);

                    if (formResponse == null)
                        return HttpNotFound();

                    if (formResponse.Status == ApplicationFormResponseStatus.NotViewed)
                    {
                        _formResponsesDataAccess.EditResponseStatus(formResponse, ApplicationFormResponseStatus.Viewed);
                    }

                    formResponse.FormResponseContents = formResponse.FormResponseContents
                        .OrderBy(x => x.ApplicantQuestion.OrderInForm).ToList();


                    ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.ProfilePic;
                    ViewBag.Staffing = _rolesManager.IsStaffing();
                    return View(formResponse);
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditResponseStatus(int? applicantId, ApplicationFormResponseStatus status)
        {
            if (applicantId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var formResponse = _formResponsesDataAccess.GetResponseByApplicantId(applicantId);

            if (formResponse == null)
                return HttpNotFound();

            _formResponsesDataAccess.EditResponseStatus(formResponse, status);

            var applicant = _applicantsDataAccess.GetApplicantById(applicantId);
            if (status == ApplicationFormResponseStatus.Passed)
            {
                _applicantsDataAccess.EditApplicantStatus(applicant, ApplicantStatus.ScreeningPassed);
            }
            else if (status == ApplicationFormResponseStatus.NotPassed)
            {
                _applicantsDataAccess.EditApplicantStatus(applicant, ApplicantStatus.ScreeningRejected);
            }
            else
            {
                _applicantsDataAccess.EditApplicantStatus(applicant, ApplicantStatus.ApplicationSubmitted);
            }

            return RedirectToAction("Index", "Applicants");
        }

        // GET: Auth
        [AllowAnonymous]
        public ActionResult Auth()
        {
            return View(new FormAuthViewModel());
        }

        // POST: Auth
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Auth(FormAuthViewModel model)
        {
            var applicant = _applicantsDataAccess.GetApplicantByPasscode(model.Passcode);

            if (applicant == null || applicant.Status != ApplicantStatus.Invited)
            {
                TempData["accessToken"] = "Denied";
                ViewBag.ErrorMsg = "Passcode is wrong or already used.";
                return View("Error");
            }
            else
            {
                TempData["accessToken"] = "Granted";
                TempData["applicantId"] = applicant.Id;
                return RedirectToAction("Index");
            }

        }

    }
}

