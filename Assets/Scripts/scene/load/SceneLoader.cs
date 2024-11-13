using System;
using net.gubbi.goofy.events;
using net.gubbi.goofy.scene.load.transition;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace net.gubbi.goofy.scene.load {

    public class SceneLoader {

        private bool sceneLoadLock;        
        private SceneTransition outTransition;
        private SceneTransition inTransition;
        
        private static volatile SceneLoader instance;
        private static object syncRoot = new Object();
        private SceneLoader() {
            SceneManager.sceneLoaded += OnSceneLoaded;            
        }
        public static SceneLoader Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) {
                            instance = new SceneLoader();
                        }
                    }
                }
                return instance;
            }
        }        

        public void changeInGameScene (string sceneName, SceneTransition outTransition = null, SceneTransition inTransition = null, Action onOutComplete = null) {
            stopTransitions();            
            this.outTransition = outTransition;
            this.inTransition = inTransition;
                                                
            EventManager.Instance.raise (new GameEvents.SceneChangeInitEvent(outTransition));
            EventManager.Instance.raise (new GameEvents.SceneChangeInCurrentGameInitEvent(sceneName));
            Action onOutCompleteWrapper = delegate {
                if (!loadLock(sceneName)) {
                    return;
                }                
                
                EventManager.Instance.raise(new GameEvents.SceneChangeInCurrentGameEvent(sceneName));
                if (onOutComplete != null) {
                    onOutComplete();
                }                
                loadSceneInternal(sceneName);
            };            
            doTransition(outTransition, onOutCompleteWrapper);
        }

        public void loadScene (string sceneName, SceneTransition outTransition = null, SceneTransition inTransition = null, Action onOutComplete = null) {
            stopTransitions();
            this.outTransition = outTransition;
            this.inTransition = inTransition;
            
            EventManager.Instance.raise (new GameEvents.SceneChangeInitEvent(outTransition));
            Action onOutCompleteWrapper = delegate {
                if (!loadLock(sceneName)) {
                    return;
                }
                
                if (onOutComplete != null) {
                    onOutComplete();
                }
                loadSceneInternal(sceneName);
            };
            doTransition(outTransition, onOutCompleteWrapper);
        }

        private void stopTransitions() {
            if (outTransition != null) {
                outTransition.stop();
            }
            
            if (inTransition != null) {
                inTransition.stop();
            }
        }

        private void loadSceneInternal (string sceneName) {            
            EventManager.Instance.removeListeners<GameEvents.SceneEvent>();
            SceneManager.LoadScene (sceneName);
        }

        private bool loadLock(string sceneName) {
            if (sceneLoadLock) {
                Debug.LogWarning("load scene " + sceneName + " denied");
                return false;
            }
            sceneLoadLock = true;
            return true;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            sceneLoadLock = false;
            
            if (outTransition != null) {
                outTransition.stop();
            }
            
            EventManager.Instance.raise(new GameEvents.SceneLoadedEvent(scene.name));
                                    
            doTransition(inTransition);                            
        }
        
        private void doTransition(SceneTransition transition, Action onComplete = null) {
            if (transition != null) {
                EventManager.Instance.raise(new GameEvents.SceneTransitionStartedEvent());

                Action onCompleteWrapper = () => {
                    EventManager.Instance.raise(new GameEvents.SceneTransitionCompletedEvent());
                    if (onComplete != null) {
                        onComplete();
                    }
                };
                
                transition.start(onCompleteWrapper);
            }
            else {
                if (onComplete != null) {
                    onComplete();
                }
            }
        }        
                
    }

}