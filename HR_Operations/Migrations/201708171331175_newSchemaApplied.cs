namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newSchemaApplied : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        MembersCount = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subsidiaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        MembersCount = c.Int(nullable: false),
                        LastUpdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Employees", "Gender", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employees", "ExperienceYears", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "RoleAccess", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "TeamId", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "RoleId", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "SubsidiaryId", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "ResignationDate", c => c.DateTime());
            AlterColumn("dbo.Employees", "PreviousEmployer", c => c.String(nullable: false));
            CreateIndex("dbo.Employees", "TeamId");
            CreateIndex("dbo.Employees", "RoleId");
            CreateIndex("dbo.Employees", "SubsidiaryId");
            AddForeignKey("dbo.Employees", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employees", "SubsidiaryId", "dbo.Subsidiaries", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employees", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
            DropColumn("dbo.Employees", "IsHR");
            DropColumn("dbo.Employees", "IsPreHiring");
            DropColumn("dbo.Employees", "IsManager");
            DropColumn("dbo.Employees", "Team");
            DropColumn("dbo.Employees", "Role");
            DropColumn("dbo.Employees", "Subsidiary");
            DropColumn("dbo.Employees", "DateOfBirth");
            DropColumn("dbo.Employees", "YearsOfExperience");
            DropColumn("dbo.Employees", "IsFired");
            DropColumn("dbo.Employees", "IsDead");
            DropColumn("dbo.Employees", "LeaveDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "LeaveDate", c => c.DateTime());
            AddColumn("dbo.Employees", "IsDead", c => c.Boolean(nullable: false));
            AddColumn("dbo.Employees", "IsFired", c => c.Boolean(nullable: false));
            AddColumn("dbo.Employees", "YearsOfExperience", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employees", "Subsidiary", c => c.String(nullable: false));
            AddColumn("dbo.Employees", "Role", c => c.String(nullable: false));
            AddColumn("dbo.Employees", "Team", c => c.String(nullable: false));
            AddColumn("dbo.Employees", "IsManager", c => c.Boolean(nullable: false));
            AddColumn("dbo.Employees", "IsPreHiring", c => c.Boolean(nullable: false));
            AddColumn("dbo.Employees", "IsHR", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Employees", "TeamId", "dbo.Teams");
            DropForeignKey("dbo.Employees", "SubsidiaryId", "dbo.Subsidiaries");
            DropForeignKey("dbo.Employees", "RoleId", "dbo.Roles");
            DropIndex("dbo.Employees", new[] { "SubsidiaryId" });
            DropIndex("dbo.Employees", new[] { "RoleId" });
            DropIndex("dbo.Employees", new[] { "TeamId" });
            AlterColumn("dbo.Employees", "PreviousEmployer", c => c.String());
            DropColumn("dbo.Employees", "ResignationDate");
            DropColumn("dbo.Employees", "SubsidiaryId");
            DropColumn("dbo.Employees", "RoleId");
            DropColumn("dbo.Employees", "TeamId");
            DropColumn("dbo.Employees", "RoleAccess");
            DropColumn("dbo.Employees", "ExperienceYears");
            DropColumn("dbo.Employees", "BirthDate");
            DropColumn("dbo.Employees", "Gender");
            DropTable("dbo.Teams");
            DropTable("dbo.Subsidiaries");
            DropTable("dbo.Roles");
        }
    }
}
