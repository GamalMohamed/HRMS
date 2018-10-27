namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedGenderToString : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Employees", "Gender", c => c.String(nullable: false));
            DropColumn("dbo.Employees","Gender");
            AddColumn("dbo.Employees","Gender", c => c.String(nullable: false));

        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Gender", c => c.Int(nullable: false));
        }
    }
}
