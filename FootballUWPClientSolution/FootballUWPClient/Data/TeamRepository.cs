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
    public class TeamRepository : ITeamRepository
    {
        private readonly HttpClient client = new HttpClient();
        public TeamRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Team>> GetTeams()
        {
            HttpResponseMessage response = await client.GetAsync("api/Teams");
            if (response.IsSuccessStatusCode)
            {
                List<Team> Teams = await response.Content.ReadAsAsync<List<Team>>();
                return Teams;
            }
            else
            {
                throw new Exception("Could not access the list of Teams.");
            }
        }

        public async Task<List<Team>> GetTeamsByLeague(int LeagueID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/teams/byLeague/{LeagueID}");
            if (response.IsSuccessStatusCode)
            {
                List<Team> Teams = await response.Content.ReadAsAsync<List<Team>>();
                return Teams;
            }
            else
            {
                throw new Exception("Could not access the list of Teams by League.");
            }
        }

        public async Task<Team> GetTeam(int ID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/Teams/{ID}");
            if (response.IsSuccessStatusCode)
            {
                Team Team = await response.Content.ReadAsAsync<Team>();
                return Team;
            }
            else
            {
                throw new Exception("Could not access that Team.");
            }
        }

        public async Task AddTeam(Team teamToAdd)
        {
            var response = await client.PostAsJsonAsync("/api/Teams", teamToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task UpdateTeam(Team teamToUpdate)
        {
            var response = await client.PutAsJsonAsync($"/api/Teams/{teamToUpdate.ID}", teamToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task DeleteTeam(Team teamToDelete)
        {
            var response = await client.DeleteAsync($"/api/Teams/{teamToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }


    }
}
