using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FootballUWPClient.Models
{
    public class Team 
    {        
        public int ID { get; set; }

        public string Name { get; set; }

        public double Budget { get; set; }

        public int PlayerCount { get; set; }

        public int LeagueID { get; set; }
        public League League { get; set; }

        public ICollection<PlayerTeam> PlayerTeams { get; set; } = new HashSet<PlayerTeam>();

    }
}
