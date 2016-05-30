using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FantasyBasketballDB.Models
{
    public class TeamSeason
    {
        [Key]
        public int ID { get; set; }

        public int Year { get; set; }

        public Team Team { get; set; }

        public IList<TeamGame> Games { get; set; }

        public IList<Player> Players { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        //Team statistics
        public int? Minutes { get; set; }

        public int? Points { get; set; }

        public int? Rebounds { get; set; }

        public int? OffensiveRebounds { get; set; }

        public int? DefensiveRebounds { get; set; }

        public int? Assists { get; set; }

        public int? Steals { get; set; }

        public int? Blocks { get; set; }

        public int? Turnovers { get; set; }

        public int? Fouls { get; set; }

        public int? FieldGoals { get; set; }

        public int? FieldGoalAttempts { get; set; }

        public int? ThreePoints { get; set; }

        public int? ThreePointAttempts { get; set; }

        public int? FreeThrows { get; set; }

        public int? FreeThrowAttempts { get; set; }
    }
}