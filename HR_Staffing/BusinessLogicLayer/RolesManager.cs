using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using HR_Staffing.DataAccessLayer;
using HR_Staffing.Models;

namespace HR_Staffing.BusinessLogicLayer
{
    public class Employee
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string AccessLevel { get; set; }

        public string Role { get; set; }

        public string ProfilePic { get; set; }
    }

    public class RolesManager
    {
        public const string ApiBaseUri = "https://coex-hrms.azurewebsites.net/api/";

        public Employee LoggedInEmployee { get; set; }

        public RolesManager()
        {
            LoggedInEmployee = new Employee();
        }

        public bool SetCurrentEmployee(string email)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUri);
                var responseTask = client.GetAsync("EmployeesApi?email=" + email);
                responseTask.Wait();

                var result = responseTask.Result;
                if (!result.IsSuccessStatusCode)
                    return false;

                var readTask = result.Content.ReadAsAsync<List<string>>();
                readTask.Wait();

                var employeeInfo = readTask.Result;
                LoggedInEmployee.Name = employeeInfo[0];
                LoggedInEmployee.Email = employeeInfo[1];
                LoggedInEmployee.AccessLevel = employeeInfo[2];
                LoggedInEmployee.Role = employeeInfo[3];
                LoggedInEmployee.ProfilePic = employeeInfo[4];

                return true;
            }
        }

        public bool IsStaffing()
        {
            return LoggedInEmployee.Role.Contains("Staffing");
        }

        public bool IsManager()
        {
            return LoggedInEmployee.AccessLevel == "TeamView";
        }

        public List<Applicant> ListRespectiveApplicants(ApplicantsDataAccess applicantsDataAccess)
        {
            if (IsStaffing())
            {
                return applicantsDataAccess.GetAllApplicants();
            }
            else if (IsManager())
            {
                return applicantsDataAccess.GetApplicantsByHiringManager(LoggedInEmployee.Name);
            }

            return null;
        }


        public bool GetApplicantDetails(ApplicantsDataAccess applicantsDataAccess, ref Applicant applicant, int? id)
        {
            if (IsStaffing())
            {
                applicant = applicantsDataAccess.GetApplicantById(id);
                return true;
            }
            else if (IsManager())
            {
                var app = applicantsDataAccess.GetApplicantById(id);
                if (app != null)
                {
                    var appsInTeam = applicantsDataAccess.GetApplicantsByHiringManager(LoggedInEmployee.Name);
                    if (appsInTeam.Exists(a => a.Id == app.Id))
                    {
                        applicant = app;
                    }
                    return applicant.Id != 0;
                }
            }

            return false;
        }

    }
}