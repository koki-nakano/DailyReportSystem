namespace DailyReportSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReportsToDbContext8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "Comments", c => c.String());
            AddColumn("dbo.Reports", "AcceptStatus", c => c.Int(nullable: false));
            DropColumn("dbo.Reports", "Comment");
            DropColumn("dbo.Reports", "Accepting");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reports", "Accepting", c => c.Int(nullable: false));
            AddColumn("dbo.Reports", "Comment", c => c.String());
            DropColumn("dbo.Reports", "AcceptStatus");
            DropColumn("dbo.Reports", "Comments");
        }
    }
}
