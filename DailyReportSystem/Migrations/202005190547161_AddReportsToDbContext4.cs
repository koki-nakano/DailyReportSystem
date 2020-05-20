namespace DailyReportSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReportsToDbContext4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "Accepting", c => c.String());
            AddColumn("dbo.Reports", "comment", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reports", "comment");
            DropColumn("dbo.Reports", "Accepting");
        }
    }
}
