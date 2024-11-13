namespace net.gubbi.goofy.state.settings {
    
    public class SettingsStateDataDto {

        public bool musicEnabled;
        public bool sfxEnabled;
        public int gameSpeed;
        public bool hasDonated;
        public GamesDto games;

        public SettingsStateDataDto() { }

        public SettingsStateDataDto(SettingsStateData stateData) {
            musicEnabled = stateData.MusicEnabled;
            sfxEnabled = stateData.SfxEnabled;
            gameSpeed = stateData.GameSpeed;
            hasDonated = stateData.HasDonated;
            games = new GamesDto(stateData.Games);
        }
        
    }
}