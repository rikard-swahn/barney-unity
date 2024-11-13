using System.Collections.Generic;
using net.gubbi.goofy.game;
using net.gubbi.goofy.iap;
using net.gubbi.goofy.util;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif
using UnityEngine;
using Constants = net.gubbi.goofy.util.Constants;

namespace net.gubbi.goofy.ui.menu.main {
    public class StartScreen : MenuScreen {

        public int desktopTargetFrameRate;
        
#if !DISABLESTEAMWORKS
        protected Callback<GameOverlayActivated_t> gameOverlayActivated;
#endif        

        protected override void Awake() {
            base.Awake();
            
            if (PlatformUtil.isDesktopPlayer() || Application.isEditor) {
                QualitySettings.vSyncCount = 1;
                Application.targetFrameRate = desktopTargetFrameRate;
                Screen.fullScreen = true;
            }            
            
            DonationFacade.Instance.init();
        }
        
        private void OnEnable() {
#if !DISABLESTEAMWORKS
            if (SteamManager.Initialized) {
                gameOverlayActivated = Callback<GameOverlayActivated_t>.Create(onSteamOverlayActivated);
            }
#endif
        }      
        
#if !DISABLESTEAMWORKS  
        private void onSteamOverlayActivated(GameOverlayActivated_t param) {
            if(param.m_bActive != 0) {
                Pause.Instance.pause(Constants.SOURCE_ID_STEAM);
            }
            else {
                Pause.Instance.unPause(Constants.SOURCE_ID_STEAM);    
            }
        }
#endif
        
    }
}