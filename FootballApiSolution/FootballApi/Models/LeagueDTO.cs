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
    public class LeagueDTO
    {

        public int ID { get; set; }

        public string Name { get; set; }

        public int TeamCount { get; set; }

        public ICollection<TeamDTO> Teams { get; set; } = new HashSet<TeamDTO>();

    }

}
