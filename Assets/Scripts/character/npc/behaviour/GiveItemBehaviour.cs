using System;
using System.Collections;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.player;
using net.gubbi.goofy.util;
using UnityEngine;

namespace Assets.Scripts.character.npc.behaviour {
    public class GiveItemBehaviour : CharacterBehaviour {

        public ItemType item;
        public bool playerGive;

        private Inventory inventory;
        private TimedAnimatorFlag playerTimedAnimatorFlag;
        private TimedAnimatorFlag characterTimedAnimatorFlag;
        private IEnumerator playerAnimatorRoutine;
        private IEnumerator characterAnimatorRoutine;

        protected override void Awake() {
            base.Awake();

            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerTimedAnimatorFlag = playerGo.GetComponent<TimedAnimatorFlag>();

            characterTimedAnimatorFlag = GetComponentInParent<TimedAnimatorFlag>();

            GameObject inventoryGo = GameObject.Find(GameObjects.INVENTORY);
            inventory = inventoryGo.GetComponent<Inventory>();
        }


        protected override void doBehaviour (Action onCompleteBehaviour) {
            Action onCompleteWrapper = delegate {
                giveItem();
                onCompleteBehaviour();
            };
            playerAnimatorRoutine = playerTimedAnimatorFlag.setFlag (onCompleteWrapper, AnimationParams.DO_ACTION, Constants.DEFAULT_ACTION_TIME);
            characterAnimatorRoutine = characterTimedAnimatorFlag.setFlag (null, AnimationParams.DO_ACTION, Constants.DEFAULT_ACTION_TIME);
        }

        public override bool stop () {
            bool stopped = base.stop();
            characterTimedAnimatorFlag.stop(playerAnimatorRoutine, AnimationParams.DO_ACTION);
            playerTimedAnimatorFlag.stop(characterAnimatorRoutine, AnimationParams.DO_ACTION);
            return stopped;
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            giveItem();
            return true;
        }

        private void giveItem ()	{
            if (playerGive) {
                inventory.removeItem(item);
            }
            else {
                inventory.addItem(item);
            }
        }

    }
}