using System;
using net.gubbi.goofy.events;
using net.gubbi.goofy.scene.load;
using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEditor;
using UnityEngine;

namespace net.gubbi.goofy.ui.menu.ingame {
    public class InGameMenuLightbox : InGameLightbox {

        public InGameLightbox settingsBox;
        public InGameLightbox helpBox;
        public Transitions.Type mainMenuOutTransition = Transitions.Type.CIRCLE_OUT;
        public Transitions.Type mainMenuInTransition = Transitions.Type.CIRCLE_IN;

        private PersistFacade _persistFacade;

        protected override void Awake() {
            base.Awake();
            _persistFacade = GameObject.FindGameObjectWithTag(Tags.ROOM).GetComponent<PersistFacade>();
            EventManager.Instance.addListener<GameEvents.LateKeyUpEvent>(lateKeyUpHandler);
        }

        public override void show() {
            base.show();
            playClickSfx();           
        }

        public void goToMainMenu() {                        
            close();
            Action onOutComplete = delegate {
                _persistFacade.HandleAutoSave();                
            };
            
            SceneLoader.Instance.loadScene(Scenes.START, Transitions.get(mainMenuOutTransition), Transitions.get(mainMenuInTransition), onOutComplete);
            playClickSfx();
        }

        public void goToSettingsMenu() {
            close();
            settingsBox.show();
            playClickSfx();
        }
        
        public void goToHelpMenu() {
            close();
            helpBox.show();
            playClickSfx();
        }

        public void quit() {
            playClickSfx();
            Application.Quit();
#if UNITY_EDITOR            
            EditorApplication.isPlaying = false;
#endif
        }
        public override void back() {
            close();
            playClickSfx();
        } 
        
        private void lateKeyUpHandler(GameEvents.LateKeyUpEvent e) {
            if (e.KeyCode == KeyCode.Escape) {
                if (!gameObject.activeInHierarchy) {
                    show();
                }
            }            
        }        
    }

}