using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using HR_Staffing.Models;

namespace HR_Staffing.Models
{
    public class ApplicantQuestion
    {
        public int Id { get; set; }

        public int ApplicantId { get; set; }

        public int OrderInForm { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Question Title")]
        public string Title { get; set; }

        [Display(Name = "Question Description")]
        public string Description { get; set; }


        public virtual Applicant Applicant { get; set; }

        public virtual FormResponseContent FormResponseContent { get; set; }
    }
}