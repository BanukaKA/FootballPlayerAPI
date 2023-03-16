using FootballUWPClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FootballUWPClient.Data
{
    public interface ITeamRepository
    {
        Task<List<Team>> GetTeams();
        Task<Team> GetTeam(int ID);
        Task<List<Team>> GetTeamsByLeague(int LeagueID);
        Task AddTeam(Team teamToAdd);
        Task UpdateTeam(Team teamToUpdate);
        Task DeleteTeam(Team teamToDelete);
    }
}
