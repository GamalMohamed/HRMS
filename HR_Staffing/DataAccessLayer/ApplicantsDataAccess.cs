using HR_Staffing.Models;
using HR_Staffing.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HR_Staffing.DataAccessLayer
{
    public class ApplicantsDataAccess
    {
        private readonly ApplicationDbContext _db;

        public ApplicantsDataAccess()
        {
            _db = new ApplicationDbContext();
        }

        public List<Applicant> GetAllApplicants()
        {
            return _db.Applicants.ToList();
        }

        public Applicant GetApplicantById(int? id)
        {
            return _db.Applicants.FirstOrDefault(m => m.Id == id);
        }

        public Applicant GetApplicantByPasscode(string passcode)
        {
            return _db.Applicants.FirstOrDefault(m => m.Passcode == passcode);
        }

        public List<Applicant> GetApplicantsByHiringManager(string hiringManager)
        {
            return _db.Applicants.Where(m => m.HiringManager == hiringManager).ToList();
        }

        public int CreateApplicant(Applicant applicant)
        {
            _db.Applicants.Add(applicant);
            _db.SaveChanges();
            
            applicant.Passcode = applicant.Id.ToString()+RandomStringGenerator.RandomString(20);
            _db.Entry(applicant).State = EntityState.Modified;
            _db.SaveChanges();

            return applicant.Id;
        }

        public void EditApplicant(Applicant applicant)
        {
            /*_db.Entry(applicant).State = EntityState.Modified;
            _db.SaveChanges();*/

            var applicantInDb = GetApplicantById(applicant.Id);
            applicantInDb.Name = applicant.Name;
            applicantInDb.Role = applicant.Role;
            applicantInDb.HiringManager = applicant.HiringManager;
            applicantInDb.Status = applicant.Status;
            applicantInDb.Comments = applicant.Comments;
            _db.SaveChanges();
        }

        public void EditApplicantStatus(Applicant applicant, ApplicantStatus applicantStatus)
        {
            applicant.Status = applicantStatus;
            _db.SaveChanges();
        }

        public void DeleteApplicant(Applicant applicant)
        {
            _db.Applicants.Remove(applicant);
            _db.SaveChanges();
        }

        public void SaveChangesToDb()
        {
            _db.SaveChanges();
        }

        public void DbDispose()
        {
            _db.Dispose();
        }

    }
}