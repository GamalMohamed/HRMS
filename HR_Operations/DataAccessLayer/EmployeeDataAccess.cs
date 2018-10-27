using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CoEX_HRMS.Models;
using CoEX_HRMS.Models;

namespace CoEX_HRMS.DataAccessLayer
{
    public class EmployeeDataAccess
    {
        private readonly ApplicationDbContext _db;

        public EmployeeDataAccess()
        {
            _db = new ApplicationDbContext();
        }

        public void CreateEmployee(Employee employee)
        {
            _db.Employees.Add(employee);
            _db.SaveChanges();
        }

        public void EditEmployee(Employee employee)
        {
            _db.Entry(employee).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteEmployee(Employee employee)
        {
            _db.Profiles.Remove(employee.Profile);
            _db.Employees.Remove(employee);
            _db.SaveChanges();
        }

        public List<Employee> GetAllEmployees()
        {
            return _db.Employees.ToList();
        }

        public List<Employee> GetActiveEmployees()
        {
            return _db.Employees.Where(e => e.Status == Status.Active).ToList();
        }

        public List<Employee> GetResignedEmployees()
        {
            return _db.Employees.Where(e => e.Status == Status.Resigned).ToList();
        }

        public Employee GetEmployeeById(int? id)
        {
            return _db.Employees.Find(id);
        }

        public Employee GetEmployeeByPhoneNumber(string phoneNumber)
        {
            return _db.Employees.FirstOrDefault(e => e.PhoneNumber.ToLower() == phoneNumber.ToLower());
        }

        public Employee GetEmployeeByEmail(string email)
        {
            return _db.Employees.FirstOrDefault(e => e.Email.ToLower() == email.ToLower());
        }

        public List<Employee> GetEmployeesByName(string name)
        {
            return _db.Employees.Where(e => e.Name.ToLower() == name.ToLower()).ToList();
        }

        public List<Employee> GetEmployeesByGender(string gender)
        {
            return _db.Employees.Where(e => e.Gender == gender).ToList();
        }

        public List<Employee> GetEmployeesByGraduationYear(string graduationYear)
        {
            return _db.Employees.Where(e => e.GraduationYear == graduationYear).ToList();
        }

        public List<Employee> GetEmployeesByMajorUniversity(string majorUniversity)
        {
            return _db.Employees.Where(e => e.MajorUniversity.ToLower() == majorUniversity.ToLower()).ToList();
        }

        public List<Employee> GetEmployeesByBirthDateInterval(DateTime startDate, DateTime endDate)
        {
            return _db.Employees.Where(e => e.BirthDate.Value > startDate && e.BirthDate.Value < endDate).ToList();
        }

        public Employee GetEmployeeByWorkload(Workload workload)
        {
            return _db.Employees.FirstOrDefault(e => e.Workload.Id == workload.Id);
        }

        public List<Employee> GetActiveEmployeesByWorkload(Workload workload)
        {
            return _db.Employees.Where(e => e.Workload.Id == workload.Id && e.Status == Status.Active).ToList();
        }

        public List<Employee> GetResignedEmployeesByWorkload(Workload workload)
        {
            return _db.Employees.Where(e => e.Workload.Id == workload.Id 
                                            && e.Status == Status.Resigned).ToList();
        }

        public List<Employee> GetActiveEmployeesByReportingManager(string reportingManager)
        {
            return _db.Employees.Where(e => e.ReportingManager == reportingManager 
                                            && e.Status == Status.Active).ToList();
        }

        public List<Employee> GetResignedEmployeesByReportingManager(string reportingManager)
        {
            return _db.Employees.Where(e => e.ReportingManager == reportingManager
                                            && e.Status == Status.Resigned).ToList();
        }

        public List<Employee> GetEmployeesByWorkloadId(int workloadId)
        {
            return _db.Employees.Where(e => e.Workload.Id == workloadId).ToList();
        }

        public List<Employee> GetEmployeesByRoleId(int roleId)
        {
            return _db.Employees.Where(e => e.Role.Id == roleId).ToList();
        }

        public List<Employee> GetEmployeesByHiringDateInterval(DateTime startDate, DateTime endDate)
        {
            return _db.Employees.Where(e => e.HiringDate.Value > startDate && e.HiringDate.Value < endDate).ToList();
        }

        public List<Employee> GetEmployeesByVendor(string vendor)
        {
            return _db.Employees.Where(e => e.Vendor.ToLower() == vendor.ToLower()).ToList();
        }

        public List<Employee> GetEmployeesByBasedOut(string basedOut)
        {
            return _db.Employees.Where(e => e.BasedOut.ToLower() == basedOut.ToLower()).ToList();
        }

        // TODO: NEED TO BE REVIEWED AGAIN
        public List<Employee> GetEmployeesBySubsidiary(string subsidiary)
        {
            return _db.Employees.Where(e => e.Subsidiary.ToLower() == subsidiary.ToLower()).ToList();
        }

        public List<Employee> GetEmployeesByReportingManager(string reportingManager)
        {
            return _db.Employees.Where(e => e.ReportingManager.ToLower() == reportingManager.ToLower()).ToList();
        }

        public List<Employee> GetEmployeesByExperienceYears(int experienceYears)
        {
            return _db.Employees.Where(e => e.ExperienceYears == experienceYears).ToList();
        }

        public List<Employee> GetEmployeesByPreviousEmployer(string previousEmployer)
        {
            return _db.Employees.Where(e => e.PreviousEmployer.ToLower() == previousEmployer.ToLower()).ToList();
        }

        public List<Employee> GetGoodAttritionResignedEmployees()
        {
            return _db.Employees.Where(e => e.Attrition == Attrition.Good && e.Status == Status.Resigned).ToList();
        }

        public List<Employee> GetBadAttritionResignedEmployees()
        {
            return _db.Employees.Where(e => e.Attrition == Attrition.Bad && e.Status == Status.Resigned).ToList();
        }

        public List<Employee> GetResignedEmployeesByAttrition(Attrition attrition)
        {
            return _db.Employees.Where(e => e.Attrition == attrition 
                                        && e.Status == Status.Resigned).ToList();
        }

        public List<Employee> GetResignedEmployeesByMovement(string movement)
        {
            return _db.Employees.Where(e => e.Movement.ToLower() == movement.ToLower()
                                        && e.Status == Status.Resigned).ToList();
        }

        public List<Employee> GetEmployeesByResignationDateInterval(DateTime startDate, DateTime endDate)
        {
            return _db.Employees.Where(e => e.ResignationDate.Value > startDate
                                        && e.ResignationDate.Value < endDate
                                        && e.Status == Status.Resigned).ToList();
        }

        public List<Employee> GetEmployeeByAccessLevel(AccessLevel accessLevel)
        {
            return _db.Employees.Where(e => e.AccessLevel == accessLevel).ToList();
        }

        public List<string> GetAllReportingManagers()
        {
            return _db.Employees.Where(e => e.ReportingManager != null).Select(e => e.ReportingManager).Distinct().ToList();
        }

        public List<string> GetAllMajorUniversity()
        {
            return _db.Employees.Where(e => e.MajorUniversity != null).Select(e => e.MajorUniversity).Distinct().ToList();
        }

        public List<string> GetAllSubsidiaries()
        {
            return _db.Employees.Where(e => e.Subsidiary != null).Select(e => e.Subsidiary).Distinct().ToList();
        }

        public List<string> GetAllVendors()
        {
            return _db.Employees.Where(e => e.Vendor != null).Select(e => e.Vendor).Distinct().ToList();
        }

        public List<string> GetAllBasedOuts()
        {
            return _db.Employees.Where(e => e.BasedOut != null).Select(e => e.BasedOut).Distinct().ToList();
        }

        public List<string> GetAllPreviousEmployers()
        {
            return _db.Employees.Where(e => e.PreviousEmployer != null).Select(e => e.PreviousEmployer).Distinct().ToList();
        }

        public List<string> GetAllMovements()
        {
            return _db.Employees.Where(e => e.Movement != null).Select(e => e.Movement).Distinct().ToList();
        }

        public List<string> GetAllGraduationYears()
        {
            return _db.Employees.Where(e => e.GraduationYear != null).Select(e => e.GraduationYear).Distinct().ToList();
        }

        public List<int> GetAllExperienceYears()
        {
            return _db.Employees.Where(e => e.ExperienceYears != null).Select(e => (int)e.ExperienceYears).Distinct().ToList();
        }


        public void DbDispose(bool disposing)
        {
            _db.Dispose();
        }

    }
}