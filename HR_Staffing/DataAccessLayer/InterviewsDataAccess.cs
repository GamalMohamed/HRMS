using HR_Staffing.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HR_Staffing.DataAccessLayer
{
    public class InterviewsDataAccess
    {
        private readonly ApplicationDbContext _db;

        public InterviewsDataAccess()
        {
            _db = new ApplicationDbContext();
        }

        public List<Interview> GetAllInterviews()
        {
            return _db.Interviews.ToList();
        }

        public Interview GetInterviewById(int? id)
        {
            return _db.Interviews.FirstOrDefault(m => m.Id == id);
        }

        public Interview GetInterviewByApplicantId(int? applicantId, InterviewType interviewType)
        {
            return _db.Interviews.FirstOrDefault(m => m.ApplicantId == applicantId && m.InterviewType == interviewType);
        }

        public void CreateInterview(Interview interview)
        {
            _db.Interviews.Add(interview);
            _db.SaveChanges();
        }

        public void EditInterview(Interview interview)
        {
            _db.Entry(interview).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteInterview(Interview interview)
        {
            _db.Interviews.Remove(interview);
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