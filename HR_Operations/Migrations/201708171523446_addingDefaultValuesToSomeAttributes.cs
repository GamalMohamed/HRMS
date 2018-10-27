namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingDefaultValuesToSomeAttributes : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Roles", "MembersCount");
            DropColumn("dbo.Roles", "LastUpdate");
            DropColumn("dbo.Teams", "MembersCount");
            DropColumn("dbo.Teams", "LastUpdate");
            AlterColumn("dbo.Employees", "IsResigned", c => c.Boolean(nullable: false, defaultValue: false));
            AlterColumn("dbo.Employees", "IsGoodAttrition", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            AddColumn("dbo.Teams", "LastUpdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Teams", "MembersCount", c => c.Int(nullable: false));
            AddColumn("dbo.Roles", "LastUpdate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Roles", "MembersCount", c => c.Int(nullable: false));
            AlterColumn("dbo.Employees", "IsResigned", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Employees", "IsGoodAttrition", c => c.Boolean(nullable: false));
        }
    }
}
