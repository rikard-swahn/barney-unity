using System;
using net.gubbi.goofy.character;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.util;
using UnityEngine;

namespace scene.item.action {
    public class SayAction : SingleSceneItemAction {

        public string say;
        public bool sayTowardsCamera = true;
        public bool sayTowardsItem;

        private CharacterFacade playerFacade;
        private Vector3 baseLine;

        protected override void Awake () {
            base.Awake ();

            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerFacade = playerGo.GetComponent<CharacterFacade>();

            if (say.Equals ("")) {
                throw new ArgumentException("Text can not be empty on gameobject " + gameObject.name);
            }
            
            Transform baseLineTransform = target.transform.Find(GameObjects.ORDERED_RENDERING + "/" + GameObjects.BASE_LINE);
            if (baseLineTransform != null) {
                baseLine = baseLineTransform.position;
            }            
        }

        protected override void doAction (ItemType selectedItem) {
            if (sayTowardsItem) {
                playerFacade.turnTowards(baseLine);
            }
            if (sayTowardsCamera) {
                playerFacade.turnTowardsCamera();
            }

            string randomSay = I18nUtil.randomOption(say);
            playerFacade.sayTextRaw (randomSay, afterAction);
        }
    }

}