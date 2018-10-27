using HR_Staffing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR_Staffing.ViewModels
{
    public class PhoneInterviewViewModel
    {
        public List<FormQuestion> FormQuestions { get; set; }
        public PhoneInterview PhoneInterview { get; set; }
    }
}