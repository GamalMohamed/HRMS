using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HR_Staffing.Models
{
    public class FeedbackContent
    {
        public int Id { get; set; }

        public int InterviewId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Question Title")]
        public string Title { get; set; }

        [Display(Name = "Question Description")]
        public string Description { get; set; }

        public string QuestionAnswer { get; set; }


        public virtual Feedback Feedback { get; set; }
    }
}