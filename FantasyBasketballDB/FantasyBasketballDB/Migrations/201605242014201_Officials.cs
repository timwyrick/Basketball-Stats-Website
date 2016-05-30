namespace FantasyBasketballDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Officials : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Team", "NickName", c => c.String());
            AddColumn("dbo.TeamGame", "GameID", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "Win", c => c.Boolean(nullable: false));
            AddColumn("dbo.TeamGame", "Minutes", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "Points", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "Rebounds", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "Assists", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "Steals", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "Blocks", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "Turnovers", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "Fouls", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "FieldGoals", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "FieldGoalAttempts", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "ThreePoints", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "ThreePointAttempts", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "FreeThrows", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "FreeThrowAttempts", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TeamGame", "FreeThrowAttempts");
            DropColumn("dbo.TeamGame", "FreeThrows");
            DropColumn("dbo.TeamGame", "ThreePointAttempts");
            DropColumn("dbo.TeamGame", "ThreePoints");
            DropColumn("dbo.TeamGame", "FieldGoalAttempts");
            DropColumn("dbo.TeamGame", "FieldGoals");
            DropColumn("dbo.TeamGame", "Fouls");
            DropColumn("dbo.TeamGame", "Turnovers");
            DropColumn("dbo.TeamGame", "Blocks");
            DropColumn("dbo.TeamGame", "Steals");
            DropColumn("dbo.TeamGame", "Assists");
            DropColumn("dbo.TeamGame", "Rebounds");
            DropColumn("dbo.TeamGame", "Points");
            DropColumn("dbo.TeamGame", "Minutes");
            DropColumn("dbo.TeamGame", "Win");
            DropColumn("dbo.TeamGame", "GameID");
            DropColumn("dbo.Team", "NickName");
        }
    }
}
