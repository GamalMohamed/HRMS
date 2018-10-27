using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HR_Staffing.Models
{
    public class Feedback
    {
        [Key, ForeignKey("Interview")]
        public int InterviewId { get; set; }

        [Required]
        public string Interviewer { get; set; }
        
        [EmailAddress]
        [Display(Name = "Interviewers Aliases")]
        public string InterviewerAlias { get; set; }

        public DateTime SubmissionDate { get; set; }

        [Display(Name = "Result")]
        public bool Hired { get; set; }

        
        public virtual Interview Interview { get; set; }

        public virtual IList<FeedbackContent> FeedbackContents { get; set; }
    }
}