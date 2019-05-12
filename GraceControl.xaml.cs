﻿using Sandbox.Game.World;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Torch.API.Session;
using VRage.Game;


// TODO Add null checks, try/catch and logs

namespace VIKPlayerGrace
{
    public partial class GraceControl : UserControl
    {
        public static GracePlugin Plugin { get; private set; }

        private PlayerData selectedDatagridRow = null; 

        public GraceControl()
        {
            InitializeComponent();    
        }

        public GraceControl(GracePlugin plugin) : this()
        {
            Plugin = plugin;
            DataContext = plugin.Config;

            PlayersList.PlayerList = new List<PlayerData>();
        }

        private void ComboBoxPlayers_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Gets all players on server and loads them into ComboBox
            var comboboxPlayers = sender as ComboBox;
            comboboxPlayers.ItemsSource = GetAllPlayers().Select(x => x.DisplayName);
        }

        private void SaveConfig_OnClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(TextBoxGrace.Text, out var gracePeriod))
                return;

            var selectedPlayer = ComboBoxPlayers.SelectedValue.ToString();
            var playerId = GetPlayerIdByName(selectedPlayer);
            if (IsDupe(playerId) || gracePeriod < 1)
                return;

            Plugin.Config.PlayersOnLeave.Clear(); // Clear the config file for Players On Leave
            PlayersList.PlayerList.Add(new PlayerData // Add the new player
            {
                PlayerId = playerId,
                PlayerName = selectedPlayer,
                GraceGrantedAt = DateTime.Now,
                GracePeriodTo = GraceCalc(gracePeriod)
            });

            foreach (var playerData in PlayersList.PlayerList) // Add all the players to config file
            {
                Plugin.Config.PlayersOnLeave.Add(playerData);
            }
            
            Plugin.Save(); // Save Config file
            SessionPatches.Refresh(); // Apply the changes to session manager
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            if (selectedDatagridRow == null)
                return;

            var itemToRemove = PlayersList.PlayerList.SingleOrDefault(r => r.PlayerId == selectedDatagridRow.PlayerId);
            if (itemToRemove != null)
                PlayersList.PlayerList.Remove(itemToRemove);

            Plugin.Config.PlayersOnLeave.Clear(); // Clear the config file for Players On Leave
            foreach (var playerData in PlayersList.PlayerList) // Add all the players back to config file
            {
                Plugin.Config.PlayersOnLeave.Add(playerData);
            }

            Plugin.Save(); // Save Config file
            ButtonDelete.IsEnabled = false;
            SessionPatches.Refresh();
            SessionPatches.Remove(selectedDatagridRow.PlayerId);
        }

        private void ComboBoxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            var playerName = cb.SelectedValue.ToString();

            var playerId = GetPlayerIdByName(playerName);
            if (IsDupe(playerId))
                ButtonSave.IsEnabled = false;
            else
                ButtonSave.IsEnabled = true;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            PlayerData pd = dg.SelectedItem as PlayerData;

            if (pd != null)
            {
                selectedDatagridRow = pd;
            }
            ButtonDelete.IsEnabled = true;
        }

       
        private bool IsDupe(Int64 pid)
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

        public static List<MyIdentity> GetAllPlayers()
        {
            // Populate the list when plugin loads
            var idents = MySession.Static.Players.GetAllIdentities().ToList();
            var npcs = MySession.Static.Players.GetNPCIdentities().ToList();

            return idents.Where(i => !npcs.Any(n => n == i.IdentityId)).ToList();
        }

        public static long GetPlayerIdByName(string name)
        {
            if (!long.TryParse(name, out long id))
            {
                foreach (var identity in MySession.Static.Players.GetAllIdentities())
                {
                    if (identity.DisplayName == name)
                    {
                        return identity.IdentityId;
                    }
                }
            }

            return 0;
        }

        public DateTime GraceCalc(int days)
        {
            return DateTime.Now + TimeSpan.FromDays(days);
        }   
    }
}