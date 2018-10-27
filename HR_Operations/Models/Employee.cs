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
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [EmailAddress, Display(Name = "Alias")]
        public string Email { get; set; }

        [Phone, Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Major-University")]
        public string MajorUniversity { get; set; }

        [Display(Name = "Graduation Year")]
        public string GraduationYear { get; set; }

        [Display(Name = "Hiring Date")]
        public DateTime? HiringDate { get; set; }

        [Display(Name = "Experience Years")]
        public int? ExperienceYears { get; set; }

        [Display(Name = "Previous Employer")]
        public string PreviousEmployer { get; set; }

        [Display(Name = "Vendor")]
        public string Vendor { get; set; }

        [Display(Name = "Access Level")]
        public AccessLevel? AccessLevel { get; set; }

        public int? WorkloadId { get; set; }

        [ForeignKey("WorkloadId")]
        public virtual Workload Workload { get; set; }

        public int? RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [Display(Name = "Based out")]
        public string BasedOut { get; set; }

        [Display(Name = "Subsidiary")]
        public string Subsidiary { get; set; }

        [Display(Name = "Reporting Manager")]
        public string ReportingManager { get; set; }

        [Display(Name = "Status")]
        public Status Status { get; set; }

        [Display(Name = "Attrition")]
        public Attrition? Attrition { get; set; }

        [Display(Name = "Resignation Date")]
        public DateTime? ResignationDate { get; set; }

        [Display(Name = "Movement")]
        public string Movement { get; set; }

        [Display(Name = "Resignation Status")]
        public string ResignationStatus { get; set; }

        public virtual Profile Profile { get; set; }

    }

    public enum AccessLevel
    {
        EmployeeView,
        TeamView,
        FullView,
        FullAccess
    }

    public enum Status
    {
        Active,
        Resigned
    }

    public enum Attrition
    {
        TBD,
        Good,
        Bad
    }
}