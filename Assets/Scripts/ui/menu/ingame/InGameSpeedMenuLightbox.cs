using Mgl;
using net.gubbi.goofy.state.settings;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.ingame {
    public class InGameSpeedMenuLightbox : InGameLightbox {
        
        public InGameLightbox settingsBox;
        public Text speedText;
        
        private void Start() {
            updateSpeedText();
        }

        public void toggleSpeed() {
            int newSpeed = (SettingsState.Instance.StateData.GameSpeed + 1) % 3;
            SettingsState.Instance.setGameSpeed(newSpeed) ;
            updateSpeedText();
            playClickSfx();
        }
        
        public override void back() {
            close();
            settingsBox.show();
        }

        private void updateSpeedText() {
            speedText.text = I18n.Instance.__("gameSpeed-" + SettingsState.Instance.StateData.GameSpeed);
        }
    }
}