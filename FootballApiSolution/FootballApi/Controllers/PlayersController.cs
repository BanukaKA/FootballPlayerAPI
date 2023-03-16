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
    public class PlayersController : ControllerBase
    {
        private readonly FootballContext _context;

        public PlayersController(FootballContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayers()
        {
            return await _context.Players
                .Include(p => p.PlayerTeams)
                .ThenInclude(p => p.Team)
                .Select(p => new PlayerDTO
                {
                    ID = p.ID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Jersey = p.Jersey,
                    DOB = p.DOB,
                    FeePaid = p.FeePaid,
                    EMail = p.EMail,
                    RowVersion = p.RowVersion,
                    TeamCount = p.PlayerTeams.Count(),
                    PlayerTeams = p.PlayerTeams.Select(t => new PlayerTeamDTO
                    {
                        PlayerID = t.PlayerID,
                        TeamID = t.TeamID,
                        Team = new TeamDTO
                        {
                            ID = t.Team.ID,
                            Name = t.Team.Name,
                            Budget = t.Team.Budget,
                            LeagueID = t.Team.LeagueID
                        }
                    }).ToList()
                })
                .ToListAsync();

        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerDTO>> GetPlayer(int id)
        {
            var playerDTO = await _context.Players
                .Include(p => p.PlayerTeams)
                .ThenInclude(pt => pt.Team)
                .Select(p => new PlayerDTO
                {
                    ID = p.ID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Jersey = p.Jersey,
                    DOB = p.DOB,
                    FeePaid = p.FeePaid,
                    EMail = p.EMail,
                    RowVersion = p.RowVersion,
                    TeamCount = p.PlayerTeams.Count(),
                    PlayerTeams = p.PlayerTeams.Select(t => new PlayerTeamDTO
                    {
                        PlayerID = t.PlayerID,
                        TeamID = t.TeamID,
                        Team = new TeamDTO
                        {
                            ID = t.Team.ID,
                            Name = t.Team.Name,
                            Budget = t.Team.Budget,
                            LeagueID = t.Team.LeagueID
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync(p => p.ID == id);

            if (playerDTO == null)
            {
                return NotFound();
            }

            return playerDTO;
        }

        // GET BY TEAMID: api/Players/5
        [HttpGet("ByTeam/{id}")]
        public async Task<ActionResult<IEnumerable<PlayerDTO>>> GetPlayersByTeam(int id)
        {
            var playerDTOs = await _context.Players
                .Include(p => p.PlayerTeams)
                .ThenInclude(pt => pt.Team)
                .Select(p => new PlayerDTO
                {
                    ID = p.ID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Jersey = p.Jersey,
                    DOB = p.DOB,
                    FeePaid = p.FeePaid,
                    EMail = p.EMail,
                    RowVersion = p.RowVersion,
                    TeamCount = p.PlayerTeams.Count(),
                    PlayerTeams = p.PlayerTeams.Select(t => new PlayerTeamDTO
                    {
                        PlayerID = t.PlayerID,
                        TeamID = t.TeamID,
                        Team = new TeamDTO
                        {
                            ID = t.Team.ID,
                            Name = t.Team.Name,
                            Budget = t.Team.Budget,
                            LeagueID = t.Team.LeagueID
                        }
                    }).ToList()
                })
                .Where(p => p.PlayerTeams
                .Any(p => p.TeamID == id))                
                .ToListAsync();

            if(playerDTOs.Count() > 0)
            {
                return playerDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Player records for that Team." });
            }
        }

        // PUT: api/Players/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, PlayerDTO playerDTO)
        {
            if (id != playerDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match Player" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var playerToUpdate = await _context.Players.FindAsync(id);

            //Check that you got it
            if (playerToUpdate == null)
            {
                return NotFound(new { message = "Error: Player record not found." });
            }

            //Wow, we have a chance to check for concurrency even before bothering
            //the database!  Of course, it will get checked again in the database just in case
            //it changes after we pulled the record.  
            //Note using SequenceEqual becuase it is an array after all.
            if (playerDTO.RowVersion != null)
            {
                if (!playerToUpdate.RowVersion.SequenceEqual(playerDTO.RowVersion))
                {
                    return Conflict(new { message = "Concurrency Error: Player has been changed by another user.  Try editing the record again." });
                }
            }

            //playerToUpdate = playerDTO; //- Fix with MappingGenerator

            //Update the properties of the entity object from the DTO object
            playerToUpdate.ID = playerDTO.ID;
            playerToUpdate.FirstName = playerDTO.FirstName;
            playerToUpdate.LastName = playerDTO.LastName;
            playerToUpdate.Jersey = playerDTO.Jersey;
            playerToUpdate.DOB = playerDTO.DOB;
            playerToUpdate.EMail = playerDTO.EMail;
            playerToUpdate.RowVersion = playerDTO.RowVersion;

            //Put the original RowVersion value in the OriginalValues collection for the entity
            _context.Entry(playerToUpdate).Property("RowVersion").OriginalValue = playerDTO.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Player has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Player has been updated by another user.  Back out and try editing the record again." });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate OHIP number." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // POST: api/Players
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(PlayerDTO playerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Player player = new Player { };    //Right click in { } and Fix with MappingGenerator

            Player player = new Player
            {
                ID = playerDTO.ID,
                FirstName = playerDTO.FirstName,
                LastName = playerDTO.LastName,
                Jersey = playerDTO.Jersey,
                DOB = playerDTO.DOB,
                EMail = playerDTO.EMail,
                RowVersion = playerDTO.RowVersion
            };


            try
            {
                _context.Players.Add(player);
                await _context.SaveChangesAsync();

                //Assign Database Generated values back into the DTO
                playerDTO.ID = player.ID;
                playerDTO.RowVersion = player.RowVersion;

                return CreatedAtAction(nameof(GetPlayer), new { id = player.ID }, playerDTO);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Email Address." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound(new { message = "Delete Error: Player has already been removed." });
            }
            try
            {
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    return BadRequest(new { message = "Delete Error: Remember, you cannot delete a Player that has teams assigned." });
                }
                else
                {
                    return BadRequest(new { message = "Delete Error: Unable to delete Player. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.ID == id);
        }
    }
}
