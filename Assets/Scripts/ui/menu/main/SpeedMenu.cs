using Mgl;
using net.gubbi.goofy.state.settings;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui.menu.main {
    public class SpeedMenu : Menu {

        public Menu settingsMenu;
        public Text speedText;
        
        public override void show() {
            base.show();
            updateSpeedText();            
        }

        public void toggleSpeed() {
            int newSpeed = (SettingsState.Instance.StateData.GameSpeed + 1) % 3;
            SettingsState.Instance.setGameSpeed(newSpeed) ;
            updateSpeedText();
            playClickSfx();
        }

        public void back() {            
            hide();
            settingsMenu.showInstantly();
            playClickSfx();
        }        

        private void updateSpeedText() {
            speedText.text = I18n.Instance.__("gameSpeed-" + SettingsState.Instance.StateData.GameSpeed);
        }

    }
}