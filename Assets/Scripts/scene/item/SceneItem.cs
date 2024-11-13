using System;
using net.gubbi.goofy.item;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using PolyNav;
using scene.item.action;
using UnityEngine;

namespace net.gubbi.goofy.scene.item {

    public class SceneItem : MonoBehaviour {

        public SceneItemAction[] actions;
        public bool defaultActive = true;
        private PolyNavObstacle obstacle;

        protected virtual void Awake() {
            obstacle = GetComponentInChildren<PolyNavObstacle>();
        }

        private void Start() {
            activateOnLoad();
            refreshState ();
        }

        private void activateOnLoad() {
            if (GameState.Instance.StateData.hasSceneProperty (gameObjectName (), SceneItemProperties.ACTIVATE_ON_LOAD)) {
                GameState.Instance.StateData.removeSeneProperty(gameObjectName (), SceneItemProperties.ACTIVATE_ON_LOAD);
                GameState.Instance.StateData.setSceneProperty(gameObjectName(), SceneItemProperties.ACTIVE, true);
            }
        }

        public void setActive(bool active) {
            GameState.Instance.StateData.setSceneProperty (gameObjectName(), SceneItemProperties.ACTIVE, active);
            refreshState ();            
        }

        public void remove() {
            GameState.Instance.StateData.setSceneProperty (gameObjectName(), SceneItemProperties.ACTIVE, false);
            refreshState ();
        }

        public bool doAction (ItemType selectedItem, Action onComplete) {
            int? actionIndex = GameState.Instance.StateData.PlayerState.ItemActionIndex;
            if (actionIndex != null) {
                actions[(int) actionIndex].resume(selectedItem, onComplete);
                return true;
            }

            for(int i = 0; i < actions.Length; i++) {
                SceneItemAction action = actions[i];                
                if (action.filter(selectedItem)) {
                    GameState.Instance.StateData.PlayerState.ItemActionIndex = i;
                    action.start(selectedItem, onComplete);
                    return true;
                }
            }
            
            return false;
        }

        public SceneItemAction getActionToDo(ItemType selectedItem) {
            for(int i = 0; i < actions.Length; i++) {
                SceneItemAction action = actions[i];                
                if (action.filter(selectedItem)) {                    
                    return action;
                }
            }

            return null;
        }
			
        protected virtual void refreshState () {
            bool active = isActive ();
            setUiActive(active);
        }

        public void setUiActive(bool active) {
            GameItemUtil.setItemVisible (gameObject, active);

            if (obstacle != null) {
                if (active) {
                    polyNav.AddObstacle(obstacle);
                }
                else {
                    polyNav.RemoveObstacle(obstacle);
                }
            }
        }

        public bool isActive() {
            if (GameState.Instance.StateData.hasSceneProperty (gameObjectName (), SceneItemProperties.ACTIVE)) {
                return GameState.Instance.StateData.getSceneProperty(gameObjectName (), SceneItemProperties.ACTIVE).getBool();
            }

            return defaultActive;
        }
        
        ///The PolyNav singleton
        private PolyNav2D polyNav {
            get {return PolyNav2D.current;}
        }

        protected string gameObjectName() {			
            return gameObject.name;
        }
    }

}