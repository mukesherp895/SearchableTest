namespace SearchableTest.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class altercontestrating : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ContestantRating", "RatedDate", c => c.DateTime(defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ContestantRating", "RatedDate", c => c.DateTime(nullable: false));
        }
    }
}
