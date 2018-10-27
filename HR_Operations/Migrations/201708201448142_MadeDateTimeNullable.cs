namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeDateTimeNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "BirthDate", c => c.DateTime());
            AlterColumn("dbo.Employees", "HiringDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "HiringDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Employees", "BirthDate", c => c.DateTime(nullable: false));
        }
    }
}
