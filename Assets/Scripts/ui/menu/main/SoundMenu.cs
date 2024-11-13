using Mgl;
using net.gubbi.goofy.events;
using net.gubbi.goofy.state.settings;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.main {
    public class SoundMenu : Menu {

        public Menu settingsMenu;
        public Text sfxText;
        public Text musicText;

        public override void show() {
            base.show();
            updateSfxText();
            updateMusicText();   
        }

        public void toggleSfx() {
            bool sfxEnabled = !SettingsState.Instance.StateData.SfxEnabled;
            SettingsState.Instance.setSfxEnabled(sfxEnabled) ;
            updateSfxText();
            playClickSfx();
        }

        public void toggleMusic() {            
            bool musicEnabled = !SettingsState.Instance.StateData.MusicEnabled;
            SettingsState.Instance.setMusicEnabled(musicEnabled);
            updateMusicText();

            if (musicEnabled) {
                EventManager.Instance.raise(new GameEvents.MusicEnabledEvent());
            }
            else {
                EventManager.Instance.raise(new GameEvents.MusicDisabledEvent());
            }             
            playClickSfx();
        }        

        public void back() {            
            hide();
            settingsMenu.showInstantly();
            playClickSfx();
        }        

        private void updateSfxText() {
            sfxText.text = I18n.Instance.__("sfxEnabled-" + SettingsState.Instance.StateData.SfxEnabled);
        }
        
        private void updateMusicText() {
            musicText.text = I18n.Instance.__("musicEnabled-" + SettingsState.Instance.StateData.MusicEnabled);
        }
        
    }
}