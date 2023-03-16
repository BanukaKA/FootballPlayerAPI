using FootballApi.Models;
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
    public class TeamMetaData : IValidatableObject
    {

        public int ID { get; set; }

        [Display(Name = "Team Name")]
        [Required(ErrorMessage = "You cannot leave the team name blank.")]
        [StringLength(70, ErrorMessage = "Team name cannot be more than 70 characters long.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You cannot leave the Budget blank.")]
        [Range(500.0, 10000.0, ErrorMessage = "Budget must be between $500 and $10,000.")]
        [DataType(DataType.Currency)]
        public double Budget { get; set; }

        [Display(Name = "League")]
        [Required(ErrorMessage = "You must select the League.")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a League.")]
        public int LeagueID { get; set; }

        [Display(Name = "Players")]        
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
