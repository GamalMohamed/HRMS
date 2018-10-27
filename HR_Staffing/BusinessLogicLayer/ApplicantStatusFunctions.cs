using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR_Staffing.DataAccessLayer;
using HR_Staffing.Models;

namespace HR_Staffing.BusinessLogicLayer
{
    public static class ApplicantStatusFunctions
    {
        private static readonly InterviewsDataAccess InterviewsDataAccess = new InterviewsDataAccess();

        public static bool IsPhoneInterviewDisabled(ApplicantStatus applicantStatus)
        {
            return applicantStatus == ApplicantStatus.ScreeningRejected;
        }

        public static bool IsHRInterviewDisabled(ApplicantStatus applicantStatus)
        {
            return applicantStatus == ApplicantStatus.ScreeningRejected
                   || applicantStatus == ApplicantStatus.PhoneInterviewRejected;
        }

        public static bool IsTechnicalInterviewDisabled(ApplicantStatus applicantStatus)
        {
            return applicantStatus == ApplicantStatus.ScreeningRejected
                   || applicantStatus == ApplicantStatus.PhoneInterviewRejected
                   || applicantStatus == ApplicantStatus.HRRejected;
        }

        public static bool IsHiringManagerInterviewDisabled(ApplicantStatus applicantStatus)
        {
            return applicantStatus == ApplicantStatus.ScreeningRejected
                   || applicantStatus == ApplicantStatus.PhoneInterviewRejected
                   || applicantStatus == ApplicantStatus.HRRejected
                   || applicantStatus == ApplicantStatus.TechRejected;
        }

        public static bool IsInterviewSubmitted(int applicantId, InterviewType interviewType)
        {
            var interview = InterviewsDataAccess.GetInterviewByApplicantId(applicantId, interviewType);

            return interview != null;
        }
    }
}