using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace VIKPlayerGrace
{
    public partial class GraceControl
    {
        public static GracePlugin Plugin { get; set; }
        public static readonly Logger Log = LogManager.GetLogger("GraceControl");

        private PlayerData selectedDatagridRow = null;

        public GraceControl()
        {
            InitializeComponent();
            PlayersList.PlayerList = new List<PlayerData>();
        }

        public GraceControl(GracePlugin plugin) : this()
        {
            Plugin = plugin;
            DataContext = plugin.Config;
        }

        private void ComboBoxPlayers_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Get all players on server and loads them into ComboBox
            var comboboxPlayers = sender as ComboBox;
            comboboxPlayers.ItemsSource = GetFromSession.GetAllPlayers().Select(x => x.DisplayName);
        }

        private void SavePlayer_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedPlayer = ComboBoxPlayers.SelectedValue.ToString();
            var playerId = GetFromSession.GetPlayerIdByName(selectedPlayer);
            if (IsDupe(playerId))
                return;

            PlayersList.PlayerList.Add(new PlayerData
            {
                PlayerId = playerId,
                PlayerName = selectedPlayer,
                GraceGrantedAt = DateTime.Now
            });

            ConfWriter(PlayersList.PlayerList);
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (selectedDatagridRow == null)
                return;

            var itemToRemove = PlayersList.PlayerList.SingleOrDefault(r => r.PlayerId == selectedDatagridRow.PlayerId);
            if (itemToRemove != null)
                PlayersList.PlayerList.Remove(itemToRemove);

            ButtonDelete.IsEnabled = false;

            ConfWriter(PlayersList.PlayerList);
        }

        private void ComboBoxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            var playerName = cb.SelectedValue.ToString();

            var playerId = GetFromSession.GetPlayerIdByName(playerName);
            if (IsDupe(playerId))
                ButtonSave.IsEnabled = false;
            else
                ButtonSave.IsEnabled = true;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            var pd = dg.SelectedItem as PlayerData;

            if (pd != null)
            {
                selectedDatagridRow = pd;
            }
            ButtonDelete.IsEnabled = true;
        }

        /// <summary>
        /// Check if player is already added
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private bool IsDupe(long pid)
        {
            if (PlayersList.PlayerList == null)
                return false;

            foreach (var playerData in PlayersList.PlayerList)
            {
                if (playerData.PlayerId == pid)
                    return true;
            }

            return false;
        }

        public static void ConfWriter(List<PlayerData> pdList)
        {
            // Clear the config file for Players
            Plugin.Config.PlayersOnLeave.Clear();

            // Add all the players back from the temporary list
            foreach (var playerData in pdList)
            {
                Plugin.Config.PlayersOnLeave.Add(playerData);
            }

            // Save Config file
            Plugin.Save();
            SessionPatches.ApplySession();
            Log.Info("Config and Session updated!");
        }
    }
}
