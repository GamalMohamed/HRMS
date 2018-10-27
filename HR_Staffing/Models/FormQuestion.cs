using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HR_Staffing.Models
{
    public class FormQuestion
    {
        public int Id { get; set; }

        public FormType FormType { get; set; }

        public int OrderInForm { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Question Title")]
        public string Title { get; set; }

        [Display(Name = "Question Description")]
        public string Description { get; set; }
    }

    public enum FormType
    {
        ApplicationForm,
        PhoneInterview,
        FeedbackForm
    }
}