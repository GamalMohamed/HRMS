using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HR_Staffing.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<Applicant> Applicants { get; set; }

        public DbSet<Interview> Interviews { get; set; }

        public DbSet<FormQuestion> FormQuestions { get; set; }
        public DbSet<FormResponse> FormResponses { get; set; }
        public DbSet<FormResponseContent> FormResponsesContents { get; set; }

        public DbSet<PhoneInterview> PhoneInterviews { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<FeedbackContent> FeedbacksContents { get; set; }

        public DbSet<ApplicantQuestion> ApplicantsQuestions { get; set; }

        public DbSet<UserTokenCache> UserTokenCacheList { get; set; }
    

        
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
