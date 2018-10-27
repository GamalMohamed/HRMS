namespace HR_Staffing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedApplicantResume : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormResponses", "ApplicantResume", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FormResponses", "ApplicantResume");
        }
    }
}
