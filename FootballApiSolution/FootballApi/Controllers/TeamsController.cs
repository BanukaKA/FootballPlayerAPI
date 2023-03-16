using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballApi.Data;
using FootballApi.Models;
using System.Numerics;

namespace FootballApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly FootballContext _context;

        public TeamsController(FootballContext context)
        {
            _context = context;
        }

        // GET: api/Teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeams()
        {
            return await _context.Teams
                .Include(t => t.League)
                .Select(t => new TeamDTO
                {
                    ID = t.ID,
                    Name = t.Name,
                    Budget = t.Budget,
                    PlayerCount = t.PlayerTeams.Count(),
                    LeagueID = t.LeagueID,
                    League = new LeagueDTO
                    {
                        ID = t.League.ID,
                        Name = t.League.Name                        
                    }
                })
                .ToListAsync();
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDTO>> GetTeam(int id)
        {
            var teamDTO = await _context.Teams
                .Include(t => t.League)
                .Select(t => new TeamDTO
                {
                    ID = t.ID,
                    Name = t.Name,
                    Budget = t.Budget,
                    PlayerCount = t.PlayerTeams.Count(),
                    LeagueID = t.LeagueID,
                    League = new LeagueDTO
                    {
                        ID = t.League.ID,
                        Name = t.League.Name
                    }
                })
                .FirstOrDefaultAsync(t => t.ID == id);

            if (teamDTO == null)
            {
                return NotFound();
            }

            return teamDTO;
        }

        [HttpGet("ByLeague/{id}")]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeamsByLeague(int id)
        {
            var teamDTOs = await _context.Teams
                .Include(t => t.League)
                .Select(t => new TeamDTO
                {
                    ID = t.ID,
                    Name = t.Name,
                    Budget = t.Budget,
                    PlayerCount = t.PlayerTeams.Count(),
                    LeagueID = t.LeagueID,
                    League = new LeagueDTO
                    {
                        ID = t.League.ID,
                        Name = t.League.Name
                    }
                })
                .Where(p => p.LeagueID == id)
                .ToListAsync();

            if (teamDTOs.Count() > 0)
            {
                return teamDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Team records for that League." });
            }
        }

        // PUT: api/Teams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(int id, TeamDTO teamDTO)
        {
            if (id != teamDTO.ID)
            {
                return BadRequest(new { message = "Error: Incorrect ID for Team." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var teamToUpdate = await _context.Teams.FindAsync(id);

            //Check that you got it
            if (teamToUpdate == null)
            {
                return NotFound(new { message = "Error: Team record not found." });
            }


            //Update the properties of the entity object from the DTO object
            teamToUpdate.ID = teamDTO.ID;
            teamToUpdate.Name = teamDTO.Name;
            teamToUpdate.Budget = teamDTO.Budget;
            teamToUpdate.LeagueID = teamDTO.LeagueID;

            

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Team has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Team has been updated by another user.  Back out and try editing the record again." });
                }
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
            }
        }

        // POST: api/Teams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Team>> PostTeam(TeamDTO teamDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Team team = new Team
            {
                Name = teamDTO.Name,
                Budget = teamDTO.Budget,
                LeagueID = teamDTO.LeagueID
            };

            try
            {
                _context.Teams.Add(team);
                await _context.SaveChangesAsync();
                //Assign Database Generated values back into the DTO
                teamDTO.ID = team.ID;
                return CreatedAtAction(nameof(GetTeam), new { id = team.ID }, teamDTO);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
            }
        }

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound(new { message = "Delete Error: Team has already been removed." });
            }
            try
            {
                _context.Teams.Remove(team);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    return BadRequest(new { message = "Delete Error: Remember, you cannot delete a Team that has league assigned." });
                }
                else
                {
                    return BadRequest(new { message = "Delete Error: Unable to delete Teams. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.ID == id);
        }
    }
}
