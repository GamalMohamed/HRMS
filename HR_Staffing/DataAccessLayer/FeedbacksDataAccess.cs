using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HR_Staffing.Models;


namespace HR_Staffing.DataAccessLayer
{
    public class FeedbacksDataAccess
    {
        private readonly ApplicationDbContext _db;

        public FeedbacksDataAccess()
        {
            _db = new ApplicationDbContext();
        }

        public List<Feedback> GetAllFeedbacks()
        {
            return _db.Feedbacks.ToList();
        }

        public Feedback GetFeedbackByInterviewId(int? interviewId)
        {
            return _db.Feedbacks.FirstOrDefault(m => m.InterviewId == interviewId);
        }

        public void CreateFeedback(Feedback feedback)
        {
            _db.Feedbacks.Add(feedback);
            _db.SaveChanges();
        }

        public void EditFeedback(Feedback feedback)
        {
            _db.Entry(feedback).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void DeleteFeedback(Feedback feedback)
        {
            _db.Feedbacks.Remove(feedback);
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