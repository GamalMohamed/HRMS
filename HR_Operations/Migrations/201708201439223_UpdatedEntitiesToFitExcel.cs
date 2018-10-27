namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedEntitiesToFitExcel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Name", c => c.String());
            AlterColumn("dbo.Employees", "Email", c => c.String());
            AlterColumn("dbo.Employees", "PhoneNumber", c => c.String());
            AlterColumn("dbo.Employees", "Gender", c => c.String());
            AlterColumn("dbo.Employees", "MajorUniversity", c => c.String());
            AlterColumn("dbo.Employees", "GraduationYear", c => c.String());
            AlterColumn("dbo.Employees", "PreviousEmployer", c => c.String());
            AlterColumn("dbo.Employees", "Vendor", c => c.String());
            AlterColumn("dbo.Employees", "BasedOut", c => c.String());
            AlterColumn("dbo.Employees", "Subsidiary", c => c.String());
            AlterColumn("dbo.Employees", "ReportingManager", c => c.String());
            DropColumn("dbo.Employees", "Tech");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "Tech", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "ReportingManager", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "Subsidiary", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "BasedOut", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "Vendor", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "PreviousEmployer", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "GraduationYear", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "MajorUniversity", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "Gender", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "PhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "Name", c => c.String(nullable: false));
        }
    }
}
