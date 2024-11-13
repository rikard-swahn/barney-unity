using System;
using net.gubbi.goofy.state;
using net.gubbi.goofy.ui.menu.ingame;
using net.gubbi.goofy.util;
using UnityEditor;
using UnityEngine;

namespace net.gubbi.goofy.ui.menu.main {
    public class RootMenu : Menu {

        public Menu settingsMenu;
        public Menu gamesMenu;
        public Menu helpMenu;

        public RectTransform startButton;
        public RectTransform settingsButton;
        public RectTransform helpButton;
        public RectTransform quitButton;

        private float startX;
        private float soundX;
        private float quitY;
        
        protected override void Awake() {
            var hasQuitButton = quitButton.gameObject.activeInHierarchy;
            base.Awake();
            
            var persistFacade = GameObject.FindGameObjectWithTag(Tags.ROOM).GetComponent<PersistFacade>();
            persistFacade.clearCurrentGame();

            startX = startButton.anchoredPosition.x;
            soundX = settingsButton.anchoredPosition.x;
            quitY = quitButton.anchoredPosition.y;
            
            if (!hasQuitButton) {
                Vector2 shift = new Vector2(0f, -25);
                startButton.anchoredPosition += shift;
                settingsButton.anchoredPosition += shift;
                helpButton.anchoredPosition += shift;
            }
        }

        public override void show() {
            base.show();
            playTweenSfx();
            Action onComplete = delegate {setInteractable(true);};                        
            translateToX(0, 0.5f, onComplete, startButton, settingsButton, helpButton);            
            translateToY(-75, 0.5f, null, quitButton);            
        }

        public override void hide(Action onComplete = null) {
            setInteractable(false);
            
            Action onCompleteWrapper = () => {
                base.hide();
                onComplete?.Invoke();
            };

            translateToX(startX, 0.5f, onCompleteWrapper, startButton);            
            translateToX(startX, 0.5f, null, helpButton);            
            translateX(soundX, 0.5f, null, settingsButton);            
            translateToY(quitY, 0.5f, null, quitButton);     
        }

        public void goToGamesMenu() {            
            hide(gamesMenu.show);            
            playClickSfx();
        }     
        
        public void goToSettingsMenu() {   
            hideInstantly();
            settingsMenu.show();
            playClickSfx();
        }              
                
        public void goToHelpMenu() {   
            hideInstantly();
            helpMenu.show();
            playClickSfx();
        }        
        
        public void exit() {
            playClickSfx();
            Application.Quit();

#if UNITY_EDITOR            
            EditorApplication.isPlaying = false;
#endif
        }        
        
    }
}