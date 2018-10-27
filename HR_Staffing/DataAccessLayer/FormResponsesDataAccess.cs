using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HR_Staffing.Models;

namespace HR_Staffing.DataAccessLayer
{
    public class FormResponsesDataAccess
    {
        private readonly ApplicationDbContext _db;

        public FormResponsesDataAccess()
        {
            _db = new ApplicationDbContext();
        }

        public List<FormResponse> GetAllResponses()
        {
            var formResponses = from d in _db.FormResponses
                orderby d.SubmissionDate descending
                select d;

            return formResponses.ToList();
        }

        public FormResponse GetResponseByApplicantId(int? applicantId)
        {
            return _db.FormResponses.FirstOrDefault(m => m.ApplicantId == applicantId);
        }

        public void CreateResponse(FormResponse formResponse)
        {
            _db.FormResponses.Add(formResponse);
            _db.SaveChanges();
        }

        public void EditResponse(FormResponse formResponse)
        {
            _db.Entry(formResponse).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void EditResponseStatus(FormResponse formResponse, ApplicationFormResponseStatus responseStatus)
        {
            formResponse.Status = responseStatus;
            _db.SaveChanges();
        }

        public void DeleteFormResponse(FormResponse formResponse)
        {
            _db.FormResponses.Remove(formResponse);
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