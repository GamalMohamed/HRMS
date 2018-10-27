namespace HR_Staffing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Applicants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Passcode = c.String(),
                        Name = c.String(nullable: false),
                        Status = c.Int(nullable: false),
                        Role = c.String(),
                        HiringManager = c.String(),
                        Comments = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicantQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicantId = c.Int(nullable: false),
                        OrderInForm = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.ApplicantId, cascadeDelete: true)
                .Index(t => t.ApplicantId);
            
            CreateTable(
                "dbo.FormResponseContents",
                c => new
                    {
                        ApplicantQuestionId = c.Int(nullable: false),
                        ApplicantId = c.Int(nullable: false),
                        QuestionAnswer = c.String(),
                    })
                .PrimaryKey(t => t.ApplicantQuestionId)
                .ForeignKey("dbo.ApplicantQuestions", t => t.ApplicantQuestionId)
                .ForeignKey("dbo.FormResponses", t => t.ApplicantId, cascadeDelete: true)
                .Index(t => t.ApplicantQuestionId)
                .Index(t => t.ApplicantId);
            
            CreateTable(
                "dbo.FormResponses",
                c => new
                    {
                        ApplicantId = c.Int(nullable: false),
                        SubmissionDate = c.DateTime(),
                        ApplicantName = c.String(nullable: false, maxLength: 255),
                        ApplicantEmail = c.String(nullable: false, maxLength: 255),
                        ApplicantPhoneNumber = c.String(nullable: false, maxLength: 30),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicantId)
                .ForeignKey("dbo.Applicants", t => t.ApplicantId)
                .Index(t => t.ApplicantId);
            
            CreateTable(
                "dbo.Interviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicantId = c.Int(nullable: false),
                        InterviewType = c.Int(nullable: false),
                        InterviewDate = c.DateTime(nullable: false),
                        StartTime = c.DateTime(),
                        EndTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Applicants", t => t.ApplicantId, cascadeDelete: true)
                .Index(t => t.ApplicantId);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        InterviewId = c.Int(nullable: false),
                        Interviewer = c.String(nullable: false),
                        InterviewerAlias = c.String(),
                        SubmissionDate = c.DateTime(nullable: false),
                        Hired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.InterviewId)
                .ForeignKey("dbo.Interviews", t => t.InterviewId)
                .Index(t => t.InterviewId);
            
            CreateTable(
                "dbo.FeedbackContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InterviewId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                        QuestionAnswer = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Feedbacks", t => t.InterviewId, cascadeDelete: true)
                .Index(t => t.InterviewId);
            
            CreateTable(
                "dbo.PhoneInterviews",
                c => new
                    {
                        ApplicantId = c.Int(nullable: false),
                        SubmissionDate = c.DateTime(),
                        Feedback = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApplicantId)
                .ForeignKey("dbo.Applicants", t => t.ApplicantId)
                .Index(t => t.ApplicantId);
            
            CreateTable(
                "dbo.FormQuestions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormType = c.Int(nullable: false),
                        OrderInForm = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 255),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserTokenCaches",
                c => new
                    {
                        UserTokenCacheId = c.Int(nullable: false, identity: true),
                        webUserUniqueId = c.String(),
                        cacheBits = c.Binary(),
                        LastWrite = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserTokenCacheId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PhoneInterviews", "ApplicantId", "dbo.Applicants");
            DropForeignKey("dbo.Feedbacks", "InterviewId", "dbo.Interviews");
            DropForeignKey("dbo.FeedbackContents", "InterviewId", "dbo.Feedbacks");
            DropForeignKey("dbo.Interviews", "ApplicantId", "dbo.Applicants");
            DropForeignKey("dbo.FormResponseContents", "ApplicantId", "dbo.FormResponses");
            DropForeignKey("dbo.FormResponses", "ApplicantId", "dbo.Applicants");
            DropForeignKey("dbo.FormResponseContents", "ApplicantQuestionId", "dbo.ApplicantQuestions");
            DropForeignKey("dbo.ApplicantQuestions", "ApplicantId", "dbo.Applicants");
            DropIndex("dbo.PhoneInterviews", new[] { "ApplicantId" });
            DropIndex("dbo.FeedbackContents", new[] { "InterviewId" });
            DropIndex("dbo.Feedbacks", new[] { "InterviewId" });
            DropIndex("dbo.Interviews", new[] { "ApplicantId" });
            DropIndex("dbo.FormResponses", new[] { "ApplicantId" });
            DropIndex("dbo.FormResponseContents", new[] { "ApplicantId" });
            DropIndex("dbo.FormResponseContents", new[] { "ApplicantQuestionId" });
            DropIndex("dbo.ApplicantQuestions", new[] { "ApplicantId" });
            DropTable("dbo.UserTokenCaches");
            DropTable("dbo.FormQuestions");
            DropTable("dbo.PhoneInterviews");
            DropTable("dbo.FeedbackContents");
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Interviews");
            DropTable("dbo.FormResponses");
            DropTable("dbo.FormResponseContents");
            DropTable("dbo.ApplicantQuestions");
            DropTable("dbo.Applicants");
        }
    }
}
