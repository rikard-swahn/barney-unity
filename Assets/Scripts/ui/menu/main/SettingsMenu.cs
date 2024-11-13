using Mgl;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace net.gubbi.goofy.ui.menu.main {
    public class SettingsMenu : Menu {

        public Menu soundMenu;
        public Menu speedMenu;
        public Menu languageMenu;
        public Menu rootMenu;
        public MessageMenu messageMenu;

        public void goToSoundMenu() {   
            hideInstantly();
            soundMenu.show();
            playClickSfx();
        }  
        
        public void goToSpeedMenu() {   
            hideInstantly();
            speedMenu.show();
            playClickSfx();
        }  
                
        public void goToLanguageMenu() {   
            hideInstantly();
            languageMenu.show();
            playClickSfx();
        }  
        
        public void goToPrivacySettings() {   
            DataPrivacy.FetchPrivacyUrl(onPrivacyUrlReceived, onPrivacyUrlFailure);
            playClickSfx();
        }

        public void back() {            
            hide();
            rootMenu.showInstantly();
            playClickSfx();
        }

        private void onPrivacyUrlFailure(string reason) {
            string detailText = I18n.Instance.__("PrivacyUrlFailText");
            
            if (Scenes.START.Equals(SceneManager.GetActiveScene().name)) {
                messageMenu.show(detailText);        
            }
        }
        private void onPrivacyUrlReceived(string url) {
            Application.OpenURL(url);
        }
    }
}