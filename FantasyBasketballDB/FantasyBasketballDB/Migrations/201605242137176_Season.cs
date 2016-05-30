namespace FantasyBasketballDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Season : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeamPlayer",
                c => new
                    {
                        Team_ID = c.Int(nullable: false),
                        Player_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_ID, t.Player_ID })
                .ForeignKey("dbo.Team", t => t.Team_ID, cascadeDelete: true)
                .ForeignKey("dbo.Player", t => t.Player_ID, cascadeDelete: true)
                .Index(t => t.Team_ID)
                .Index(t => t.Player_ID);
            
            AddColumn("dbo.PlayerSeason", "TeamPlayedFor_ID", c => c.Int());
            CreateIndex("dbo.PlayerSeason", "TeamPlayedFor_ID");
            AddForeignKey("dbo.PlayerSeason", "TeamPlayedFor_ID", "dbo.Team", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerSeason", "TeamPlayedFor_ID", "dbo.Team");
            DropForeignKey("dbo.TeamPlayer", "Player_ID", "dbo.Player");
            DropForeignKey("dbo.TeamPlayer", "Team_ID", "dbo.Team");
            DropIndex("dbo.TeamPlayer", new[] { "Player_ID" });
            DropIndex("dbo.TeamPlayer", new[] { "Team_ID" });
            DropIndex("dbo.PlayerSeason", new[] { "TeamPlayedFor_ID" });
            DropColumn("dbo.PlayerSeason", "TeamPlayedFor_ID");
            DropTable("dbo.TeamPlayer");
        }
    }
}
