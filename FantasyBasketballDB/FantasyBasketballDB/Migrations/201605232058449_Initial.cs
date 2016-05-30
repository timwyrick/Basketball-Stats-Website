namespace FantasyBasketballDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Award",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Draft",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Player",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DraftYear = c.Int(nullable: false),
                        DraftPosition = c.Int(nullable: false),
                        College = c.String(),
                        Minutes = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        Rebounds = c.Int(nullable: false),
                        Assists = c.Int(nullable: false),
                        Steals = c.Int(nullable: false),
                        Blocks = c.Int(nullable: false),
                        Turnovers = c.Int(nullable: false),
                        Fouls = c.Int(nullable: false),
                        FieldGoals = c.Int(nullable: false),
                        FieldGoalAttempts = c.Int(nullable: false),
                        ThreePoints = c.Int(nullable: false),
                        ThreePointAttempts = c.Int(nullable: false),
                        FreeThrows = c.Int(nullable: false),
                        FreeThrowAttempts = c.Int(nullable: false),
                        PlusMinus = c.Int(nullable: false),
                        Draft_ID = c.Int(),
                        TeamSeason_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Draft", t => t.Draft_ID)
                .ForeignKey("dbo.TeamSeason", t => t.TeamSeason_ID)
                .Index(t => t.Draft_ID)
                .Index(t => t.TeamSeason_ID);
            
            CreateTable(
                "dbo.PlayerSeason",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Minutes = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        Rebounds = c.Int(nullable: false),
                        Assists = c.Int(nullable: false),
                        Steals = c.Int(nullable: false),
                        Blocks = c.Int(nullable: false),
                        Turnovers = c.Int(nullable: false),
                        Fouls = c.Int(nullable: false),
                        FieldGoals = c.Int(nullable: false),
                        FieldGoalAttempts = c.Int(nullable: false),
                        ThreePoints = c.Int(nullable: false),
                        ThreePointAttempts = c.Int(nullable: false),
                        FreeThrows = c.Int(nullable: false),
                        FreeThrowAttempts = c.Int(nullable: false),
                        PlusMinus = c.Int(nullable: false),
                        Player_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Player", t => t.Player_ID)
                .Index(t => t.Player_ID);
            
            CreateTable(
                "dbo.PlayerGame",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.Int(nullable: false),
                        Minutes = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        Rebounds = c.Int(nullable: false),
                        Assists = c.Int(nullable: false),
                        Steals = c.Int(nullable: false),
                        Blocks = c.Int(nullable: false),
                        Turnovers = c.Int(nullable: false),
                        Fouls = c.Int(nullable: false),
                        FieldGoals = c.Int(nullable: false),
                        FieldGoalAttempts = c.Int(nullable: false),
                        ThreePoints = c.Int(nullable: false),
                        ThreePointAttempts = c.Int(nullable: false),
                        FreeThrows = c.Int(nullable: false),
                        FreeThrowAttempts = c.Int(nullable: false),
                        PlusMinus = c.Int(nullable: false),
                        PlayerSeason_ID = c.Int(),
                        TeamGame_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.PlayerSeason", t => t.PlayerSeason_ID)
                .ForeignKey("dbo.TeamGame", t => t.TeamGame_ID)
                .Index(t => t.PlayerSeason_ID)
                .Index(t => t.TeamGame_ID);
            
            CreateTable(
                "dbo.Game",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.String(),
                        GameID = c.Int(nullable: false),
                        Season = c.Int(nullable: false),
                        HomeTeamID = c.Int(nullable: false),
                        AwayTeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TeamGame",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstQuarterPoints = c.Int(nullable: false),
                        SecondQuarterPoints = c.Int(nullable: false),
                        ThirdQuarterPoints = c.Int(nullable: false),
                        FourthQuarterPoints = c.Int(nullable: false),
                        Overtime1Points = c.Int(nullable: false),
                        Overtime2Points = c.Int(nullable: false),
                        Overtime3Points = c.Int(nullable: false),
                        Overtime4Points = c.Int(nullable: false),
                        Overtime5Points = c.Int(nullable: false),
                        TeamSeason_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TeamSeason", t => t.TeamSeason_ID)
                .Index(t => t.TeamSeason_ID);
            
            CreateTable(
                "dbo.Team",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TeamID = c.Int(nullable: false),
                        Name = c.String(),
                        City = c.String(),
                        YearEstablished = c.Int(nullable: false),
                        YearClosed = c.Int(nullable: false),
                        Wins = c.Int(nullable: false),
                        Losses = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.TeamSeason",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Wins = c.Int(nullable: false),
                        Losses = c.Int(nullable: false),
                        Points = c.Int(nullable: false),
                        Rebounds = c.Int(nullable: false),
                        Assists = c.Int(nullable: false),
                        Steals = c.Int(nullable: false),
                        Blocks = c.Int(nullable: false),
                        Turnovers = c.Int(nullable: false),
                        Fouls = c.Int(nullable: false),
                        FieldGoals = c.Int(nullable: false),
                        FieldGoalAttempts = c.Int(nullable: false),
                        ThreePoints = c.Int(nullable: false),
                        ThreePointAttempts = c.Int(nullable: false),
                        FreeThrows = c.Int(nullable: false),
                        FreeThrowAttempts = c.Int(nullable: false),
                        Team_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Team", t => t.Team_ID)
                .Index(t => t.Team_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TeamSeason", "Team_ID", "dbo.Team");
            DropForeignKey("dbo.Player", "TeamSeason_ID", "dbo.TeamSeason");
            DropForeignKey("dbo.TeamGame", "TeamSeason_ID", "dbo.TeamSeason");
            DropForeignKey("dbo.PlayerGame", "TeamGame_ID", "dbo.TeamGame");
            DropForeignKey("dbo.Player", "Draft_ID", "dbo.Draft");
            DropForeignKey("dbo.PlayerSeason", "Player_ID", "dbo.Player");
            DropForeignKey("dbo.PlayerGame", "PlayerSeason_ID", "dbo.PlayerSeason");
            DropIndex("dbo.TeamSeason", new[] { "Team_ID" });
            DropIndex("dbo.TeamGame", new[] { "TeamSeason_ID" });
            DropIndex("dbo.PlayerGame", new[] { "TeamGame_ID" });
            DropIndex("dbo.PlayerGame", new[] { "PlayerSeason_ID" });
            DropIndex("dbo.PlayerSeason", new[] { "Player_ID" });
            DropIndex("dbo.Player", new[] { "TeamSeason_ID" });
            DropIndex("dbo.Player", new[] { "Draft_ID" });
            DropTable("dbo.TeamSeason");
            DropTable("dbo.Team");
            DropTable("dbo.TeamGame");
            DropTable("dbo.Game");
            DropTable("dbo.PlayerGame");
            DropTable("dbo.PlayerSeason");
            DropTable("dbo.Player");
            DropTable("dbo.Draft");
            DropTable("dbo.Award");
        }
    }
}
