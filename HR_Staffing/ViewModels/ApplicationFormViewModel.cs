using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR_Staffing.Models;

namespace HR_Staffing.ViewModels
{
    public class ApplicationFormViewModel
    {
        public IEnumerable<ApplicantQuestion> ApplicantQuestions { get; set; }
        public FormResponse FormResponse { get; set; }
    }
}