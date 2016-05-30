namespace FantasyBasketballDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PlayerGame", "Player_ID", c => c.Int());
            AddColumn("dbo.Game", "AwayTeam_ID", c => c.Int());
            AddColumn("dbo.Game", "HomeTeam_ID", c => c.Int());
            AddColumn("dbo.TeamGame", "Team_ID", c => c.Int());
            CreateIndex("dbo.PlayerGame", "Player_ID");
            CreateIndex("dbo.Game", "AwayTeam_ID");
            CreateIndex("dbo.Game", "HomeTeam_ID");
            CreateIndex("dbo.TeamGame", "Team_ID");
            AddForeignKey("dbo.PlayerGame", "Player_ID", "dbo.Player", "ID");
            AddForeignKey("dbo.TeamGame", "Team_ID", "dbo.Team", "ID");
            AddForeignKey("dbo.Game", "AwayTeam_ID", "dbo.Team", "ID");
            AddForeignKey("dbo.Game", "HomeTeam_ID", "dbo.Team", "ID");
            DropColumn("dbo.Game", "HomeTeamID");
            DropColumn("dbo.Game", "AwayTeamID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Game", "AwayTeamID", c => c.Int(nullable: false));
            AddColumn("dbo.Game", "HomeTeamID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Game", "HomeTeam_ID", "dbo.Team");
            DropForeignKey("dbo.Game", "AwayTeam_ID", "dbo.Team");
            DropForeignKey("dbo.TeamGame", "Team_ID", "dbo.Team");
            DropForeignKey("dbo.PlayerGame", "Player_ID", "dbo.Player");
            DropIndex("dbo.TeamGame", new[] { "Team_ID" });
            DropIndex("dbo.Game", new[] { "HomeTeam_ID" });
            DropIndex("dbo.Game", new[] { "AwayTeam_ID" });
            DropIndex("dbo.PlayerGame", new[] { "Player_ID" });
            DropColumn("dbo.TeamGame", "Team_ID");
            DropColumn("dbo.Game", "HomeTeam_ID");
            DropColumn("dbo.Game", "AwayTeam_ID");
            DropColumn("dbo.PlayerGame", "Player_ID");
        }
    }
}
