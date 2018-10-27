using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CoEX_HRMS.Models;

namespace CoEX_HRMS.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}