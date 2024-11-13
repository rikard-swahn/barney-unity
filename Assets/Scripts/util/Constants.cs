namespace net.gubbi.goofy.util {
    public class Constants {
        public static readonly float DEFAULT_ACTION_TIME = 0.5f;
        public static readonly int DEMO_SECONDS = 600;
        public static readonly uint STEAM_APP_ID = 1052210;
        
        public static readonly string SOURCE_ID_GAME = "GAME";
        public static readonly string SOURCE_ID_STEAM = "STEAM";
        private const string WALKTHROUGH_URL = "https://rikard-swahn.github.io/barney/Barneys-Dream-Cruise-Walkthrough{locale}.pdf";

        public static string getWalkthroughUrl() {
            var locale = I18nManager.GetLanguage();
            var url = "en".Equals(locale) ? WALKTHROUGH_URL.Replace("{locale}", "") : WALKTHROUGH_URL.Replace("{locale}", "-" + locale.ToUpper());
            return url;
        }
        
    }
}