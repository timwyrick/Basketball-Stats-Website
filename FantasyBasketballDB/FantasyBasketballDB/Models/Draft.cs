using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FantasyBasketballDB.Models
{
    public class Draft
    {
        [Key]
        public int ID { get; set; }

        public string Date { get; set; }

        public IList<Player> Players { get; set; }
    }
}