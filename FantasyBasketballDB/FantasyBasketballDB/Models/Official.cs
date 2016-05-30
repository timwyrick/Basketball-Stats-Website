using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FantasyBasketballDB.Models
{
    public class Official
    {
        [Key]
        public int ID { get; set; }

        public IList<Game> Games { get; set; }

        public int FirstSeason { get; set; }

        public int LastSeason { get; set; }
    }
}