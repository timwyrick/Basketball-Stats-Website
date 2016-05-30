using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FantasyBasketballDB.Models
{
    public class Player
    {
        [Key]
        public int ID { get; set; }

        public int PlayerID { get; set; }

        public string Name { get; set; }

        public int DraftYear { get; set; }

        public int DraftPosition { get; set; }

        //Consider changing to an acutal object
        public string College { get; set; }

        public IList<Team> Teams { get; set; }

        public IList<PlayerSeason> Seasons { get; set; }

        //Career Statistics
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

        public int? PlusMinus { get; set; }

    }
}