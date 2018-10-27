namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeAccessLevelNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "AccessLevel", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "AccessLevel", c => c.Int(nullable: false));
        }
    }
}
