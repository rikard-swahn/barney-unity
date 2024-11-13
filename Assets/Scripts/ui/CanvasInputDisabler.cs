using net.gubbi.goofy.events;
using net.gubbi.goofy.ui.cursor;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui {
    public class CanvasInputDisabler : MonoBehaviour {

        public bool RaiseCursorEvents;
        public bool disableOnActions;
        
        private GraphicRaycaster graphicRaycaster;
        private bool rayCasterEnabledAtStart;
        private bool inTransition;
        private bool inAction;

        private void Awake() {
            graphicRaycaster = GetComponent<GraphicRaycaster>();
            rayCasterEnabledAtStart = graphicRaycaster.enabled;
            EventManager.Instance.addListener<GameEvents.SceneTransitionStartedEvent>(sceneLoadStartedHandler);
            EventManager.Instance.addListener<GameEvents.SceneTransitionCompletedEvent>(sceneTransitionCompletedHandler);

            if (disableOnActions) {
                EventManager.Instance.addListener<GameEvents.UIActionStartEvent>(uiActionStartedHandler);
                EventManager.Instance.addListener<GameEvents.UIActionEndEvent>(uiActionEndedHandler);
            }
        }

        private void sceneLoadStartedHandler(GameEvents.SceneTransitionStartedEvent e) {
            if (RaiseCursorEvents) {
                EventManager.Instance.raise(new GameEvents.CursorEvent(CursorType.DISABLED));
            }

            graphicRaycaster.enabled = false;
            inTransition = true;
        }
        
        private void sceneTransitionCompletedHandler(GameEvents.SceneTransitionCompletedEvent e) {
            if (RaiseCursorEvents) {
                EventManager.Instance.raise(new GameEvents.CursorEvent(CursorType.NORMAL));    
            }
            
            graphicRaycaster.enabled = rayCasterEnabledAtStart && !inAction;
            inTransition = false;
        }
        
        private void uiActionStartedHandler(GameEvents.UIActionStartEvent e) {
            graphicRaycaster.enabled = false;
            inAction = true;
        }

        private void uiActionEndedHandler(GameEvents.UIActionEndEvent e) {
            graphicRaycaster.enabled = rayCasterEnabledAtStart && !inTransition;
            inAction = false;
        }        

    }
}