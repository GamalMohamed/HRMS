using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CoEX_HRMS.Models;

namespace CoEX_HRMS.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserTokenCache> UserTokenCacheList { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<Workload> Workloads { get; set; }

        public DbSet<Role> Roles { get; set; }

    }

    public class UserTokenCache
    {
        [Key]
        public int UserTokenCacheId { get; set; }
        public string webUserUniqueId { get; set; }
        public byte[] cacheBits { get; set; }
        public DateTime LastWrite { get; set; }
    }
}
