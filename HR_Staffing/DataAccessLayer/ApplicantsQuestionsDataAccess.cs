using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR_Staffing.Models;

namespace HR_Staffing.DataAccessLayer
{
    public class ApplicantsQuestionsDataAccess
    {
        private readonly ApplicationDbContext _db;

        public ApplicantsQuestionsDataAccess()
        {
            _db = new ApplicationDbContext();
        }

        public List<ApplicantQuestion> GetAllQuestions(int applicantId)
        {
            var applicantQuestions = from d in _db.ApplicantsQuestions
                where d.ApplicantId == applicantId
                orderby d.OrderInForm
                select d;

            return applicantQuestions.ToList();
        }

        public ApplicantQuestion GetQuestionById(int? id)
        {
            return _db.ApplicantsQuestions.FirstOrDefault(m => m.Id == id);
        }

        public void CreateQuestion(ApplicantQuestion applicantQuestion)
        {
            var maxOrderInForm = 0;
            try
            {
                maxOrderInForm = _db.ApplicantsQuestions.Where(x => x.ApplicantId == applicantQuestion.ApplicantId).Max(x => x.OrderInForm);
            }
            catch { /* Ignore exception (no rows in the table) */}
            applicantQuestion.OrderInForm = maxOrderInForm + 1;

            _db.ApplicantsQuestions.Add(applicantQuestion);
            _db.SaveChanges();
        }

        public void EditQuestion(ApplicantQuestion applicantQuestion)
        {
            var applicantQuestionInDb = GetQuestionById(applicantQuestion.Id);
            applicantQuestionInDb.Title = applicantQuestion.Title;
            applicantQuestionInDb.Description = applicantQuestion.Description;
            _db.SaveChanges();
        }

        public void DeleteQuestion(ApplicantQuestion applicantQuestion)
        {
            _db.ApplicantsQuestions.Remove(applicantQuestion);
            _db.SaveChanges();
            RefreshQuestionsOrder(applicantQuestion.ApplicantId);
        }

        public void RefreshQuestionsOrder(int applicantId)
        {
            var applicantQuestions = GetAllQuestions(applicantId);

            var i = 1;
            foreach (var applicantQuestion in applicantQuestions)
            {
                applicantQuestion.OrderInForm = i;
                i++;
            }
            _db.SaveChanges();
        }

        public void SwapQuestionsOrder(int id, int applicantId, int currentOrder, int otherQuestionOrder)
        {
            var otherApplicantQuestionInDb = _db.ApplicantsQuestions.Single(x => x.OrderInForm == otherQuestionOrder && x.ApplicantId == applicantId);
            otherApplicantQuestionInDb.OrderInForm = currentOrder;

            var currentApplicantQuestionInDb = _db.ApplicantsQuestions.Single(x => x.Id == id);
            currentApplicantQuestionInDb.OrderInForm = otherQuestionOrder;

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