using System;
using net.gubbi.goofy.character;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.util;
using UnityEngine;

namespace scene.item.action {
    public class SayRandomAction : SingleSceneItemAction {

        public string say;
        public bool sayTowardsCamera = true;

        private CharacterFacade playerFacade;

        protected override void Awake () {
            base.Awake ();

            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerFacade = playerGo.GetComponent<CharacterFacade>();

            if (say.Equals ("")) {
                throw new ArgumentException("Text can not be empty on gameobject " + gameObject.name);
            }
        }

        protected override void doAction (ItemType selectedItem) {
            string option = I18nUtil.randomOption(say);
            
            if (sayTowardsCamera) {
                playerFacade.turnTowardsCamera();
            }
            
            playerFacade.sayTextRaw (option, afterAction);
        }
    }

}