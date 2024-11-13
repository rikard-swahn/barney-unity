using net.gubbi.goofy.audio;
using net.gubbi.goofy.events;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.game;
using net.gubbi.goofy.iap;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.player;
using net.gubbi.goofy.state;
using net.gubbi.goofy.ui.cursor;
using net.gubbi.goofy.util;
#if !DISABLESTEAMWORKS
using Steamworks;
#endif
using UnityEngine;
using Constants = net.gubbi.goofy.util.Constants;

namespace net.gubbi.goofy.scene {
    public class Room : MonoBehaviour {

        public AudioClip music;
        public AudioClip sfxLooping;
                           
        public float scaleReferenceY = 0f;
        public float scaleAtReferenceY = 1f;
        
        //This value sets the difference in scale between top and bottom of screen        
        public float yScaling = 0.25f;           
        
        public Filter musicCondition;

#if !DISABLESTEAMWORKS
        protected Callback<GameOverlayActivated_t> gameOverlayActivated;
#endif        

        private SceneInputHandler playerInput;
        private bool controlDisabledOverride;
        private SfxPlayer sfxPlayer;
        private MusicPlayer musicPlayer;

        protected virtual void Awake() {
            EventManager.Instance.addListener<GameEvents.MouseEvent>(handleMouseEvent);
            EventManager.Instance.addListener<GameEvents.SceneTransitionStartedEvent>(sceneTransitionStartedHandler);
            EventManager.Instance.addListener<GameEvents.SceneTransitionCompletedEvent>(sceneTransitionCompletedHandler);            
            EventManager.Instance.addListener<GameEvents.MusicFinishedEvent>(musicFinishedHandler);            
            InteractionState.Instance.clear();
            
            sfxPlayer = GameObject.Find (GameObjects.SFX_PLAYER).GetComponent<SfxPlayer>();
        }
        
        private void OnEnable() {
#if !DISABLESTEAMWORKS
            if (SteamManager.Initialized) {
                gameOverlayActivated = Callback<GameOverlayActivated_t>.Create(onSteamOverlayActivated);
            }
#endif
        }        

        private void Start() {
            GameObject invGo = GameObject.Find(GameObjects.INVENTORY);
            if (invGo != null) {
                Inventory inventory = invGo.GetComponent<Inventory>();

                inventory.addItem(ItemType.TO_DO_LIST);
                
                if (!inventory.hasItem(ItemType.BANK_CARD)) {
                    inventory.addItem(new CardItem(0));
                }
            }

            //GameState.Instance.StateData.setFlags("IntroAfterSleepComplete");
            //GameState.Instance.StateData.setFlags("grandmaInvited");
            GameState.Instance.StateData.Initialized = true;
            
            
            musicPlayer = GameObject.Find(GameObjects.MUSIC_PLAYER).GetComponent<MusicPlayer>();
            playMusic();
            sfxPlayer.playLooping(sfxLooping);
        }

        private void Update() {
            EventManager.Instance.raise(new GameEvents.GameTimeEvent(Time.deltaTime));
        }

        private void musicFinishedHandler(GameEvents.MusicFinishedEvent e) {
            playMusic();
        }

        private void sceneTransitionStartedHandler(GameEvents.SceneTransitionStartedEvent e) {
            controlDisabledOverride = true;
        }

        private void sceneTransitionCompletedHandler(GameEvents.SceneTransitionCompletedEvent e) {
            controlDisabledOverride = false;         
        }        
        
        private void handleMouseEvent(GameEvents.MouseEvent e) {                    
            if (InteractionState.Instance.isMouseOverMenu() && !InteractionState.Instance.hasState(InteractionState.StateType.PAUSE_MENU) && !controlDisabledOverride) {
                EventManager.Instance.raise(new GameEvents.CursorEvent(CursorType.NORMAL));
            }
        }

        private void playMusic() {
            if (musicCondition == null || musicCondition.matches()) {
                musicPlayer.playLooping(music);
            }
            else {
                musicPlayer.playLooping(null);
            }
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