using System.Collections;
using System.Linq;
using net.gubbi.goofy.character;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.player;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action.use {

    public class UseAction : SingleSceneItemAction {

        public GameObject conditionsCharacter;
        public bool waitForAnimation = true;
        public float waitAfterAnimation;
        public bool removeItem;
        public bool removeSceneItem;
        public string[] setFlags;
        public bool turnTowardsBaseLine = true;
        public float actionTime = Constants.DEFAULT_ACTION_TIME;
        public bool mirrorOnVertical;

        private CharacterFacade playerFacade;
        private TimedAnimatorFlag timedAnimatorFlag;
        private Inventory inventory;
        private Vector3 baseLine;
        private CharacterMove playerMove;

        protected override void Awake () {
            base.Awake ();
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerFacade = playerGo.GetComponent<CharacterFacade>();
            timedAnimatorFlag = playerGo.GetComponent<TimedAnimatorFlag>();
            playerMove = playerGo.GetComponent<CharacterMove>();

            GameObject inventoryGo = GameObject.Find(GameObjects.INVENTORY);
            inventory = inventoryGo.GetComponent<Inventory>();
            
            if (conditions.All(c => !c.contains<ItemSelectedFilter>())) {
                ItemSelectedFilter nothingSelected = ScriptableObject.CreateInstance<ItemSelectedFilter>();
                nothingSelected.item = ItemType.EMPTY;
                conditions.Add(nothingSelected);                
            }
            
            baseLine = target.transform.Find(GameObjects.ORDERED_RENDERING + "/" + GameObjects.BASE_LINE).position;
        }

        public override FilterContext getFilterContext(ItemType selectedItem) {
            FilterContext ctx = base.getFilterContext(selectedItem);

            if (conditionsCharacter != null) {
                return ctx.mergeProperties(
                    FilterContext.builder()
                        .property(FilterContext.CHARACTER, conditionsCharacter)
                        .build()
                );
            }

            return ctx;
        }

        protected override void doAction (ItemType selectedItem) {
            if (turnTowardsBaseLine) {
                playerFacade.turnTowards(baseLine);
            }

            playerMove.MirrorOnVertical = mirrorOnVertical;
            
            if (waitForAnimation) {
                timedAnimatorFlag.setFlag (() => { afterActionWait(selectedItem); }, AnimationParams.DO_ACTION, actionTime);    
            }
            else {
                timedAnimatorFlag.setFlag (delegate { playerMove.MirrorOnVertical = false; }, AnimationParams.DO_ACTION, actionTime);
                afterActionWait(selectedItem);
            }
            
        }

        private void afterActionWait(ItemType selectedItem) {
            playerMove.MirrorOnVertical = false;
            
            if (waitAfterAnimation > 0f) {
                StartCoroutine(wait(selectedItem));
            } else {
                afterAction (selectedItem);
            }
        }

        private IEnumerator wait (ItemType selectedItem) {
            yield return new WaitForSeconds(waitAfterAnimation);
            afterAction (selectedItem);
        }

        protected virtual void afterAction(ItemType selectedItem) {
            if (selectedItem != ItemType.EMPTY && removeItem) {
                inventory.removeItem (selectedItem);
            }

            if (removeSceneItem) {
                SceneItemUtil.setItemActive (target.name, false);
            }

            foreach (string flag in setFlags) {
                GameState.Instance.StateData.setFlags(flag);
            }

            afterAction ();
        }


    }

}