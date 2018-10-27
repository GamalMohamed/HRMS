namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeAttritionNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Attrition", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Attrition", c => c.Int(nullable: false));
        }
    }
}
