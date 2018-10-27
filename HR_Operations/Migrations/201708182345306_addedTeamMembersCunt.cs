namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedTeamMembersCunt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Teams", "MembersCount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Teams", "MembersCount");
        }
    }
}
