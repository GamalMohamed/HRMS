using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CoEX_HRMS.DataAccessLayer;
using CoEX_HRMS.Models;

namespace CoEX_HRMS.Controllers
{
    public class EmployeesApiController : ApiController
    {
        private readonly EmployeeDataAccess _employeeDataAccess = new EmployeeDataAccess();

        // GET: api/EmployeesApi
        public IHttpActionResult GetEmployees(bool reportingManagers)
        {
            if (reportingManagers)
            {
                var reportingManagerslist = _employeeDataAccess.GetAllReportingManagers();

                reportingManagerslist.AddRange(_employeeDataAccess.GetEmployeeByAccessLevel(AccessLevel.TeamView)
                                            .Select(n => n.Name)
                                            .Where(m => reportingManagerslist.All(y => y != m)));

                return Ok(reportingManagerslist);
            }

            return NotFound();
        }


        // GET: api/EmployeesApi/m@m.com
        public IHttpActionResult GetEmployee(string email)
        {
            if (email == null)
                return NotFound();

            var employee = _employeeDataAccess.GetEmployeeByEmail(email);
            if (employee == null)
            {
                return NotFound();
            }

            var employeeinfo = new List<string>()
            {
                employee.Name,employee.Email,employee.AccessLevel.ToString(),
                employee.Role.Title,employee.Profile.ProfilePic
            };

            return Ok(employeeinfo);
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