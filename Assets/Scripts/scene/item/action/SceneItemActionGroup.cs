using System;
using System.Collections.Generic;
using System.Linq;
using net.gubbi.goofy.events;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using net.gubbi.goofy.player;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace scene.item.action {
    public class SceneItemActionGroup : SceneItemAction {
        public List<SingleSceneItemAction> actions;

        private int actionIndex;
        private Action onCompletedGroup;
        private SceneInputHandler sceneInput;

        private static readonly int START_ACTION = 0;
        private bool disabled;

        protected void Awake() {
            sceneInput = GameObject.Find(GameObjects.PLAYER).GetComponent<SceneInputHandler>();
            EventManager.Instance.addListener<GameEvents.SceneChangeInitEvent>(handleSceneChange);

            if (actions.Count == 0) {
                actions = GetComponents<SingleSceneItemAction>().ToList();
            }
        }

        public override void start(ItemType selectedItem, Action onComplete) {
            setActionIndex(START_ACTION);
            onCompletedGroup = onComplete;

            doAction(selectedItem);
        }

        public override bool filter(ItemType selectedItem) {
            FilterContext ctx = FilterContext.builder()
                .build();

            foreach (var action in actions) {
                ctx.mergeProperties(action.getFilterContext(selectedItem));
            }

            foreach (var condition in conditions) {
                if (!condition.matches(ctx)) {
                    return false;
                }
            }

            foreach (var action in actions) {
                if (!action.conditionsMatches(ctx)) {
                    return false;
                }
            }
            
            return true;
        }
        public override void resume(ItemType selectedItem, Action onComplete) {
            setActionIndex(getCurrentActionStartValue());
            onCompletedGroup = onComplete;
            resumeAction(selectedItem);
        }
        
        public override string getLabelPrefix() {
            SingleSceneItemAction action = actions.FirstOrDefault(a => a.getLabelPrefix() != null);
            return action != null ? action.getLabelPrefix() : null;
        }        
        
        private void resumeAction(ItemType selectedItem) {
            sceneInput.setControlEnabled (false, getControlById());
            Action onCompletedActionWrapper = delegate { onCompletedAction(selectedItem); };
            SingleSceneItemAction action = actions.ElementAt(actionIndex);
            action.resume(selectedItem, onCompletedActionWrapper);
        }                
        private void doAction(ItemType selectedItem) {
            sceneInput.setControlEnabled (false, getControlById());
            Action onCompletedActionWrapper = delegate { onCompletedAction(selectedItem); };
            SingleSceneItemAction action = actions.ElementAt(actionIndex);
            action.doFullAction(selectedItem, onCompletedActionWrapper);
        }

        private void onCompletedAction(ItemType selectedItem) {
            if (disabled) {
                return;
            }
            
            sceneInput.setControlEnabled (true, getControlById());
            if (actionIndex < actions.Count - 1) {
                setActionIndex (actionIndex + 1);
                doAction (selectedItem);
            } else {
                setActionIndex(START_ACTION);
                onCompletedGroup ();
            }
        }

        private void setActionIndex (int actionIndex) {
            this.actionIndex = actionIndex;
            GameState.Instance.StateData.PlayerState.ItemActionGroupIndex = actionIndex;
        }

        private int getCurrentActionStartValue () {
            int? action = GameState.Instance.StateData.PlayerState.ItemActionGroupIndex;

            if (action != null) {
                return (int)action;
            }

            return START_ACTION;
        }
        
        private void handleSceneChange(GameEvents.SceneChangeInitEvent e) {
            disabled = true;
        }
        

    }
}