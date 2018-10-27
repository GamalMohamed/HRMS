namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedEmployeeModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "Attrition", c => c.Int(nullable: false));
            DropColumn("dbo.Employees", "IsGoodAttrition");
            DropColumn("dbo.Employees", "IsResigned");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "IsResigned", c => c.Boolean(nullable: false));
            AddColumn("dbo.Employees", "IsGoodAttrition", c => c.Boolean(nullable: false));
            DropColumn("dbo.Employees", "Attrition");
            DropColumn("dbo.Employees", "Status");
        }
    }
}
