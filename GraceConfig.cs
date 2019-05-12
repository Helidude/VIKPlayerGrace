using Sandbox.Game.World;
using System;
using System.Collections.ObjectModel;
using Torch;

namespace VIKPlayerGrace
{
    public class GraceConfig : ViewModel
    {
        // Plugin enabled or not
        private bool _playerGraceEnabled = true;
        public bool PlayerGraceEnabled { get => _playerGraceEnabled; set => SetValue(ref _playerGraceEnabled, value); }

        // Store players on leave
        public ObservableCollection<PlayerData> PlayersOnLeave { get; } = new ObservableCollection<PlayerData>();

    }

    public class PlayerData : ViewModel
    {

        private long _playerId;
        public long PlayerId { get => _playerId; set => SetValue(ref _playerId, value); }

        private string _playerName;
        public string PlayerName { get => _playerName; set => SetValue(ref _playerName, value); }

        private DateTime _playerGraceGrantedAt;
        public DateTime GraceGrantedAt { get => _playerGraceGrantedAt; set => SetValue(ref _playerGraceGrantedAt, value); }

        private DateTime _playerGracePeriodTo;
        public DateTime GracePeriodTo { get => _playerGracePeriodTo; set => SetValue(ref _playerGracePeriodTo, value); }

    }  
}
