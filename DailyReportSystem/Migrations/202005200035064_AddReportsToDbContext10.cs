namespace DailyReportSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReportsToDbContext10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "Accepting", c => c.Int(nullable: false));
            //DropColumn("dbo.Reports", "AcceptStatus");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reports", "AcceptStatus", c => c.Int(nullable: false));
            //DropColumn("dbo.Reports", "Accepting");
        }
    }
}
