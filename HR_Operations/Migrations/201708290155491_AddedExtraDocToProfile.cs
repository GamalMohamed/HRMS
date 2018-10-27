namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedExtraDocToProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "ExtraDocument", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "ExtraDocument");
        }
    }
}
