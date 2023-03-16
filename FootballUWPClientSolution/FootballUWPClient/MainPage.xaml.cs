using FootballUWPClient.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FootballUWPClient.Models;
using FootballUWPClient.Data;
using System.Numerics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409
//By Banuka Kumara Ambegoda
//PROG 1442 Project 1

namespace FootballUWPClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly ILeagueRepository leagueRepository;
        private readonly ITeamRepository teamRepository;

        public MainPage()
        {
            InitializeComponent();
            leagueRepository = new LeagueRepository();
            teamRepository = new TeamRepository();
            FillDropDown();
        }

        private async void FillDropDown()
        {
            //Show Progress
            progRing.IsActive = true;
            progRing.Visibility = Visibility.Visible;
            btnAdd.IsEnabled = true;

            try
            {
                List<League> leagues = await leagueRepository.GetLeagues();
                //Add the All Option
                leagues.Insert(0, new League { ID = 0, Name = " - All Leagues" });
                //Bind to the ComboBox
                LeagueCombo.ItemsSource = leagues;
                ShowTeams(null);
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    Jeeves.ShowMessage("Error", "No connection with the server.");
                }
                else
                {
                    Jeeves.ShowMessage("Error", "Could not complete operation");
                }
            }
            finally
            {
                progRing.IsActive = false;
                progRing.Visibility = Visibility.Collapsed;
            }
        }

        private async void ShowTeams(int? LeagueID)
        {
            //Show Progress
            progRing.IsActive = true;
            progRing.Visibility = Visibility.Visible;

            try
            {
                List<Team> teams;

                if (LeagueID.GetValueOrDefault() > 0)
                {
                    teams = await teamRepository.GetTeamsByLeague(LeagueID.GetValueOrDefault());
                }
                else
                {
                    teams = await teamRepository.GetTeams();
                }

                teamList.ItemsSource = teams;

            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    Jeeves.ShowMessage("Error", "No connection with the server.");
                }
                else
                {
                    Jeeves.ShowMessage("Error", "Could not complete operation");
                }
            }
            finally
            {
                progRing.IsActive = false;
                progRing.Visibility = Visibility.Collapsed;
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            FillDropDown();
        }
        
        private void LeagueCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            League selDoc = (League)LeagueCombo.SelectedItem;
            ShowTeams(selDoc?.ID);
        }

        private void teamGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the detail page
            Frame.Navigate(typeof(TeamDetailPage), (Team)e.ClickedItem);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Team newTm = new Team();

            // Navigate to the detail page
            Frame.Navigate(typeof(TeamDetailPage), newTm);
        }
    }
}
