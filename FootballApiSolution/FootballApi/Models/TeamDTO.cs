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
    [ModelMetadataType(typeof(TeamMetaData))]
    public class TeamDTO : IValidatableObject
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public double Budget { get; set; }

        public int PlayerCount { get; set; }

        public int LeagueID { get; set; }
        public LeagueDTO League { get; set; }

        public ICollection<PlayerTeamDTO> PlayerTeams { get; set; } = new HashSet<PlayerTeamDTO>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (Name[0] == 'X' || Name[0] == 'F' || Name[0] == 'S')
            {
                yield return new ValidationResult("Team names are not allowed to start with the letters X, F, or S.", new[] { "Name" });
            }
            if ((Budget < 500) || (Budget > 10000))
            {
                yield return new ValidationResult("Team names are not allowed to have a budget below $500.00 or above $10000.00.", new[] { "Name" });
            }
        }
    }
}
