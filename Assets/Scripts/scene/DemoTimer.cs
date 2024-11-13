using System;
using Mgl;
using net.gubbi.goofy.events;
using net.gubbi.goofy.scene.load;
using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.state;
using net.gubbi.goofy.state.settings;
using net.gubbi.goofy.ui.menu.main;
using net.gubbi.goofy.util;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif
using UnityEngine;
using UnityEngine.UI;
using Constants = net.gubbi.goofy.util.Constants;

namespace net.gubbi.goofy.scene {
    public class DemoTimer : MonoBehaviour {

        public Transitions.Type mainMenuOutTransition = Transitions.Type.CIRCLE_OUT;
        public Transitions.Type mainMenuInTransition = Transitions.Type.CIRCLE_IN;

        
        private ConfirmLightbox confirmBox; 
        private MessageLightbox messageBox; 
        private Text text;
        private bool timeUpShown;
        private PersistFacade _persistFacade;
        private bool disabled;

        private void Awake() {
#if !DEMO
            gameObject.SetActive(false);
            return;            
#endif
            
            _persistFacade = GameObject.FindGameObjectWithTag(Tags.ROOM).GetComponent<PersistFacade>();
            text = GetComponent<Text>();
            confirmBox = GameObject.Find(GameObjects.CONFIRM_BOX).GetComponent<ConfirmLightbox>();
            messageBox = GameObject.Find(GameObjects.MESSAGE_BOX).GetComponent<MessageLightbox>();
            
            EventManager.Instance.addListener<GameEvents.SceneTransitionStartedEvent>(delegate { disabled = true;});
            EventManager.Instance.addListener<GameEvents.SceneTransitionCompletedEvent>(delegate { disabled = false;});
        }

        private void Update() {
            if (!SettingsState.Instance.getCurrentGameSlot().AutoSaveEnabled) {
                text.text = "";
                return;
            }
            
            float timeLeft = Constants.DEMO_SECONDS - GameState.Instance.StateData.PartStartedGameTimeSeconds;
            timeLeft = Mathf.Max(0, timeLeft);
            
            int minutes = Mathf.FloorToInt(timeLeft / 60f);
            int seconds = Mathf.FloorToInt(timeLeft - minutes * 60);
            
            text.text = I18n.Instance.__("demoTimer", minutes.ToString("00"), seconds.ToString("00"));

            if (timeLeft == 0f && !timeUpShown && !disabled) {
                showTimeUp();
            }
        }
        

        private void showTimeUp() {
            timeUpShown = true;
            _persistFacade.HandleAutoSave();
            confirmBox.show(I18n.Instance.__("demoTimeUp"), showFullGameInSteam, goToMainMenu);
        }

        public void goToMainMenu() {
            SceneLoader.Instance.loadScene(Scenes.START, Transitions.get(mainMenuOutTransition), Transitions.get(mainMenuInTransition));
        }

        private void showFullGameInSteam() {
#if !DISABLESTEAMWORKS

            if (SteamManager.Initialized) {
                if (SteamUtils.IsOverlayEnabled())  {
                    SteamFriends.ActivateGameOverlayToStore((AppId_t) Constants.STEAM_APP_ID, EOverlayToStoreFlag.k_EOverlayToStoreFlag_AddToCartAndShow);
                    goToMainMenu();
                }
                else {
                    messageBox.show(I18n.Instance.__("steamOverlayDisabled"), goToMainMenu);
                }
            }
            else {
                messageBox.show(I18n.Instance.__("steamNotInitialized"), goToMainMenu);
            }
#endif
            
        }
        
    }
}