using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CoEX_HRMS.Models;

namespace CoEX_HRMS.ViewModels
{
    public class ResignedViewModel
    {
        [Display(Name = "Attrition")]
        public Attrition? Attrition { get; set; }

        [Display(Name = "Starting Resignation Date")]
        public DateTime? StartingResignationDate { get; set; }

        [Display(Name = "Ending Resignation Date")]
        public DateTime? EndingResignationDate { get; set; }
        
        [Display(Name = "Movement")]
        public List<string> Movement { get; set; }

        public List<string> MovementsList { get; set; }
    }
}