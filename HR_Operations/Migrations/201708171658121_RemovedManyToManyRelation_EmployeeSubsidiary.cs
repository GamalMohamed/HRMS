namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedManyToManyRelation_EmployeeSubsidiary : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SubsidiaryEmployees", "Subsidiary_Id", "dbo.Subsidiaries");
            DropForeignKey("dbo.SubsidiaryEmployees", "Employee_Id", "dbo.Employees");
            DropIndex("dbo.SubsidiaryEmployees", new[] { "Subsidiary_Id" });
            DropIndex("dbo.SubsidiaryEmployees", new[] { "Employee_Id" });
            AddColumn("dbo.Employees", "Subsidiary", c => c.String(nullable: false));
            DropTable("dbo.Subsidiaries");
            DropTable("dbo.SubsidiaryEmployees");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SubsidiaryEmployees",
                c => new
                    {
                        Subsidiary_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subsidiary_Id, t.Employee_Id });
            
            CreateTable(
                "dbo.Subsidiaries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Employees", "Subsidiary");
            CreateIndex("dbo.SubsidiaryEmployees", "Employee_Id");
            CreateIndex("dbo.SubsidiaryEmployees", "Subsidiary_Id");
            AddForeignKey("dbo.SubsidiaryEmployees", "Employee_Id", "dbo.Employees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SubsidiaryEmployees", "Subsidiary_Id", "dbo.Subsidiaries", "Id", cascadeDelete: true);
        }
    }
}
