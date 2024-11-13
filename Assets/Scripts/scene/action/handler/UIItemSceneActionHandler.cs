﻿using System;
 using net.gubbi.goofy.character;
 using net.gubbi.goofy.events;
 using net.gubbi.goofy.item;
 using net.gubbi.goofy.item.inventory;
 using net.gubbi.goofy.scene.item;
 using net.gubbi.goofy.state;
 using net.gubbi.goofy.util;
 using scene.item.action;
 using UnityEngine;

namespace net.gubbi.goofy.scene.action {
    public class UIItemSceneActionHandler : SceneActionHandler {

        private UISceneItem sceneItem;
        private ItemType selectedItemOnAction;
        private CharacterFacade playerFacade;        
        private Inventory inventory;

        private void Awake () {
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerFacade = playerGo.GetComponent<CharacterFacade>();

            sceneItem = GetComponent<UISceneItem>();
            inventory = GameObject.Find(GameObjects.INVENTORY).GetComponent<Inventory>();
        }

        public void doAction() {
            doAction(ItemType.EMPTY);
        }

        public override void doAction (ItemType selectedItem) {
            Action afterSceneItemAction = delegate {
                GameState.Instance.StateData.PlayerState.clearActionAndTarget();                                                
                inventory.deselectItem();
                EventManager.Instance.raise(new GameEvents.UIActionEndEvent());
            };
            
            EventManager.Instance.raise(new GameEvents.UIActionStartEvent());                        
            GameState.Instance.StateData.PlayerState.SceneActionTargetName = gameObject.name;            
            bool handled = sceneItem.doAction(selectedItem, afterSceneItemAction);

            if (!handled && selectedItem != ItemType.EMPTY) {                                 
                playerFacade.sayTextTowardsCamera (I18nUtil.randomOption("FailedAction"), afterSceneItemAction);                    
            }
        }

        public override string getLabelPrefix(ItemType selectedItem) {
            SceneItemAction action = sceneItem.getActionToDo(selectedItem);
            return action != null ? action.getLabelPrefix() : null;
        }
    }
}