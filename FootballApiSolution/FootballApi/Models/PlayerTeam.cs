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
    public class PlayerTeam
    {
        public int PlayerID { get; set; }
        public Player Player { get; set; }


        public int TeamID { get; set; }
        public Team Team { get; set; }
    }
}
