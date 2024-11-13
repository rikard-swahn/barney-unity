﻿using System;
using net.gubbi.goofy.character;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.item;
using net.gubbi.goofy.say.conversation;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.util;
using PolyNav;
using UnityEngine;

namespace Assets.Scripts.scene.item.action {
    public class CharacterConversationAction : SingleSceneItemAction {

        public GameObject character;
        public bool clearSelectedItem;

        private PolyNavAgent navAgent;
        private ConversationService conversationService;
        private Transform visitTransform;
        private CharacterBehaviours behaviours;
        private CharacterFacade playerFacade;
        private CharacterMove playerMove;
        private CharacterFacade characterFacade;

        protected override void Awake () {
            base.Awake ();

            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            navAgent = playerGo.GetComponentInChildren<PolyNavAgent>();
            playerMove = playerGo.GetComponent<CharacterMove> ();
            playerFacade = playerGo.GetComponent<CharacterFacade> ();

            characterFacade = character.GetComponent<CharacterFacade> ();
            visitTransform = character.transform.Find(GameObjects.VISIT_POSITION);
            conversationService = character.GetComponent<ConversationService>();
            behaviours = character.GetComponentInChildren<CharacterBehaviours> ();
        }

        protected override void doAction (ItemType selectedItem) {
            if (clearSelectedItem) {
                selectedItem = ItemType.EMPTY;
            }

            Action<bool> finshedNavCallback = delegate {
                playerFacade.turnTowards (character.gameObject.transform.position);
                characterFacade.turnTowards (playerMove.getPosition ());
                conversationService.begin(selectedItem);
                afterAction();
            };

            if (behaviours != null) {
                behaviours.freeze ();
            }

            if (characterFacade != null) {
                characterFacade.turnTowards (playerMove.getPosition ());
            }

            if (visitTransform != null) {
                navAgent.SetDestination (visitTransform.position, finshedNavCallback);
            } else {
                finshedNavCallback (true);
            }
        }
    }

}