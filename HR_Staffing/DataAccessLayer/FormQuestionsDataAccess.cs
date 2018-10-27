using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HR_Staffing.Models;

namespace HR_Staffing.DataAccessLayer
{
    public class FormQuestionsDataAccess
    {
        private readonly ApplicationDbContext _db;

        public FormQuestionsDataAccess()
        {
            _db = new ApplicationDbContext();
        }

        public List<FormQuestion> GetAllQuestions(FormType formType)
        {
            var formQuestions = from d in _db.FormQuestions
                where d.FormType == formType
                orderby d.OrderInForm
                select d;

            return formQuestions.ToList();
        }

        public FormQuestion GetQuestionById(int? id)
        {
            return _db.FormQuestions.FirstOrDefault(m => m.Id == id);
        }

        public void CreateQuestion(FormQuestion formQuestion)
        {
            var maxOrderInForm = 0;
            try
            {
                maxOrderInForm = _db.FormQuestions.Where(x => x.FormType == formQuestion.FormType).Max(x => x.OrderInForm);
            }
            catch { /* Ignore exception (no rows in the table) */}
            formQuestion.OrderInForm = maxOrderInForm + 1;

            _db.FormQuestions.Add(formQuestion);
            _db.SaveChanges();
        }

        public void EditQuestion(FormQuestion formQuestion)
        {
            var formQuestionInDb = GetQuestionById(formQuestion.Id);
            formQuestionInDb.Title = formQuestion.Title;
            formQuestionInDb.Description = formQuestion.Description;
            _db.SaveChanges();
        }

        public void DeleteQuestion(FormQuestion formQuestion)
        {
            _db.FormQuestions.Remove(formQuestion);
            _db.SaveChanges();
            RefreshQuestionsOrder(formQuestion.FormType);
        }

        public void RefreshQuestionsOrder(FormType formType)
        {
            var formQuestions = GetAllQuestions(formType);

            var i = 1;
            foreach (var formQuestion in formQuestions)
            {
                formQuestion.OrderInForm = i;
                i++;
            }
            _db.SaveChanges();
        }

        public void SwapQuestionsOrder(int id, FormType formType, int currentOrder, int otherQuestionOrder)
        {
            var otherFormQuestionInDb = _db.FormQuestions.Single(x => x.OrderInForm == otherQuestionOrder && x.FormType == formType);
            otherFormQuestionInDb.OrderInForm = currentOrder;

            var currentFormQuestionInDb = _db.FormQuestions.Single(x => x.Id == id);
            currentFormQuestionInDb.OrderInForm = otherQuestionOrder;

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