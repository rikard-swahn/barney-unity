using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.game;
using ui;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui {
    public class InputDisabledUI : HandheldEnable {

        private bool playerControlEnabled = true;
        private bool controlDisabledOverride;

        private Graphic[] graphicElements;

        protected override void Awake() {            
            base.Awake();
            graphicElements = GetComponentsInChildren<Graphic>();
            
            if (enabled) {
                EventManager.Instance.addListener<GameEvents.PlayerControlEnabledEvent>(playerControlEnabledHandler);
                EventManager.Instance.addListener<GameEvents.PlayerControlDisabledEvent>(playerControlDisabledHandler);
                EventManager.Instance.addListener<GameEvents.SceneTransitionStartedEvent>(sceneTransitionStartedHandler);
                EventManager.Instance.addListener<GameEvents.SceneTransitionCompletedEvent>(sceneTransitionCompletedHandler);                            
            }
            
            showDisabled(false);
        }

        private void Update() {
            if (enabled) {
                bool disabled = !playerControlEnabled && !InteractionState.Instance.hasOnlyState(InteractionState.StateType.SCENE_CANVAS) 
                               || controlDisabledOverride;
                
               showDisabled(disabled);                
            }            
        }

        private void showDisabled(bool disabled) {
            graphicElements.ForEach(e => e.enabled = disabled);
        }

        private void playerControlEnabledHandler(GameEvents.PlayerControlEnabledEvent e) {
            playerControlEnabled = true;
            
        }
        private void playerControlDisabledHandler(GameEvents.PlayerControlDisabledEvent e) {
            playerControlEnabled = false;
        }
        
        private void sceneTransitionStartedHandler(GameEvents.SceneTransitionStartedEvent e) {
            controlDisabledOverride = true;
        }
        private void sceneTransitionCompletedHandler(GameEvents.SceneTransitionCompletedEvent e) {
            controlDisabledOverride = false;
        }

    }
}