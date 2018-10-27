using System.Collections.Generic;
using CoEX_HRMS.Models;
using CoEX_HRMS.DataAccessLayer;

namespace CoEX_HRMS.BusinessLogicLayer
{
    public class RolesManager
    {
        public Employee LoggedInEmployee { get; set; }

        public bool SetCurrentEmployee(EmployeeDataAccess dal, string email)
        {
            LoggedInEmployee = dal.GetEmployeeByEmail(email);
            return LoggedInEmployee != null;
        }

        public string IdentifyRole()
        {
            switch (LoggedInEmployee.AccessLevel)
            {
                case AccessLevel.FullAccess:
                    return "FullAccess";
                case AccessLevel.FullView:
                    return "FullView";
                case AccessLevel.TeamView:
                    return "TeamView";
                case AccessLevel.EmployeeView:
                    return "EmployeeView";
                default:
                    return "Error";
            }
        }

        public List<Employee> ListRespectiveActiveEmployees(EmployeeDataAccess dal)
        {
            switch (IdentifyRole())
            {
                case "FullAccess":
                case "FullView":
                    return dal.GetActiveEmployees();

                case "TeamView":
                    return dal.GetActiveEmployeesByReportingManager(LoggedInEmployee.Name);

                default:
                    return null;
            }
        }

        public List<Employee> ListRespectiveResignedEmployees(EmployeeDataAccess dal)
        {
            switch (IdentifyRole())
            {
                case "FullAccess":
                case "FullView":
                    return dal.GetResignedEmployees();

                case "TeamView":
                    return dal.GetResignedEmployeesByReportingManager(LoggedInEmployee.Name);

                default:
                    return null;
            }
        }

        public bool GetEmployeeDetails(EmployeeDataAccess dal, ref Employee employee, int? id)
        {
            switch (IdentifyRole())
            {
                case "FullAccess":
                case "FullView":
                    employee = dal.GetEmployeeById(id);
                    return true;
                case "TeamView":
                    var emp = dal.GetEmployeeById(id);
                    if (emp != null)
                    {
                        var empsInTeam = dal.GetActiveEmployeesByReportingManager(LoggedInEmployee.Name);
                        if (empsInTeam.Exists(e => e.Id == emp.Id))
                            employee = emp;
                    }
                    return employee.Id != 0;
                default:
                    return false;
            }
        }

    }
}