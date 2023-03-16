using FootballUWPClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FootballUWPClient.Data
{
    internal interface ILeagueRepository
    {
        Task<List<League>> GetLeagues();
        Task<League> GetLeague(int ID);
    }
}
