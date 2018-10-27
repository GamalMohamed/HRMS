namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeIntsNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Employees", "WorkloadId", "dbo.Workloads");
            DropIndex("dbo.Employees", new[] { "WorkloadId" });
            DropIndex("dbo.Employees", new[] { "RoleId" });
            AlterColumn("dbo.Employees", "ExperienceYears", c => c.Int());
            AlterColumn("dbo.Employees", "WorkloadId", c => c.Int());
            AlterColumn("dbo.Employees", "RoleId", c => c.Int());
            CreateIndex("dbo.Employees", "WorkloadId");
            CreateIndex("dbo.Employees", "RoleId");
            AddForeignKey("dbo.Employees", "RoleId", "dbo.Roles", "Id");
            AddForeignKey("dbo.Employees", "WorkloadId", "dbo.Workloads", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "WorkloadId", "dbo.Workloads");
            DropForeignKey("dbo.Employees", "RoleId", "dbo.Roles");
            DropIndex("dbo.Employees", new[] { "RoleId" });
            DropIndex("dbo.Employees", new[] { "WorkloadId" });
            AlterColumn("dbo.Employees", "RoleId", c => c.Int(nullable: false));
            AlterColumn("dbo.Employees", "WorkloadId", c => c.Int(nullable: false));
            AlterColumn("dbo.Employees", "ExperienceYears", c => c.Int(nullable: false));
            CreateIndex("dbo.Employees", "RoleId");
            CreateIndex("dbo.Employees", "WorkloadId");
            AddForeignKey("dbo.Employees", "WorkloadId", "dbo.Workloads", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Employees", "RoleId", "dbo.Roles", "Id", cascadeDelete: true);
        }
    }
}
