using System;
using System.Collections;
using System.Linq;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using UnityEngine;

namespace Assets.Scripts.character.npc.behaviour {
    public class SetAnimatorTriggerAction : SingleSceneItemAction {

        public string animationName;
        public bool waitForCompletion;

        private Animator[] targetAnimators;

        private new void Awake() {
            base.Awake ();
            targetAnimators = target.GetComponentsInChildren<Animator>();
        }

        protected override void doAction (ItemType selectedItem) {            
            targetAnimators.ForEach(a => a.SetTrigger(animationName));

            if (waitForCompletion) {
                StartCoroutine(doWaitForAnimation(afterAction)); 
            }
            else {
                afterAction();
            }
        }
        private IEnumerator doWaitForAnimation (Action onCompleteAnimation) {                                    
            while (!targetAnimators.Any(a => a.GetCurrentAnimatorStateInfo(0).IsTag(animationName))) {
                yield return null;
            }

            if(waitForCompletion) {
                while (targetAnimators.Any(a => a.GetCurrentAnimatorStateInfo(0).IsTag(animationName)
                                                && !a.IsInTransition(0)
                                                && a.GetCurrentAnimatorStateInfo(0).normalizedTime < 1
                )) {
                    yield return null;
                }                
            }

            if (onCompleteAnimation != null) {
                onCompleteAnimation();
            }
        }        

    }
}