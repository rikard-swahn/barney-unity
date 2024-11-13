using UnityEngine;

namespace net.gubbi.goofy.state.settings {
    public class SettingsStateData {
        
        private static readonly bool MUSIC_ENABLED_DEFAULT = true;
        private static readonly bool SFX_ENABLED_DEFAULT = true;
        private static readonly int GAME_SPEED_DEFAULT = 0;

        public bool MusicEnabled {get; set;}        
        public bool SfxEnabled {get; set;}
        
        public int GameSpeed {get; set;}
        public bool HasDonated {get; set;}
        public Games Games {get; private set;}

        public SettingsStateData() {
            MusicEnabled = MUSIC_ENABLED_DEFAULT;
            SfxEnabled = SFX_ENABLED_DEFAULT;
            GameSpeed = GAME_SPEED_DEFAULT;
            Games = new Games();
        }

        public SettingsStateData(SettingsStateDataDto stateDataDto) {
            MusicEnabled = stateDataDto.musicEnabled;
            SfxEnabled = stateDataDto.sfxEnabled;
            HasDonated = stateDataDto.hasDonated;
            GameSpeed = stateDataDto.gameSpeed;
            Games = new Games(stateDataDto.games);
        }
    }
}