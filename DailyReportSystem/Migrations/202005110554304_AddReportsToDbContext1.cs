namespace DailyReportSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReportsToDbContext1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "WorkTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reports", "LeaveTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Reports", "CliantCompany", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Reports", "CliantPIC", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Reports", "CliantStatus", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reports", "CliantStatus");
            DropColumn("dbo.Reports", "CliantPIC");
            DropColumn("dbo.Reports", "CliantCompany");
            DropColumn("dbo.Reports", "LeaveTime");
            DropColumn("dbo.Reports", "WorkTime");
        }
    }
}
