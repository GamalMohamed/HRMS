using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HR_Staffing.Models
{
    public class FormResponseContent
    {
        [Key, ForeignKey("ApplicantQuestion")]
        public int ApplicantQuestionId { get; set; }

        public int ApplicantId { get; set; }

        public string QuestionAnswer { get; set; }


        public virtual FormResponse FormResponse { get; set; }

        public virtual ApplicantQuestion ApplicantQuestion { get; set; }
    }
}