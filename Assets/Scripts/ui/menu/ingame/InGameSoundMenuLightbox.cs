using Mgl;
using net.gubbi.goofy.events;
using net.gubbi.goofy.state.settings;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.ingame {
    public class InGameSoundMenuLightbox : InGameLightbox {
        
        public InGameLightbox settingsBox;
        public Text sfxText;
        public Text musicText;
        
        private void Start() {
            updateSfxText();
            updateMusicText();
        }

        public void toggleSfx() {
            bool sfxEnabled = !SettingsState.Instance.StateData.SfxEnabled;
            SettingsState.Instance.setSfxEnabled(sfxEnabled) ;
            updateSfxText();
            
            if (sfxEnabled) {
                EventManager.Instance.raise(new GameEvents.SfxEnabledEvent());
            }
            else {
                EventManager.Instance.raise(new GameEvents.SfxDisabledEvent());
            }                        
            
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

        public override void back() {
            close();
            settingsBox.show();
        }

        private void updateSfxText() {
            sfxText.text = I18n.Instance.__("sfxEnabled-" + SettingsState.Instance.StateData.SfxEnabled);
        }
        
        private void updateMusicText() {
            musicText.text = I18n.Instance.__("musicEnabled-" + SettingsState.Instance.StateData.MusicEnabled);
        }
        
    }
}