using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FantasyBasketballDB.Models
{
    public class Game
    {

        [Key]
        public int ID { get; set; }

        public string Date { get; set; }

        public int GameID { get; set; }

        public int Season { get; set; }

        public TeamGame HomeTeamGame { get; set; }

        public TeamGame AwayTeamGame { get; set; }

        
    }
}