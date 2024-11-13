﻿using System;
using Mgl;
using net.gubbi.goofy.character;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
 using net.gubbi.goofy.player;
 using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using PolyNav;
using scene.item.action;
using UnityEngine;

namespace net.gubbi.goofy.scene.action {
    public class ItemSceneActionHandler : SceneActionHandler {

        public bool controlEnabled = true;

        private SceneItem sceneItem;
        private PolyNavAgent navAgent;
        private Vector3 visitPos = Vector3.zero;
        private ItemType selectedItemOnAction;
        private CharacterFacade playerFacade;
        private Inventory inventory;
        private SceneInputHandler sceneInput;
        
        private static readonly string CONTROL_BY_ID = "action";

        private void Awake () {
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            navAgent = playerGo.GetComponentInChildren<PolyNavAgent>();
            playerFacade = playerGo.GetComponent<CharacterFacade>();
            sceneInput = playerGo.GetComponent<SceneInputHandler>();

            Transform visitTr = transform.Find(GameObjects.VISIT_POSITION);
            if (visitTr != null) {
                visitPos = visitTr.position;
            }

            sceneItem = GetComponent<SceneItem>();
            inventory = GameObject.Find(GameObjects.INVENTORY).GetComponent<Inventory>();
        }

        public override void doAction (ItemType selectedItem) {
            if (!controlEnabled) {
                sceneInput.setControlEnabled (false, CONTROL_BY_ID);    
            }
                        
            Action clearTarget = delegate {
                GameState.Instance.StateData.PlayerState.clearActionAndTarget();
                inventory.deselectItem();
            };            
            
            Action<bool> finshedNavCallback = delegate {
                if (!controlEnabled) {
                    sceneInput.setControlEnabled(true, CONTROL_BY_ID);
                }

                bool handled = sceneItem.doAction(selectedItem, clearTarget);

                if (!handled && selectedItem != ItemType.EMPTY) {
                    playerFacade.sayTextTowardsCamera (I18nUtil.randomOption("FailedAction"), clearTarget);                    
                }
            };
				
            GameState.Instance.StateData.PlayerState.SceneActionTargetName = gameObject.name;
            if (!Vector3.zero.Equals(visitPos) && GameState.Instance.StateData.PlayerState.ItemActionIndex == null) {                              
                Action abortNav = delegate {                    
                    if (!controlEnabled) {
                        sceneInput.setControlEnabled(true, CONTROL_BY_ID);
                    }
                    clearTarget();                                        
                };                
                
                navAgent.SetDestination (visitPos, finshedNavCallback, abortNav);
            } else {
                finshedNavCallback (true);
            }
        }

        public override string getLabelPrefix(ItemType selectedItem) {
            SceneItemAction action = sceneItem.getActionToDo(selectedItem);
            return action != null ? action.getLabelPrefix() : null;
        }
    }
}