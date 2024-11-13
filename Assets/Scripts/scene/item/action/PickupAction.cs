using System.Linq;
using net.gubbi.goofy.character;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.player;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public abstract class PickupAction : SingleSceneItemAction {

        public bool removeSceneItem = true;
        public float actionTime = Constants.DEFAULT_ACTION_TIME;

        protected Inventory inventory;
        private CharacterFacade playerFacade;
        private TimedAnimatorFlag timedAnimatorFlag;
        private Vector3 baseLine;
        private SceneItem sceneItem;

        protected override void Awake () {
            base.Awake ();

            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerFacade = playerGo.GetComponent<CharacterFacade>();

            timedAnimatorFlag = playerGo.GetComponent<TimedAnimatorFlag>();

            GameObject inventoryGo = GameObject.Find(GameObjects.INVENTORY);
            inventory = inventoryGo.GetComponent<Inventory>();

            Transform renderingTrans = target.transform.Find(GameObjects.ORDERED_RENDERING + "/" + GameObjects.BASE_LINE);
            if (renderingTrans != null) {
                baseLine = renderingTrans.position;
            }

            if (conditions.All(c => c.GetType() != typeof(ItemSelectedFilter))) {
                ItemSelectedFilter nothingSelected = ScriptableObject.CreateInstance<ItemSelectedFilter>();
                nothingSelected.item = ItemType.EMPTY;
                conditions.Add(nothingSelected);                
            }
            
            sceneItem = target.GetComponent<SceneItem>();
        }

        protected override void doAction (ItemType selectedItem) {
            playerFacade.turnTowards(baseLine);

            timedAnimatorFlag.setFlag (afterAction, AnimationParams.DO_ACTION, actionTime);
        }

        protected override void afterAction() {
            doPickupOfItem (sceneItem);
            base.afterAction ();
        }

        /// <summary>
        /// Adds item to player and removes scene item.
        /// </summary>
        protected abstract void doPickupOfItem (SceneItem sceneItem);
    }

}