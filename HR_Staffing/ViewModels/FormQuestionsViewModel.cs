using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR_Staffing.Models;

namespace HR_Staffing.ViewModels
{
    public class FormQuestionsViewModel
    {
        public FormType FormType { get; set; }
        public List<FormQuestion> FormQuestions { get; set; }
    }
}