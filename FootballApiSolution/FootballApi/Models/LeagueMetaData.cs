using FootballApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FootballApi.Models
{
    public class LeagueMetaData
    {

        public int ID { get; set; }

        [Display(Name = "League Name")]
        [Required(ErrorMessage = "You cannot leave the league name blank.")]
        [StringLength(70, ErrorMessage = "League name cannot be more than 70 characters long.")]
        public string Name { get; set; }
        
        [Display(Name = "Teams")]
        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();

    }
}
