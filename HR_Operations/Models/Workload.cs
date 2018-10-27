using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using CoEX_HRMS.Models;

namespace CoEX_HRMS.Models
{
    public class Workload
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Employees Count")]
        public int EmployeesCount { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

    }
}