using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using CoEX_HRMS.Models;
using CoEX_HRMS.Models;

namespace CoEX_HRMS.ViewModels
{
    public class SearchFormViewModel
    {
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Starting Birth Date")]
        public DateTime? StartingBirthDate { get; set; }

        [Display(Name = "Ending Birth Date")]
        public DateTime? EndingBirthDate { get; set; }

        [Display(Name = "Major-University")]
        public List<string> MajorUniversity { get; set; }

        [Display(Name = "Graduation Year")]
        public List<string> GraduationYear { get; set; }


        public List<int> RoleId { get; set; }

        public List<int> WorkloadId { get; set; }

        [Display(Name = "Vendor")]
        public List<string> Vendor { get; set; }

        [Display(Name = "Based out")]
        public List<string> BasedOut { get; set; }

        [Display(Name = "Subsidiary")]
        public List<string> Subsidiary { get; set; }

        [Display(Name = "Reporting Manager")]
        public List<string> ReportingManager { get; set; }


        [Display(Name = "Starting Hiring Date")]
        public DateTime? StartingHiringDate { get; set; }

        [Display(Name = "Ending Hiring Date")]
        public DateTime? EndingHiringDate { get; set; }

        [Display(Name = "Experience Years")]
        public List<int> ExperienceYears { get; set; }

        [Display(Name = "Previous Employer")]
        public List<string> PreviousEmployer { get; set; }

    }
}