using System;
using net.gubbi.goofy.audio;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.player;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public abstract class CharacterBehaviour : MonoBehaviour {

        public string requiresStateFlag;
        public bool controlEnabled = true;
        public Filter skipCondition;
        public bool skipOnResume;
        public bool delayStop;
        public DelayedAudioClip sfxBefore;
        public DelayedAudioClip sfxAfter;
        public MusicClip musicBefore;
        public bool stopSfx;
        public bool stopMusic;

        protected CharacterFacade characterFacade;
        protected CharacterFacade playerFacade;
        protected CharacterMove characterMove;
        protected PlayerMove playerMove;
        protected CharacterSceneHandler characterSceneHandler;
        protected string characterKey;
        protected GameObject characterGo;
        protected bool started;
        
        private SceneInputHandler sceneInput;        
        private SfxPlayer sfxPlayer;
        private MusicPlayer musicPlayer;
        private Action onCompleteWrapper;
        
        private static readonly string CONTROL_BY_ID = "behaviour";

        protected virtual void Awake() {
            characterGo = gameObject.getAncestorWithTag (Tags.CHARACTER);
            characterKey = characterGo.name;
            characterFacade = GetComponentInParent<CharacterFacade> ();
            characterMove = GetComponentInParent<CharacterMove> ();
            characterSceneHandler = GetComponentInParent<CharacterSceneHandler> ();

            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerFacade = playerGo.GetComponent<CharacterFacade> ();
            sceneInput = playerGo.GetComponent<SceneInputHandler>();
            playerMove = playerGo.GetComponent<PlayerMove> ();
            
            sfxPlayer = GameObject.Find (GameObjects.SFX_PLAYER).GetComponent<SfxPlayer>();
            musicPlayer = GameObject.Find (GameObjects.MUSIC_PLAYER).GetComponent<MusicPlayer>();
        }

        public virtual void freezeUI () {
            setControlEnabled(true);
            started = false;
        }

        public void resumeBehaviourBackground() {
            beforeBehaviour();
        }

        public bool resume(Action onComplete) {
            if (skipOnResume) {
                setControlEnabled (true);
                onComplete();
                started = true;
            }
            else {
                started = start(onComplete);    
            }

            return started;
        }

        public bool start(Action onComplete) {
            onCompleteWrapper = delegate {
                started = false;
                setControlEnabled (true);
                onComplete();
            };            

            if (skipBehavior()) {
                onCompleteWrapper();
                started = true;
            }            
            else if (allowBehavior()) {
                setControlEnabled (controlEnabled);
                //Debug.Log("Do behav " + GetType().Name + " on obj " + transform.root.gameObject.name);
                doFullBehaviour (onCompleteWrapper);
                started = true;
            }
            else {
                started = false;
            }

            return started;
        }
        
        public virtual bool stop() {
            setControlEnabled(true);

            if (started && delayStop) {
                started = false;
                onCompleteWrapper();
                return false;
            }

            started = false;
            return true;
        }

        private void doFullBehaviour(Action onComplete) {        
            Action onCompleteWrapper = delegate {
                afterBehaviour();
                onComplete();                
            };
            
            beforeBehaviour();
            doBehaviour(onCompleteWrapper);
        }

        private void beforeBehaviour() {
            if (stopSfx) {
                sfxPlayer.stop();
            }
            
            sfxPlayer.play(sfxBefore);
                        
            if (stopMusic) {
                musicPlayer.fadeOutStop();
            }
            else {
                musicPlayer.playDelayed(musicBefore);                    
            }
        }

        protected abstract void doBehaviour (Action onComplete);
        protected virtual void afterBehaviour() {
            sfxPlayer.play(sfxAfter);            
        }
        
        public bool endOfBehaviour(bool endingCurrentBehavior) {
            if (skipBehavior()) {
                return true;
            }            
            
            if (allowBehavior ()) {
                return doEndOfBehaviour (endingCurrentBehavior);
            }

            return false;			
        }

        protected abstract bool doEndOfBehaviour (bool endingCurrentBehavior);     
        
        private void setControlEnabled(bool enabled) {
            sceneInput.setControlEnabled (enabled, CONTROL_BY_ID + "-" + characterKey);
        }

        private bool allowBehavior() {
            return requiresStateFlag.isNullOrEmpty() || GameState.Instance.StateData.flagsSet (requiresStateFlag);
        }
        
        private bool skipBehavior() {
            return skipCondition != null && skipCondition.matches();
        }
        
    }
}