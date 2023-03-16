using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FootballUWPClient.Models
{
    public class League
    {

        public int ID { get; set; }

        public string Name { get; set; }
        
        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();

    }
}
