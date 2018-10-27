namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class retainingOldDBTemporary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        IsHR = c.Boolean(nullable: false),
                        IsPreHiring = c.Boolean(nullable: false),
                        IsManager = c.Boolean(nullable: false),
                        Team = c.String(nullable: false),
                        Role = c.String(nullable: false),
                        HiringDate = c.DateTime(nullable: false),
                        Tech = c.String(nullable: false),
                        Subsidiary = c.String(nullable: false),
                        Vendor = c.String(nullable: false),
                        BasedOut = c.String(nullable: false),
                        ReportingManager = c.String(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        GraduationYear = c.String(nullable: false),
                        YearsOfExperience = c.Int(nullable: false),
                        MajorUniversity = c.String(nullable: false),
                        PreviousEmployer = c.String(),
                        IsGoodAttrition = c.Boolean(nullable: false),
                        IsFired = c.Boolean(nullable: false),
                        IsDead = c.Boolean(nullable: false),
                        IsResigned = c.Boolean(nullable: false),
                        LeaveDate = c.DateTime(),
                        Movement = c.String(),
                        ResignationStatus = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Profiles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ProfilePic = c.String(),
                        MilitaryCertificate = c.String(),
                        BirthCertificate = c.String(),
                        IdCard = c.String(),
                        Passport = c.String(),
                        GraduationCertificate = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.Id)
                .Index(t => t.Id);
            
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
            DropForeignKey("dbo.Profiles", "Id", "dbo.Employees");
            DropIndex("dbo.Profiles", new[] { "Id" });
            DropTable("dbo.UserTokenCaches");
            DropTable("dbo.Profiles");
            DropTable("dbo.Employees");
        }
    }
}
