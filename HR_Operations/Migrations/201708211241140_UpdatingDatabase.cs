namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingDatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "TeamId", "dbo.Teams");
            DropIndex("dbo.Employees", new[] { "TeamId" });
            CreateTable(
                "dbo.Workloads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        EmployeesCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Employees", "AccessLevel", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "WorkloadId", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "WorkloadId");
            AddForeignKey("dbo.Employees", "WorkloadId", "dbo.Workloads", "Id", cascadeDelete: true);
            DropColumn("dbo.Employees", "RoleAccess");
            DropColumn("dbo.Employees", "TeamId");
            DropTable("dbo.Teams");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        MembersCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Employees", "TeamId", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "RoleAccess", c => c.Int(nullable: false));
            DropForeignKey("dbo.Employees", "WorkloadId", "dbo.Workloads");
            DropIndex("dbo.Employees", new[] { "WorkloadId" });
            DropColumn("dbo.Employees", "WorkloadId");
            DropColumn("dbo.Employees", "AccessLevel");
            DropTable("dbo.Workloads");
            CreateIndex("dbo.Employees", "TeamId");
            AddForeignKey("dbo.Employees", "TeamId", "dbo.Teams", "Id", cascadeDelete: true);
        }
    }
}
