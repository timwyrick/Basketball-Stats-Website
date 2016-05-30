namespace FantasyBasketballDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Seasons : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Official",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstSeason = c.Int(nullable: false),
                        LastSeason = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Season",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.TeamSeason", "Season_ID", c => c.Int());
            AddColumn("dbo.Game", "Official_ID", c => c.Int());
            CreateIndex("dbo.TeamSeason", "Season_ID");
            CreateIndex("dbo.Game", "Official_ID");
            AddForeignKey("dbo.Game", "Official_ID", "dbo.Official", "ID");
            AddForeignKey("dbo.TeamSeason", "Season_ID", "dbo.Season", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamSeason", "Season_ID", "dbo.Season");
            DropForeignKey("dbo.Game", "Official_ID", "dbo.Official");
            DropIndex("dbo.Game", new[] { "Official_ID" });
            DropIndex("dbo.TeamSeason", new[] { "Season_ID" });
            DropColumn("dbo.Game", "Official_ID");
            DropColumn("dbo.TeamSeason", "Season_ID");
            DropTable("dbo.Season");
            DropTable("dbo.Official");
        }
    }
}
