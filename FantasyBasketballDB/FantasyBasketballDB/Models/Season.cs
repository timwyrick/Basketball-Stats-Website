using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FantasyBasketballDB.Models
{
    public class Season
    {
        [Key]
        public int ID { get; set; }

        public int Year { get; set; }

        public IList<TeamSeason> TeamSeasons { get; set; }
    }
}