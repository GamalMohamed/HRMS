using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HR_Staffing.ViewModels
{
    public class FormAuthViewModel
    {
        [Required]
        public string Passcode { get; set; }
    }
}