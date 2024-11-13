using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.ui.menu.ingame {
    public class InGameHelpMenuLightbox : InGameLightbox {
        
        public InGameLightbox menuBox;
        public InGameLightbox howToPlayBox;

        public void goToHowToPlay() {   
            close();
            howToPlayBox.show();
            playClickSfx();
        }

        public void goToWalkthrough() {
            Application.OpenURL(Constants.getWalkthroughUrl());
            playClickSfx();
        }
        
        public override void back() {
            close();
            menuBox.show();
        }

    }
}