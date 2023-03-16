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
    public class LeaguesController : ControllerBase
    {
        private readonly FootballContext _context;

        public LeaguesController(FootballContext context)
        {
            _context = context;
        }

        // GET: api/Leagues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeagueDTO>>> GetLeagues()
        {
            return await _context.Leagues
                .Include(t => t.Teams)
                .Select(t => new LeagueDTO
                {
                    ID = t.ID,
                    Name = t.Name,
                    TeamCount = t.Teams.Count(),
                })
                .ToListAsync();
        }

        // GET: api/Leagues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeagueDTO>> GetLeague(int id)
        {
            var leagueDTO = await _context.Leagues
                .Include(t => t.Teams)
                .Select(t => new LeagueDTO
                {
                    ID = t.ID,
                    Name = t.Name,
                    TeamCount = t.Teams.Count(),
                })
                .FirstOrDefaultAsync(l => l.ID == id); ;

            if (leagueDTO == null)
            {
                return NotFound();
            }

            return leagueDTO;
        }

        // PUT: api/Leagues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeague(int id, LeagueDTO leagueDTO)
        {
            if (id != leagueDTO.ID)
            {
                return BadRequest(new { message = "Error: Incorrect ID for League." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var leagueToUpdate = await _context.Leagues.FindAsync(id);

            //Check that you got it
            if (leagueToUpdate == null)
            {
                return NotFound(new { message = "Error: League record not found." });
            }

            //Update the properties of the entity object from the DTO object
            leagueToUpdate.ID = leagueDTO.ID;
            leagueToUpdate.Name = leagueDTO.Name;
            
            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeagueExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: League has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: League has been updated by another user.  Back out and try editing the record again." });
                }
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
            }
        }

        // POST: api/Leagues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<League>> PostLeague(LeagueDTO leagueDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            League league = new League
            {
                Name = leagueDTO.Name
            };

            try
            {
                _context.Leagues.Add(league);
                await _context.SaveChangesAsync();
                //Assign Database Generated values back into the DTO
                leagueDTO.ID = league.ID;
                return CreatedAtAction(nameof(GetLeague), new { id = league.ID }, leagueDTO);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
            }
        }

        // DELETE: api/Leagues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLeague(int id)
        {
            var league = await _context.Leagues.FindAsync(id);
            if (league == null)
            {
                return NotFound(new { message = "Delete Error: League has already been removed." });
            }
            try
            {
                _context.Leagues.Remove(league);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    return BadRequest(new { message = "Delete Error: Remember, you cannot delete a League that has teams assigned." });
                }
                else
                {
                    return BadRequest(new { message = "Delete Error: Unable to delete League. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        private bool LeagueExists(int id)
        {
            return _context.Leagues.Any(e => e.ID == id);
        }
    }
}
