using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HR_Staffing.Models
{
    public class FormResponse
    {
        public FormResponse()
        {
            this.FormResponseContents = new List<FormResponseContent>();
        }
        
        [Key, ForeignKey("Applicant")]
        public int ApplicantId { get; set; }

        public DateTime? SubmissionDate { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Applicant Name")]
        public string ApplicantName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        [Display(Name = "Applicant Email")]
        public string ApplicantEmail { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Applicant Phone Number")]
        public string ApplicantPhoneNumber { get; set; }

        [Required]
        [Display(Name = "Applicant Resume")]
        public string ApplicantResume { get; set; }

        [Display(Name = "Response Status")]
        public ApplicationFormResponseStatus Status { get; set; }


        public virtual Applicant Applicant { get; set; }

        public virtual IList<FormResponseContent> FormResponseContents { get; set; }

    }

    public enum ApplicationFormResponseStatus
    {
        Passed,
        NotPassed,
        Viewed,
        NotViewed
    }
}