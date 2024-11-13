﻿using System;
using Mgl;
using net.gubbi.goofy.character;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.item;
 using net.gubbi.goofy.item.inventory;
 using net.gubbi.goofy.player;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using PolyNav;
using UnityEngine;

namespace net.gubbi.goofy.scene.action {
    public class CharacterSceneActionHandler : SceneActionHandler {

        public bool turnTowardsPlayerInstantly = true;
        public bool turnTowardsVisitPosInstantly;
        public bool turnPlayerTowardsBaseline;
        
        private PolyNavAgent navAgent;
        private ConversationService conversationService;
        private Transform visitTransform;
        private CharacterBehaviours behaviours;
        private CharacterFacade playerFacade;
        private CharacterMove playerMove;
        private CharacterFacade characterFacade;
        private SceneInputHandler sceneInput;
        private Vector3 characterBaseLine;
        private Inventory inventory;

        private static readonly string CONTROL_BY_ID = "action";

        private void Awake () {
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            navAgent = playerGo.GetComponentInChildren<PolyNavAgent>();
            playerFacade = playerGo.GetComponent<CharacterFacade> ();
            playerMove = playerGo.GetComponent<CharacterMove> ();

            characterFacade = GetComponent<CharacterFacade> ();
            characterBaseLine = transform.Find(GameObjects.ORDERED_RENDERING + "/" + GameObjects.BASE_LINE).position;
            visitTransform = transform.Find(GameObjects.VISIT_POSITION);
            conversationService = GetComponent<ConversationService>();
            behaviours = GetComponentInChildren<CharacterBehaviours> ();
            sceneInput = playerGo.GetComponent<SceneInputHandler>();
            inventory = GameObject.Find(GameObjects.INVENTORY).GetComponent<Inventory>();
        }

        public override void doAction (ItemType selectedItem) {
            sceneInput.setControlEnabled (false, CONTROL_BY_ID);                   

            Action<bool> finshedNavCallback = delegate {
                sceneInput.setControlEnabled (true, CONTROL_BY_ID);
                GameState.Instance.StateData.PlayerState.clearActionAndTarget();
                if (turnPlayerTowardsBaseline) {
                    playerFacade.turnTowards (characterBaseLine);
                }
                else {
                    playerFacade.turnTowards (gameObject.transform.position);                            
                }                
                characterFacade.turnTowards (playerMove.getPosition ());
                conversationService.begin(selectedItem);
            };

            if (behaviours != null) {
                behaviours.freeze ();
            }

            if (characterFacade != null && turnTowardsPlayerInstantly) {
                characterFacade.turnTowards (playerMove.getPosition ());
            }            

            if (characterFacade != null && turnTowardsVisitPosInstantly) {
                characterFacade.turnTowards (visitTransform.position);
            }            

            if (visitTransform != null) {
                GameState.Instance.StateData.PlayerState.SceneActionTargetName = gameObject.name;
                
                Action abortNav = delegate {                    
                    GameState.Instance.StateData.PlayerState.clearActionAndTarget();
                    sceneInput.setControlEnabled (true, CONTROL_BY_ID);
                    inventory.deselectItem();
                    
                    if (behaviours != null) {
                        behaviours.unFreeze();
                    }
                };                   
                
                navAgent.SetDestination (visitTransform.position, finshedNavCallback, abortNav);
            } else {
                finshedNavCallback (true);
            }
        }

        public override string getLabelPrefix(ItemType selectedItem) {
            return I18n.Instance.__("TalkTo");
        }
    }
}