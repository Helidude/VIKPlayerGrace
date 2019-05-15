using System;
using System.Collections.ObjectModel;
using Torch;

namespace VIKPlayerGrace
{
    public class GraceConfig : ViewModel
    {
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
    }  
}
