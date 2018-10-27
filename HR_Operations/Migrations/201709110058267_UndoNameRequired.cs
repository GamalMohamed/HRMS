namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UndoNameRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Name", c => c.String(nullable: false));
        }
    }
}
