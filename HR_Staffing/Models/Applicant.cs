using HR_Staffing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HR_Staffing.Models
{
    public class Applicant
    {
        public int Id { get; set; }
		
		public string Passcode { get; set; }

		[Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Applicant Status")]
        public ApplicantStatus Status { get; set; }

        public string Role { get; set; }

        public string HiringManager { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comments { get; set; }


        public virtual IList<ApplicantQuestion> ApplicantQuestions { get; set; }

        public virtual FormResponse FormResponse { get; set; }

        public virtual PhoneInterview PhoneInterview { get; set; }
		
		public virtual IList<Interview> Interviews { get; set; }

    }

    public enum ApplicantStatus
    {
        Invited,
        ApplicationSubmitted,
        ScreeningPassed,
        ScreeningRejected,
        PhoneInterviewPassed,
        PhoneInterviewRejected,
        HRRejected,
        HRPassed,
        TechPassed,
        TechRejected,
        HiringManagerAccepted,
        HiringManagerRejected,
        Hired,
        NotHired
    }
}