using FootballApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FootballApi.Models
{
    public class PlayerTeamDTO
    {
        public int PlayerID { get; set; }
        public PlayerDTO Player { get; set; }


        public int TeamID { get; set; }
        public TeamDTO Team { get; set; }
    }
}
