using System;
using System.Net.Http;
using FootballUWPClient.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballUWPClient.Utilities;
using System.Net.Http.Headers;
using System.Numerics;

namespace FootballUWPClient.Data
{
    internal class LeagueRepository : ILeagueRepository
    {
        private readonly HttpClient client = new HttpClient();

        public LeagueRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<League>> GetLeagues()
        {
            HttpResponseMessage response = await client.GetAsync("api/leagues");
            if (response.IsSuccessStatusCode)
            {
                List<League> leagues = await response.Content.ReadAsAsync<List<League>>();
                return leagues;
            }
            else
            {
                throw new Exception("Could not access the list of Leagues.");
            }
        }
        public async Task<League> GetLeague(int LeagueID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/leagues/{LeagueID}");
            if (response.IsSuccessStatusCode)
            {
                League league = await response.Content.ReadAsAsync<League>();
                return league;
            }
            else
            {
                throw new Exception("Could not access that League.");
            }
        }
    }
}
