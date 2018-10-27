namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ManyToManyRelationEmployeeSubsidiary : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "SubsidiaryId", "dbo.Subsidiaries");
            DropIndex("dbo.Employees", new[] { "SubsidiaryId" });
            CreateTable(
                "dbo.SubsidiaryEmployees",
                c => new
                    {
                        Subsidiary_Id = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Subsidiary_Id, t.Employee_Id })
                .ForeignKey("dbo.Subsidiaries", t => t.Subsidiary_Id, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Subsidiary_Id)
                .Index(t => t.Employee_Id);
            
            DropColumn("dbo.Employees", "SubsidiaryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "SubsidiaryId", c => c.Int(nullable: false));
            DropForeignKey("dbo.SubsidiaryEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.SubsidiaryEmployees", "Subsidiary_Id", "dbo.Subsidiaries");
            DropIndex("dbo.SubsidiaryEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.SubsidiaryEmployees", new[] { "Subsidiary_Id" });
            DropTable("dbo.SubsidiaryEmployees");
            CreateIndex("dbo.Employees", "SubsidiaryId");
            AddForeignKey("dbo.Employees", "SubsidiaryId", "dbo.Subsidiaries", "Id", cascadeDelete: true);
        }
    }
}
