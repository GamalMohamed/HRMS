using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using CoEX_HRMS.Models;
using CoEX_HRMS.DataAccessLayer;
using CoEX_HRMS.Models;
using CoEX_HRMS.Services;
using CoEX_HRMS.Utils;
using System.Web.UI;
using CoEX_HRMS.BusinessLogicLayer;

namespace CoEX_HRMS.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly EmployeeDataAccess _employeeDataAccess = new EmployeeDataAccess();
        private readonly ProfileDataAccess _profileDataAccess = new ProfileDataAccess();
        private readonly BlobServices _blobServices = new BlobServices();
        private readonly RolesManager _rolesManager = new RolesManager();


        private string GetCurrentUserEmail()
        {
            return ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;
        }

        private Employee GetCurrentEmployee()
        {
            var employee = _employeeDataAccess.GetEmployeeByEmail(GetCurrentUserEmail());
            return employee;
        }

        // GET: Profile
        public ActionResult Index()
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                if (_rolesManager.LoggedInEmployee.Profile == null)
                {
                    _rolesManager.LoggedInEmployee.Profile = new Profile {Id = _rolesManager.LoggedInEmployee.Id};
                    _profileDataAccess.CreateProfile(_rolesManager.LoggedInEmployee.Profile);
                    return RedirectToAction("Index"); //HACK: reload to avoid ununderstandable Null exception!!
                }

                ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                ViewBag.Access = "FullAccess";
                return View("Details", _rolesManager.LoggedInEmployee);
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }
        
        // GET: Profile/Details/5
        public ActionResult Details(int id)
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                var accessLevel = _rolesManager.IdentifyRole();
                if (accessLevel == "FullAccess" || accessLevel == "FullView" || accessLevel == "TeamView")
                {
                    var employee = new Employee();
                    if (!_rolesManager.GetEmployeeDetails(_employeeDataAccess, ref employee, id))
                    {
                        ViewBag.ErrorMsg = "You are not authorized to view this page";
                        return View("Error");
                    }
                    if (employee != null)
                    {
                        if (employee.Profile == null)
                        {
                            employee.Profile = new Profile { Id = employee.Id };
                            _profileDataAccess.CreateProfile(employee.Profile);

                            ViewBag.Access = accessLevel;
                            return RedirectToAction("Details", id); // HACK: reload to avoid ununderstandable Null exception!!
                        }

                        ViewBag.Access = accessLevel;
                        ViewBag.ProfilePic = _rolesManager.LoggedInEmployee.Profile.ProfilePic;
                        return View(employee);
                    }

                    ViewBag.ErrorMsg = "No such page exists.";
                    return View("Error");
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }
        
        // GET: Profile/Delete/5
        public ActionResult Delete(int? id)
        {
            if (_rolesManager.SetCurrentEmployee(_employeeDataAccess, GetCurrentUserEmail()))
            {
                var accessLevel = _rolesManager.IdentifyRole();
                if (accessLevel == "FullAccess")
                {
                    if (id == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    var profile = _profileDataAccess.GetProfileById(id);
                    if (profile == null)
                    {
                        return HttpNotFound();
                    }

                    _profileDataAccess.DeleteProfile(profile);
                    return RedirectToAction("Index", "Employees");
                }

                ViewBag.ErrorMsg = "You are not authorized to view this page";
                return View("Error");
            }

            ViewBag.ErrorMsg = "You are not registered on our system. Plz contact the system administrator if u think this is wrong.";
            return View("Error");
        }
        
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            var profileId = Convert.ToInt32(Request.Form["profileId"]);
            if (file?.ContentLength > 0)
            {
                var extension = Path.GetExtension(file.FileName);
                if (extension?.ToLower() == ".pdf" || extension?.ToLower() == ".png"
                    || extension?.ToLower() == ".jpg" || extension?.ToLower() == ".jpeg")
                {
                    var imageName = Path.GetFileNameWithoutExtension(file.FileName);
                    imageName = Path.Combine(Server.MapPath("~/Images") + imageName + extension);
                    file.SaveAs(imageName);

                    Upload(imageName, extension, profileId);

                    // Delete file from server after finishing 
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    System.IO.File.Delete(imageName);
                }
                else
                {
                    ViewBag.ErrorMsg = "File type not supported for upload. Available formats: .pdf, .png, .jpg, .jpeg";
                    return View("Error");
                }
                
            }


            // Redirect respectively
            var profileEmail = _profileDataAccess.GetProfileById(profileId).Employee.Email;
            var currentUserEmail = GetCurrentEmployee().Email;
            if (profileEmail != currentUserEmail)
            {
                return RedirectToAction("Details", "Profile", new { id = profileId });
            }
            else
            {
                return RedirectToAction("Index", "Profile");
            }
        }

        public void Upload(string path, string extension, int id)
        {
            var val = Request.Form["upload"];

            var employeeProfile = _profileDataAccess.GetProfileById(id);
            if (employeeProfile == null)
                return;

            var imageName = "";
            var locPath = _blobServices.BlobUrl;
            var employeeName = String.Join("", employeeProfile.Employee.Name.Split(' '));
            string type = "";
            switch (val)
            {
                case "SubmitID":
                    type = "idcards";
                    imageName = employeeProfile.Id.ToString() + '.' + employeeName
                        + "_ID" + extension;
                    employeeProfile.IdCard = locPath + type + "/" + imageName;
                    break;
                case "SubmitCertificate":
                    type = "birthcertificates";
                    imageName = employeeProfile.Id.ToString() + '.' + employeeName
                        + "_BirthCertificate" + extension;
                    employeeProfile.BirthCertificate = locPath + type + "/" + imageName;
                    break;
                case "SubmitPassport":
                    type = "passports";
                    imageName = employeeProfile.Id.ToString() + '.' + employeeName
                        + "_Passport" + extension;
                    employeeProfile.Passport = locPath + type + "/" + imageName;
                    break;
                case "SubmitMilitary":
                    type = "militarycertificates";
                    imageName = employeeProfile.Id.ToString() + '.' + employeeName
                        + "_MilitaryCertificate" + extension;
                    employeeProfile.MilitaryCertificate = locPath + type + "/" + imageName;
                    break;
                case "SubmitGraduation":
                    type = "graduationcertificates";
                    imageName = employeeProfile.Id.ToString() + '.' + employeeName
                        + "_GraduationCertificate" + extension;
                    employeeProfile.GraduationCertificate = locPath + type + "/" + imageName;
                    break;
                case "SubmitProfilePic":
                    type = "profilepics";
                    imageName = employeeProfile.Id.ToString() + '.' + employeeName
                        + "_ProfilePic" + extension;
                    employeeProfile.ProfilePic = locPath + type + "/" + imageName;
                    break;
                case "SubmitExtraDoc":
                    type = "extradocs";
                    imageName = employeeProfile.Id.ToString() + '.' + employeeName
                        + "_extraDoc" + extension;
                    employeeProfile.ExtraDocument = locPath + type + "/" + imageName;
                    break;
            }

            _profileDataAccess.SaveChangesToDb();
            _blobServices.BlobImageUpload(imageName, path, type);

        }

        public void Del(string type, string extension, string containerName, int id)
        {
            var profile = _profileDataAccess.GetProfileById(id);
            if (profile == null)
            {
                return;
            }

            var employeeName = String.Join("", profile.Employee.Name.Split(' '));
            var imageName = profile.Id.ToString() + '.' + employeeName + '_' + type + extension;
            _blobServices.BlobDelete(imageName, containerName);

            switch (type)
            {
                case "ID":
                    profile.IdCard = null;
                    break;
                case "BirthCertificate":
                    profile.BirthCertificate = null;
                    break;
                case "Passport":
                    profile.Passport = null;
                    break;
                case "MilitaryCertificate":
                    profile.MilitaryCertificate = null;
                    break;
                case "GraduationCertificate":
                    profile.GraduationCertificate = null;
                    break;
                case "ProfilePic":
                    profile.ProfilePic = null;
                    break;
                case "extraDoc":
                    profile.ExtraDocument = null;
                    break;
            }
            _profileDataAccess.SaveChangesToDb();
        }

        public ActionResult DeleteImage(string type, int id)
        {
            var employee = _employeeDataAccess.GetEmployeeById(id);
            switch (type)
            {
                case "ID":
                    Del("ID",
                        employee.Profile.GetFileExtension(employee.Profile.IdCard),
                        "idcards", id);
                    break;
                case "BirthCertificate":
                    Del("BirthCertificate",
                        employee.Profile.GetFileExtension(employee.Profile.BirthCertificate),
                        "birthcertificates", id);
                    break;
                case "passport":
                    Del("Passport",
                        employee.Profile.GetFileExtension(employee.Profile.Passport),
                        "passports", id);
                    break;
                case "MilitaryCertificate":
                    Del("MilitaryCertificate",
                        employee.Profile.GetFileExtension(employee.Profile.MilitaryCertificate),
                        "militarycertificates", id);
                    break;
                case "GraduationCertificate":
                    Del("GraduationCertificate",
                        employee.Profile.GetFileExtension(employee.Profile.GraduationCertificate),
                        "graduationcertificates", id);
                    break;
                case "ProfilePic":
                    Del("ProfilePic",
                        employee.Profile.GetFileExtension(employee.Profile.ProfilePic),
                        "profilepics", id);
                    break;
                case "extraDoc":
                    Del("extraDoc",
                        employee.Profile.GetFileExtension(employee.Profile.ExtraDocument),
                        "extradocs", id);
                    break;
            }

            // Redirect respectively
            var profileEmail = _profileDataAccess.GetProfileById(id).Employee.Email;
            var currentUserEmail = GetCurrentEmployee().Email;
            if (profileEmail != currentUserEmail)
            {
                return RedirectToAction("Details", "Profile", new { id });
            }
            else
            {
                return RedirectToAction("Index", "Profile");
            }
        }

        //donwload function to check the image need to be download
        public void DownloadSingleImage(string type)
        {
            var employe = GetCurrentEmployee();
            switch (type)
            {
                case "ProfilePic":
                    if ((employe.Profile.ProfilePic != null) && IsExist(employe.Profile.ProfilePic))
                        DownloadAct(employe.Profile.ProfilePic, employe.Name + "_profile_Image" + Path.GetExtension(employe.Profile.ProfilePic));
                    break;
                case "BirthCertificate":
                    if ((employe.Profile.BirthCertificate != null) && IsExist(employe.Profile.BirthCertificate))
                        DownloadAct(employe.Profile.BirthCertificate, employe.Name + "_profile_Image" + Path.GetExtension(employe.Profile.BirthCertificate));
                    break;
                case "MilitaryCertificate":
                    if ((employe.Profile.MilitaryCertificate != null) && IsExist(employe.Profile.IdCard))
                        DownloadAct(employe.Profile.MilitaryCertificate, employe.Name + "_profile_Image" + Path.GetExtension(employe.Profile.MilitaryCertificate));
                    break;
                case "IdCard":
                    if ((employe.Profile.IdCard != null) && IsExist(employe.Profile.IdCard))
                        DownloadAct(employe.Profile.IdCard, employe.Name + "_profile_Image" + Path.GetExtension(employe.Profile.IdCard));
                    else
                    {
                        Response.Write("<script>alert('No Id card ')</script>");

                    }
                    break;
                case "Passport":
                    if ((employe.Profile.Passport != null) && IsExist(employe.Profile.Passport))
                        DownloadAct(employe.Profile.Passport, employe.Name + "_profile_Image" + Path.GetExtension(employe.Profile.Passport));
                    break;
                case "GraduationCertificate":
                    if ((employe.Profile.GraduationCertificate != null) && IsExist(employe.Profile.GraduationCertificate))
                        DownloadAct(employe.Profile.GraduationCertificate, employe.Name + "_profile_Image" + Path.GetExtension(employe.Profile.GraduationCertificate));
                    break;
                default:
                    break;
            }
        }

        //download dialog show
        public void DownloadAct(string url, string name)
        {
            WebClient client = new WebClient();
            byte[] data = client.DownloadData(new Uri(url));
            HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "image/*";
            response.AppendHeader("Content-Disposition", $"attachment; filename={name}");
            response.BinaryWrite(data);
            response.End();
        }

        //check if url is exist
        public bool IsExist(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";

            var response = (HttpWebResponse)request.GetResponse();

            return response.StatusCode == HttpStatusCode.OK;
        }

        protected override void Dispose(bool disposing)
        {
            _profileDataAccess.DbDispose();
        }
    }
}
