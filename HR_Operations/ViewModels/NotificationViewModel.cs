using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoEX_HRMS.Models;

namespace CoEX_HRMS.ViewModels
{
    public class NotificationViewModel
    {
        public List<Employee> HiringEmployees { get; set; }
        public List<Employee> ResignedEmployees { get; set; }
        public List<Profile> UncompletedProfiles { get; set; }
    }
}