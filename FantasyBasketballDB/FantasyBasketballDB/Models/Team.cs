using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FantasyBasketballDB.Models
{
    public class Team
    {
        [Key]
        public int ID { get; set; }

        public int TeamID { get; set; }

        public string Name { get; set; }

        public string NickName { get; set; }

        public string City { get; set; }

        public int YearEstablished { get; set; }

        public int YearClosed { get; set; }

        public IList<Player> Players { get; set; }

        public IList<TeamSeason> Seasons { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }
    }
}