using FantasyBasketballDB.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
namespace FantasyBasketballDB.DAL
{
    public class FantasyBasketballDBContext : DbContext
    {
        public FantasyBasketballDBContext() : base("FantasyBasketballDBContext")
        {
            
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerGame> PlayerGames { get; set; }
        public DbSet<TeamGame> TeamGames { get; set; }
        public DbSet<PlayerSeason> PlayerSeasons { get; set; }
        public DbSet<TeamSeason> TeamSeasons { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Draft> Drafts { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Official> Officials { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}