using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HR_Staffing.Models
{
    public class PhoneInterview
    {
        [Key, ForeignKey("Applicant")]
        public int ApplicantId { get; set; }

        public DateTime? SubmissionDate { get; set; }

        [Display(Name = "Phone Interview Feedback")]
        public string Feedback { get; set; }

        [Display(Name = "Phone Interview Status")]
        public PhoneIntervewStatus Status { get; set; }


        public virtual Applicant Applicant { get; set; }
    }

    public enum PhoneIntervewStatus
    {
        Passed,
        NotPassed
    }
}