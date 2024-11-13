using System;
using net.gubbi.goofy.character;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class LookAction : SingleSceneItemAction {

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
                throw new ArgumentException("Text can not be empty on game object " + gameObject.name);
            }

            ItemSelectedFilter nothingSelected = ScriptableObject.CreateInstance<ItemSelectedFilter>();
            nothingSelected.item = ItemType.EMPTY;
            conditions.Add(nothingSelected);

            Transform baseLineTransform = target.transform.Find(GameObjects.ORDERED_RENDERING + "/" + GameObjects.BASE_LINE);
            if (baseLineTransform != null) {
                baseLine = baseLineTransform.position;
            }
            else if (sayTowardsItem) {
                throw new Exception("No baseline, but sayTowardsItem");
            }
        }

        protected override void doAction (ItemType selectedItem) {
            if (sayTowardsItem) {
                playerFacade.turnTowards(baseLine);
            }
            
            if (sayTowardsCamera) {
                playerFacade.sayTextTowardsCamera (say, afterAction);
            } else  {
                playerFacade.sayText (say, afterAction);
            }
        }
    }

}