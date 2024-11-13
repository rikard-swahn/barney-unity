using System.Collections;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.game;
using net.gubbi.goofy.state.settings;
using UnityEngine;

namespace net.gubbi.goofy.audio {
    public class SfxPlayer : MonoBehaviour {
     
        public AudioSource gameAudioSource;
        public AudioSource menuAudioSource;
        public float fadeOutTime = 0.5f;

        private AudioClip clip;
        private bool inSceneTransitionOut;
        private static readonly float MAX_VOLUME = 1f;
        
        private void Awake() {
            EventManager.Instance.addListener<GameEvents.SfxEnabledEvent>(sfxEnabledHandler);
            EventManager.Instance.addListener<GameEvents.SfxDisabledEvent>(sfxDisabledHandler);           
            EventManager.Instance.addListener<GameEvents.PauseGameEvent>(pauseGameHandler);
            EventManager.Instance.addListener<GameEvents.UnPauseGameEvent>(unPauseGameHandler);      
            EventManager.Instance.addListener<GameEvents.SceneChangeInitEvent>(handleSceneChangeStarted);
        }

        public void play(DelayedAudioClip clip) {
            if (clip.clip != null) {
                this.delayedAction(
                    () => {
                        if (clip.loop) {
                            playLooping(clip.clip);
                        }
                        else {
                            playOnce(clip.clip);
                        }
                    },
                    clip.delay
                );                
            }
        }

        //This method is kept to be compatible with existing calls from canvas
        public void play(AudioClip clip) {
            playOnce(clip);
        } 
        
        public void playLoopingIfSilence(AudioClip clip) {
            if (!gameAudioSource.isPlaying) {
                playLooping(clip);
            }
        }        
        
        public void playOnce(AudioClip clip) {
            if (clip != null && SettingsState.Instance.StateData.SfxEnabled) {       
                if (inSceneTransitionOut) {
                    return;
                }                   
                
                gameAudioSource.PlayOneShot(clip);
                if (Pause.Instance.isPaused()) {
                    gameAudioSource.Pause();
                }                
            }
        }    
        
        public void playOnceMenu(AudioClip clip) {
            if (clip != null && SettingsState.Instance.StateData.SfxEnabled) {                
                menuAudioSource.PlayOneShot(clip);
            }
        }

        public void playLooping(AudioClip clip) {
            if (clip != null) {
                if (inSceneTransitionOut) {
                    return;
                }

                this.clip = clip;
                
                if (SettingsState.Instance.StateData.SfxEnabled && (gameAudioSource.clip == null || !gameAudioSource.clip.name.Equals (clip.name) || !gameAudioSource.isPlaying)) {
                    gameAudioSource.clip = clip;
                    gameAudioSource.Play();
                    if (Pause.Instance.isPaused()) {
                        gameAudioSource.Pause();
                    }                    
                }
            }
        }

        public void stop() {
            clip = null;
            gameAudioSource.Stop();
            gameAudioSource.clip = null;
        }

        /// <summary>
        /// Stop if playing clip is given clip
        /// </summary>
        public void stop(AudioClip clip) {
            if (clip != null && gameAudioSource.clip != null && gameAudioSource.clip.name.Equals (clip.name)) {
                stop();
            }
        }

        private void sfxEnabledHandler(GameEvents.SfxEnabledEvent e) {
            playLooping(clip);
        }

        private void sfxDisabledHandler(GameEvents.SfxDisabledEvent e) {
            gameAudioSource.Stop();
            menuAudioSource.Stop();
        }

        private void pauseGameHandler(GameEvents.PauseGameEvent e) {
            if (gameAudioSource.isPlaying) {
                gameAudioSource.Pause();
            }
        }

        private void unPauseGameHandler(GameEvents.UnPauseGameEvent e) {        
            gameAudioSource.UnPause();
        }
        
        private void handleSceneChangeStarted(GameEvents.SceneChangeInitEvent e) {
            inSceneTransitionOut = true;

            if (e.OutTransition == null || e.OutTransition.fadeMusic) {
                StartCoroutine(fadeOut(fadeOutTime));    
            }
        }   
        
        private IEnumerator fadeOut(float fadeTime) {
            float startVolume = gameAudioSource.volume;
            fadeTime = fadeTime * (startVolume / MAX_VOLUME);

            while (gameAudioSource.volume > 0) {
                gameAudioSource.volume -= startVolume * (Time.deltaTime / fadeTime);
                yield return null;
            }

            gameAudioSource.volume = 0;
        }        

    }
}