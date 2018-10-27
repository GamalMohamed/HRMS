using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR_Staffing.Models;

namespace HR_Staffing.DataAccessLayer
{
    public class PhoneInterviewDataAccess
    {
        private readonly ApplicationDbContext _db;

        public PhoneInterviewDataAccess()
        {
            _db = new ApplicationDbContext();
        }

        public List<PhoneInterview> GetAllPhoneInterviews()
        {
            return _db.PhoneInterviews.ToList();
        }

        public PhoneInterview GetPhoneInterviewByApplicantId(int applicantId)
        {
            return _db.PhoneInterviews.SingleOrDefault(m => m.ApplicantId == applicantId);
        }

        public void CreatePhoneInterview(PhoneInterview phoneInterview)
        {
            _db.PhoneInterviews.Add(phoneInterview);
            _db.SaveChanges();
        }

    }
}