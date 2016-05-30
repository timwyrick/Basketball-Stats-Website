namespace FantasyBasketballDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayerID : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Player", "PlayerID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Player", "PlayerID");
        }
    }
}
