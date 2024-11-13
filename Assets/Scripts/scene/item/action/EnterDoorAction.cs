﻿using System;
using System.Collections;
using System.Linq;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using net.gubbi.goofy.player;
using net.gubbi.goofy.scene.door;
using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.util;
using PolyNav;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class EnterDoorAction : SingleSceneItemAction {
        
        public Transitions.Type outTransition;
        public Transitions.Type inTransition;
        public bool closeDoor;
        public float waitAfterClosing = Constants.DEFAULT_ACTION_TIME;
        public bool setCollisionDisabled;
        
        private Door door;
        private Vector2 goTo;
        private PlayerMove playerMove;
        private PolyNavAgent polyNavAgent;

        protected override void Awake () {
            base.Awake ();
            door = GetComponent<Door> ();
            
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerMove = playerGo.GetComponent<PlayerMove>();
            polyNavAgent = playerGo.GetComponentInChildren<PolyNavAgent>();
            if (door.insideWaypoint != null) {
                goTo = door.insideWaypoint.gameObject.transform.position;
            }
            
            if (conditions.All(c => !c.contains<ItemSelectedFilter>())) {
                ItemSelectedFilter nothingSelected = ScriptableObject.CreateInstance<ItemSelectedFilter>();
                nothingSelected.item = ItemType.EMPTY;
                conditions.Add(nothingSelected);                
            }            
        }

        protected override void doAction (ItemType selectedItem) {
            Action<bool> onCompleteWrapper = delegate {
                if (closeDoor) {
                    door.setOpen(false);
                    
                    StartCoroutine(wait (delegate {
                        door.enter(false, Transitions.get(outTransition), Transitions.get(inTransition), afterAction);
                    }, waitAfterClosing));
                }
                else {
                    door.enter(false, Transitions.get(outTransition), Transitions.get(inTransition), afterAction);
                }
                
            };

            if (setCollisionDisabled) {
                playerMove.setCollisionDisabled(true);
            }
            else {
                polyNavAgent.disableCollision = true;
            }

            if (door.insideWaypoint != null) {
                playerMove.SetDestination(goTo, onCompleteWrapper);
            }
            else {
                onCompleteWrapper(true);
            }
        }

        private IEnumerator wait(Action afterWait, float waitTime) {
            yield return new WaitForSeconds(waitTime);
            afterWait();
        }

    }

}