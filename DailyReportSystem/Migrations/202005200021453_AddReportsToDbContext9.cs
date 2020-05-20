namespace DailyReportSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReportsToDbContext9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reports", "Comment", c => c.String());
            DropColumn("dbo.Reports", "Comments");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Reports", "Comments", c => c.String());
            DropColumn("dbo.Reports", "Comment");
        }
    }
}
