using System.Collections.Generic;
using net.gubbi.goofy.events;
using UnityEngine;
using Object = System.Object;

namespace net.gubbi.goofy.game {
    public class Pause {
        private HashSet<string> pausedBy = new HashSet<string>();

        private static volatile Pause instance;
        private static object syncRoot = new Object();
        private Pause() {}
        public static Pause Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) {
                            instance = new Pause();
                        }
                    }
                }
                return instance;
            }
        }

        public bool isPaused() {
            return pausedBy.Count > 0;
        }

        public void pause(string byId) {
            if (!isPaused()) {
                EventManager.Instance.raise(new GameEvents.PauseGameEvent());
                Time.timeScale = 0f;
            }

            pausedBy.Add(byId);
        }

        public void unPause(string byId) {
            if (!isPaused()) {
                return;
            }

            pausedBy.Remove(byId);

            if (!isPaused()) {
                EventManager.Instance.raise(new GameEvents.UnPauseGameEvent());
                Time.timeScale = 1f;
            }
        }
    }
}