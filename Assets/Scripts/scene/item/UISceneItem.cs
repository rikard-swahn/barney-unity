﻿using System;
using System.Collections.Generic;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using ui;
using UnityEngine.UI;

namespace net.gubbi.goofy.scene.item {

    public class UISceneItem : SceneItem {
        
        public bool setGameObject;
        public Hightlight[] highlightIfActive;
        public bool defaultInteractable = true;
        private List<Selectable> selectables;

        protected override void Awake() {
            base.Awake();            
            selectables = new List<Selectable>(transform.root.gameObject.GetComponentsInChildren<Selectable>());
        }

        protected override void refreshState () {
            bool interactable = isInteractable();
            selectables.ForEach(s => s.interactable = interactable);            
            
            bool active = isActive ();            
            highlightIfActive.ForEach(h => h.setHighlighted(active));

            if (setGameObject) {
                gameObject.SetActive(active);
            }
            else {
                GameItemUtil.setItemVisible(this, active);
            }
        }

        public void setInteractable(bool interactable) {
            GameState.Instance.StateData.setSceneProperty (gameObjectName(), SceneItemProperties.UI_INTERACTABLE, interactable);
            refreshState();
        }

        public bool isInteractable() {
            if (GameState.Instance.StateData.hasSceneProperty (gameObjectName (), SceneItemProperties.UI_INTERACTABLE)) {
                return GameState.Instance.StateData.getSceneProperty(gameObjectName (), SceneItemProperties.UI_INTERACTABLE).getBool();
            }

            return defaultInteractable;
        }
    }

}