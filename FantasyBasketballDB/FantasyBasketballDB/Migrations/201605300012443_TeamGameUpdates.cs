namespace FantasyBasketballDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TeamGameUpdates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TeamGame", "Team_ID", "dbo.Team");
            DropForeignKey("dbo.Game", "AwayTeam_ID", "dbo.Team");
            DropForeignKey("dbo.Game", "HomeTeam_ID", "dbo.Team");
            DropIndex("dbo.TeamGame", new[] { "Team_ID" });
            DropIndex("dbo.Game", new[] { "AwayTeam_ID" });
            DropIndex("dbo.Game", new[] { "HomeTeam_ID" });
            AddColumn("dbo.TeamGame", "Game_ID", c => c.Int());
            AddColumn("dbo.Game", "AwayTeamGame_ID", c => c.Int());
            AddColumn("dbo.Game", "HomeTeamGame_ID", c => c.Int());
            CreateIndex("dbo.TeamGame", "Game_ID");
            CreateIndex("dbo.Game", "AwayTeamGame_ID");
            CreateIndex("dbo.Game", "HomeTeamGame_ID");
            AddForeignKey("dbo.Game", "AwayTeamGame_ID", "dbo.TeamGame", "ID");
            AddForeignKey("dbo.Game", "HomeTeamGame_ID", "dbo.TeamGame", "ID");
            AddForeignKey("dbo.TeamGame", "Game_ID", "dbo.Game", "ID");
            DropColumn("dbo.TeamGame", "GameID");
            DropColumn("dbo.TeamGame", "Team_ID");
            DropColumn("dbo.Game", "AwayTeam_ID");
            DropColumn("dbo.Game", "HomeTeam_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Game", "HomeTeam_ID", c => c.Int());
            AddColumn("dbo.Game", "AwayTeam_ID", c => c.Int());
            AddColumn("dbo.TeamGame", "Team_ID", c => c.Int());
            AddColumn("dbo.TeamGame", "GameID", c => c.Int(nullable: false));
            DropForeignKey("dbo.TeamGame", "Game_ID", "dbo.Game");
            DropForeignKey("dbo.Game", "HomeTeamGame_ID", "dbo.TeamGame");
            DropForeignKey("dbo.Game", "AwayTeamGame_ID", "dbo.TeamGame");
            DropIndex("dbo.Game", new[] { "HomeTeamGame_ID" });
            DropIndex("dbo.Game", new[] { "AwayTeamGame_ID" });
            DropIndex("dbo.TeamGame", new[] { "Game_ID" });
            DropColumn("dbo.Game", "HomeTeamGame_ID");
            DropColumn("dbo.Game", "AwayTeamGame_ID");
            DropColumn("dbo.TeamGame", "Game_ID");
            CreateIndex("dbo.Game", "HomeTeam_ID");
            CreateIndex("dbo.Game", "AwayTeam_ID");
            CreateIndex("dbo.TeamGame", "Team_ID");
            AddForeignKey("dbo.Game", "HomeTeam_ID", "dbo.Team", "ID");
            AddForeignKey("dbo.Game", "AwayTeam_ID", "dbo.Team", "ID");
            AddForeignKey("dbo.TeamGame", "Team_ID", "dbo.Team", "ID");
        }
    }
}
