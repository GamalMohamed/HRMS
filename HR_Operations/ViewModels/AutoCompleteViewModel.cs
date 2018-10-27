using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoEX_HRMS.ViewModels
{
    public class AutoCompleteViewModel
    {
        public SearchFormViewModel SearchFormViewModel { get; set; }
        public List<string> ReportingMangersList { get; set; }

        public List<string> BasedOutList { get; set; }

        public List<string> SubsidaryList { get; set; }
        
        public List<string> MajorUniversitiesList { get; set; }

        public List<string> VendorsList { get; set; }

        public List<string> PreviousEmployersList { get; set; }

        public List<string> GraduationyearsList { get; set; }

        public List<int> ExperiencesList { get; set; }
    }
}