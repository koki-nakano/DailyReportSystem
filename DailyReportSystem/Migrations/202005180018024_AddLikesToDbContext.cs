namespace DailyReportSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLikesToDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        EmployeeId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Likes");
        }
    }
}
