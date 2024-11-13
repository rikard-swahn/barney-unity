using System;
using System.Collections;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.game;
using net.gubbi.goofy.state.settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace net.gubbi.goofy.audio {
    public class MusicPlayer : MonoBehaviour {

        public float crossFadeTime = 1f;
        public float fadeInTime = 1f;
        public float fadeOutTime = 0.5f;

        private AudioSource audioSource;
        private IEnumerator fadeOutAndInRoutine;
        private IEnumerator fadeInRoutine;
        private IEnumerator fadeOutRoutine;
        private IEnumerator stopWithFadeOutRoutine;
        private AudioClip clip;
        private IEnumerator audioFinishedRoutine;

        private static readonly float MAX_VOLUME = 1f;
        private static MusicPlayer instance;
        private bool inSceneTransitionOut;

        private void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
                audioSource = GetComponent<AudioSource>();                
            }
            else if (instance != this) {
                instance.crossFadeTime = this.crossFadeTime;
                instance.fadeInTime = this.fadeInTime;
                instance.fadeOutTime = this.fadeOutTime;
                Destroy(gameObject);
            }
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (instance == this) {
                EventManager.Instance.addListener<GameEvents.MusicEnabledEvent>(musicEnabledHandler);
                EventManager.Instance.addListener<GameEvents.MusicDisabledEvent>(musicDisabledHandler);
                EventManager.Instance.addListener<GameEvents.PauseGameEvent>(pauseGameHandler);
                EventManager.Instance.addListener<GameEvents.UnPauseGameEvent>(unPauseGameHandler);
                EventManager.Instance.addListener<GameEvents.SceneChangeInitEvent>(handleSceneChangeStarted);
                EventManager.Instance.addListener<GameEvents.SceneLoadedEvent>(handleSceneLoaded);
            }
        }

        public void playDelayed(MusicClip clip) {
            if (clip.clip != null) {
                this.delayedAction(
                    () => {
                        if (clip.loop) {
                            playLooping(clip);
                        }
                        else {
                            playOnce(clip);
                        }
                    },
                    clip.delay
                );
            }
        }

        public void playLooping(AudioClip clip) {
            if (inSceneTransitionOut) {
                return;
            }
            
            audioSource.loop = true;
            removeOnFinishedAction();
            this.clip = clip;

            if (clip == null) {
                fadeOutStop();
            }
            else if (SettingsState.Instance.StateData.MusicEnabled) {
                playWithFadeIn();
            }
        }

        private void playLooping(MusicClip musicClip) {
            if (inSceneTransitionOut) {
                return;
            }                
            
            audioSource.loop = true;
            removeOnFinishedAction();
            play(musicClip);
        }

        private void playOnce(MusicClip musicClip) {
            if (inSceneTransitionOut) {
                return;
            }            
            
            audioSource.loop = false;
            removeOnFinishedAction();
            play(musicClip);
        }

        private void play(MusicClip musicClip) {
            this.clip = musicClip != null ? musicClip.clip : null;

            if (musicClip == null || musicClip.clip == null) {
                fadeOutStop();
            }
            else if (SettingsState.Instance.StateData.MusicEnabled) {
                playWithFadeIn();

                if (!audioSource.loop) {
                    raiseEventOnFinished();
                }
            }
        }

        public void fadeOutStop() {
            removeOnFinishedAction();
            stopFade();
            stopWithFadeOutRoutine = fadeOutStopRoutine();
            StartCoroutine(stopWithFadeOutRoutine);
        }

        private IEnumerator fadeOutStopRoutine() {
            if (audioSource.isPlaying) {
                fadeOutRoutine = fadeOut(fadeOutTime);
                yield return StartCoroutine(fadeOutRoutine);
                audioSource.Stop();
                audioSource.clip = null;
                this.clip = null;
                EventManager.Instance.raise(new GameEvents.MusicFinishedEvent());
            }
            else {
                audioSource.Stop();
                audioSource.clip = null;
                this.clip = null;
            }
        }

        private void playWithFadeIn() {
            stopFade();

            if (audioSource.isPlaying) {
                if (!audioSource.clip.name.Equals(clip.name)) {
                    //A different clip should play. Fade out the current and fade in the new
                    fadeOutAndInRoutine = fadeOutAndIn(clip, crossFadeTime);
                    StartCoroutine(fadeOutAndInRoutine);
                }
                else {
                    //Same clip, fade it in to target volume
                    fadeInRoutine = fadeIn(MAX_VOLUME, fadeInTime);
                    StartCoroutine(fadeInRoutine);
                }
            }
            else {
                audioSource.clip = clip;
                audioSource.volume = 0f;
                playWithPauseSupport();
                audioSource.volume = MAX_VOLUME;
            }
        }

        private void stopFade() {
            if (fadeInRoutine != null) {
                StopCoroutine(fadeInRoutine);
                fadeInRoutine = null;
            }

            if (fadeOutRoutine != null) {
                StopCoroutine(fadeOutRoutine);
                fadeOutRoutine = null;
            }

            if (fadeOutAndInRoutine != null) {
                StopCoroutine(fadeOutAndInRoutine);
                fadeOutAndInRoutine = null;
            }

            if (stopWithFadeOutRoutine != null) {
                StopCoroutine(stopWithFadeOutRoutine);
                stopWithFadeOutRoutine = null;
            }
        }

        private IEnumerator fadeOutAndIn(AudioClip nextClip, float fadeTime) {
            fadeOutRoutine = fadeOut(fadeTime);
            yield return StartCoroutine(fadeOutRoutine);

            audioSource.Stop();
            audioSource.clip = nextClip;
            playWithPauseSupport();
            fadeInRoutine = fadeIn(MAX_VOLUME, fadeTime);
            yield return StartCoroutine(fadeInRoutine);
        }

        private IEnumerator fadeOut(float fadeTime) {
            float startVolume = audioSource.volume;
            fadeTime = fadeTime * (startVolume / MAX_VOLUME);

            while (audioSource.volume > 0) {
                audioSource.volume -= startVolume * (Time.deltaTime / fadeTime);
                yield return null;
            }

            audioSource.volume = 0;
        }

        private IEnumerator fadeIn(float fadeToVolume, float fadeTime) {
            float startVolume = audioSource.volume;
            float volumeChange = fadeToVolume - startVolume;
            fadeTime = fadeTime * (volumeChange / MAX_VOLUME);

            while (audioSource.volume < fadeToVolume && volumeChange > 0) {
                audioSource.volume += volumeChange * (Time.deltaTime / fadeTime);
                yield return null;
            }

            audioSource.volume = fadeToVolume;
        }

        private void removeOnFinishedAction() {
            if (audioFinishedRoutine != null) {
                StopCoroutine(audioFinishedRoutine);
                audioFinishedRoutine = null;
            }
        }

        private void raiseEventOnFinished() {
            if (audioFinishedRoutine != null) {
                return;
            }

            this.audioFinishedRoutine = whenAudioFinished(delegate {
                EventManager.Instance.raise(new GameEvents.MusicFinishedEvent());
            });
            StartCoroutine(audioFinishedRoutine);
        }

        private IEnumerator whenAudioFinished(Action onFinished) {
            yield return new WaitUntil(() =>
                !audioSource.isPlaying && audioSource.time.looseEquals(clip.length, 0.01f));
            onFinished();
        }

        private void musicEnabledHandler(GameEvents.MusicEnabledEvent e) {
            if (clip != null) {
                audioSource.clip = clip;
                audioSource.volume = MAX_VOLUME;

                playWithPauseSupport();

                if (!audioSource.loop) {
                    raiseEventOnFinished();
                }
            }
        }

        private void playWithPauseSupport() {
            audioSource.Play();
            if (Pause.Instance.isPaused()) {
                audioSource.Pause();
            }
        }

        private void musicDisabledHandler(GameEvents.MusicDisabledEvent e) {
            removeOnFinishedAction();
            stopFade();
            audioSource.Stop();
            audioSource.clip = null;
            clip = null;
            EventManager.Instance.raise(new GameEvents.MusicFinishedEvent());            
        }

        private void pauseGameHandler(GameEvents.PauseGameEvent e) {
            if (audioSource.isPlaying) {
                audioSource.Pause();
            }
        }

        private void unPauseGameHandler(GameEvents.UnPauseGameEvent e) {
            audioSource.UnPause();
        }

        private void handleSceneChangeStarted(GameEvents.SceneChangeInitEvent e) {
            inSceneTransitionOut = true;

            if (e.OutTransition == null || e.OutTransition.fadeMusic) {
                fadeOutStop();    
            }
        }

        private void handleSceneLoaded(GameEvents.SceneLoadedEvent e) {
            inSceneTransitionOut = false;
        }
    }
}