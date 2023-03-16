using Microsoft.EntityFrameworkCore;
using FootballApi.Models;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;

namespace FootballApi.Data
{
    public static class FootballInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            FootballContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<FootballContext>();

            try
            {
                //Ensureing the data deletion
                //context.Database.EnsureDeleted();

                //Create the database if it does not exist
                context.Database.Migrate();

                //This approach to seeding data uses int and string arrays with loops to
                //create the data using random values
                Random random = new Random();

                //Prepare some string arrays for building objects
                string[] firstNames = new string[] { "Fred", "Barney", "Wilma", "Betty", "Dave", "Tim", "Elton", "Paul", "Shania", "Bruce" };
                string[] lastsNames = new string[] { "Stovell", "Jones", "Bloggs", "Flintstone", "Rubble", "Brown", "John", "McCartney", "Twain", "Cockburn" };
                string[] teams = new string[] { "Chicago White Sox", "Cleveland Guardians", "Chicago Cubs", "Cincinnati Reds", "Baltimore Orioles", "Boston Red Sox", "Atlanta Braves", "Miami Marlins" };

                //For 3B as described in Part 2C
                //Prepare some string arrays for building objects
                string[] leagues = new string[] { "American League Central", "National League Central", "American League East", "National League East", "American League West", "National League West" };
                //League
                if (!context.Leagues.Any())
                {
                    //loop through the array of League names
                    foreach (string g in leagues)
                    {
                        League league = new()
                        {
                            Name = g
                        };
                        context.Leagues.Add(league);
                    }
                    context.SaveChanges();
                }

                //Team
                if (!context.Teams.Any())
                {
                    //loop through the array of Team names
                    foreach (string iname in teams)
                    {
                        Team inst = new Team()
                        {
                            Name = iname,
                            Budget = Double.Parse(random.Next(501, 10000).ToString()),
                            LeagueID = random.Next(1, 6)
                        };
                        context.Teams.Add(inst);
                    }
                    context.SaveChanges();
                }
                //Create a collection of the primary keys of the Teams
                int[] teamIDs = context.Teams.Select(a => a.ID).ToArray();
                int teamIDCount = teamIDs.Length;

                //Footballian
                if (!context.Players.Any())
                {
                    // Start birthdate for randomly produced employees 
                    // We will subtract a random number of days from today
                    DateTime startDOB = DateTime.Today;

                    //Double loop through the arrays of names 
                    //and build the Footballian as we go
                    foreach (string f in firstNames)
                    {
                        foreach (string l in lastsNames)
                        {
                            Player m = new Player()
                            {
                                FirstName = f,
                                LastName = l,
                                EMail = f + l + "@gmail.com",
                                Jersey = random.Next(10, 99).ToString(),
                                FeePaid = Double.Parse(random.Next(501, 10000).ToString()),
                                DOB = startDOB.AddDays(-random.Next(6500, 25000))
                            };
                            context.Players.Add(m);
                        }
                    }
                    context.SaveChanges();
                }

                //Create a collection of the primary keys of the Players
                int[] playerIDs = context.Players.Select(p => p.ID).ToArray();
                int playerIDCount = playerIDs.Length;

                //Add a few teams to each Players
                if (!context.PlayerTeams.Any())
                {
                    int k = 0;
                    foreach (int i in playerIDs)
                    {
                        int howMany = random.Next(1, teamIDCount);//add a few teams to a Footballian
                        for (int j = 1; j <= howMany; j++)
                        {
                            k = (k >= teamIDCount) ? 0 : k;//Resets counter k to 0 if we have run out of teams
                            PlayerTeam p = new PlayerTeam()
                            {
                                PlayerID = i,
                                TeamID = teamIDs[k]
                            };
                            context.PlayerTeams.Add(p);
                            k++;
                        }
                    }
                    context.SaveChanges();
                }             
                                   

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.GetBaseException().Message);
            }
        }
    }
}
