using System;
using net.gubbi.goofy.events;
using UnityEngine;

namespace net.gubbi.goofy.scene.load.transition {
        
    public abstract class SceneTransition : ScriptableObject {
        
        public float transitionTime = 1f;
        public bool fadeMusic;

        [NonSerialized]
        private readonly int drawDepth = -1000;        
        [NonSerialized]
        protected float progress;
        [NonSerialized]
        private bool transitioning;
        [NonSerialized]
        private Action onComplete;

        public virtual bool start(Action onComplete) {
            if (transitioning) {
                Debug.LogWarning("Already transitioning. Not starting transition!");
                return false;
            }
            transitioning = true;
            this.onComplete = onComplete;
            EventManager.Instance.addListener<GameEvents.RepaintEvent>(handleRepaintEvent);
            return true;
        }

        public virtual void stop() {            
            progress = 0f;
            transitioning = false;
            EventManager.Instance.removeListener<GameEvents.RepaintEvent>(handleRepaintEvent);
        }

        private void handleRepaintEvent(GameEvents.RepaintEvent e) {
            if (!transitioning) {
                return;
            }

            if (progress < 1) {
                progress += (Time.deltaTime / transitionTime);
                progress = Mathf.Clamp01(progress);
            }
            
            GUI.depth = drawDepth;
            doOnGUI();             
            
            if (progress == 1f) {
                completeTransition();
            }            
        }

        protected abstract void doOnGUI();

        protected virtual void completeTransition() {            
            if (onComplete != null) {
                onComplete();
            }
        }

     
    }
}