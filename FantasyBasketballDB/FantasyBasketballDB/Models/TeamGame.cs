using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FantasyBasketballDB.Models
{
    public class TeamGame
    {
        [Key]
        public int ID { get; set; }

        public TeamSeason TeamSeason { get; set; }

        public Game Game { get; set; }

        public bool Win { get; set; }

        public IList<PlayerGame> Players { get; set; }

        public int? FirstQuarterPoints { get; set; }

        public int? SecondQuarterPoints { get; set; }

        public int? ThirdQuarterPoints { get; set; }

        public int? FourthQuarterPoints { get; set; }

        public int? Overtime1Points { get; set; }

        public int? Overtime2Points { get; set; }

        public int? Overtime3Points { get; set; }

        public int? Overtime4Points { get; set; }

        public int? Overtime5Points { get; set; }

        public int? Overtime6Points { get; set; }

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