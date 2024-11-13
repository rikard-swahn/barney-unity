using net.gubbi.goofy.ui.menu.ingame;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.ui.menu.main {
    public class HelpMenu : Menu {

        public Menu rootMenu;
        public HowToPlayLightbox howToPlayBox;

        public void goToHowToPlay() {   
            hideInstantly();
            howToPlayBox.show();
            playClickSfx();
        }

        public void goToWalkthrough() {
            Application.OpenURL(Constants.getWalkthroughUrl());
            playClickSfx();
        }
        
        public void back() {            
            hide();
            rootMenu.showInstantly();
            playClickSfx();
        }

    }
}