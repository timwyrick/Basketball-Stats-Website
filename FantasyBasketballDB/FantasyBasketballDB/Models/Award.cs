using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FantasyBasketballDB.Models
{
    public class Award
    {
        [Key]
        public int ID { get; set; }

        public int Date { get; set; }

        public string Description { get; set; }
    }
}