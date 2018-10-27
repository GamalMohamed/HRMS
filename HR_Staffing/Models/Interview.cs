using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HR_Staffing.Models
{
    public class Interview
    {
        public int Id { get; set; }

        public int ApplicantId { get; set; }

        [Required]
        public InterviewType InterviewType { get; set; }  // HR, Technical, Hiring manager ??

        [Required]
        [Display(Name = "Interview Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime InterviewDate { get; set; }
        
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? StartTime { get; set; }
        
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? EndTime { get; set; }

        public virtual Applicant Applicant { get; set; }

        public virtual Feedback Feedback { get; set; }

    }

    public enum InterviewType
    {
        HR,
        Technical,
        HiringManager
    }
}