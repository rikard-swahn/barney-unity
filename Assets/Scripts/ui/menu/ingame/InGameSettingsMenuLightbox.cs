namespace net.gubbi.goofy.ui.menu.ingame {
    public class InGameSettingsMenuLightbox : InGameLightbox {
        
        public InGameLightbox menuBox;
        public InGameLightbox soundBox;
        public InGameLightbox speedBox;
        
        public void goToSoundMenu() {
            close();
            soundBox.show();
            playClickSfx();
        }

        public void goToSpeedMenu() {
            close();
            speedBox.show();
            playClickSfx();
        }            

        public override void back() {
            close();
            menuBox.show();
        }
    }
}