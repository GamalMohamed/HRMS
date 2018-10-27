namespace CoEX_HRMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SettingEmployeesCountDefaultValue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Workloads", "EmployeesCount", c => c.Int(defaultValue: 0));
        }
        
        public override void Down()
        {
        }
    }
}
