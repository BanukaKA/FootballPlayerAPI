using FootballApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    [ModelMetadataType(typeof(PlayerMetaData))]
    public class PlayerDTO : IValidatableObject
    {
        public int ID { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Jersey { get; set; }

        public DateTime DOB { get; set; }

        public double FeePaid { get; set; }

        public string EMail { get; set; }

        public int TeamCount { get; set; }

        public Byte[] RowVersion { get; set; }

        public ICollection<PlayerTeamDTO> PlayerTeams { get; set; } = new HashSet<PlayerTeamDTO>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DOB < DateTime.Today.AddYears(-10) && FeePaid < 120.0)
            {
                yield return new ValidationResult("Players over 10 years old must pay a Fee of at least $120.", new[] { "FeePaid" });
            }
        }
    }
}
