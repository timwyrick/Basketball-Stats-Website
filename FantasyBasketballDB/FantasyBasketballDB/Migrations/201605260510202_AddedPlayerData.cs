namespace FantasyBasketballDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPlayerData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Player", "OffensiveRebounds", c => c.Int(nullable: false));
            AddColumn("dbo.Player", "DefensiveRebounds", c => c.Int(nullable: false));
            AddColumn("dbo.PlayerSeason", "OffensiveRebounds", c => c.Int(nullable: false));
            AddColumn("dbo.PlayerSeason", "DefensiveRebounds", c => c.Int(nullable: false));
            AddColumn("dbo.PlayerGame", "GameID", c => c.Int(nullable: false));
            AddColumn("dbo.PlayerGame", "OffensiveRebounds", c => c.Int(nullable: false));
            AddColumn("dbo.PlayerGame", "DefensiveRebounds", c => c.Int(nullable: false));
            AddColumn("dbo.TeamSeason", "Minutes", c => c.Int(nullable: false));
            AddColumn("dbo.TeamSeason", "OffensiveRebounds", c => c.Int(nullable: false));
            AddColumn("dbo.TeamSeason", "DefensiveRebounds", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "OffensiveRebounds", c => c.Int(nullable: false));
            AddColumn("dbo.TeamGame", "DefensiveRebounds", c => c.Int(nullable: false));
            AlterColumn("dbo.PlayerGame", "Date", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PlayerGame", "Date", c => c.Int(nullable: false));
            DropColumn("dbo.TeamGame", "DefensiveRebounds");
            DropColumn("dbo.TeamGame", "OffensiveRebounds");
            DropColumn("dbo.TeamSeason", "DefensiveRebounds");
            DropColumn("dbo.TeamSeason", "OffensiveRebounds");
            DropColumn("dbo.TeamSeason", "Minutes");
            DropColumn("dbo.PlayerGame", "DefensiveRebounds");
            DropColumn("dbo.PlayerGame", "OffensiveRebounds");
            DropColumn("dbo.PlayerGame", "GameID");
            DropColumn("dbo.PlayerSeason", "DefensiveRebounds");
            DropColumn("dbo.PlayerSeason", "OffensiveRebounds");
            DropColumn("dbo.Player", "DefensiveRebounds");
            DropColumn("dbo.Player", "OffensiveRebounds");
        }
    }
}
