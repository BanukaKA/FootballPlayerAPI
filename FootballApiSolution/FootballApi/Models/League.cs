using FootballApi.Models;
using Microsoft.AspNetCore.Mvc;
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
    [ModelMetadataType(typeof(LeagueMetaData))]
    public class League : Auditable
    {

        public int ID { get; set; }

        public string Name { get; set; }
        
        public ICollection<Team> Teams { get; set; } = new HashSet<Team>();

    }
}
