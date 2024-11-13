using System;
using System.Collections;
using System.Linq;
using net.gubbi.goofy.item;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {
    public class WaitForAnimationAction : SingleSceneItemAction {
        
        public string animationName;
        public bool waitForCompletion = true;
        public bool waitForOtherAnimation;
        public Animator[] targetAnimators;

        protected override void Awake() {
            base.Awake ();

            if (targetAnimators == null || targetAnimators.Length == 0) {
                targetAnimators = target.GetComponentsInChildren<Animator>();
            }
        } 

        protected override void doAction(ItemType selectedItem) {
            StartCoroutine(doWaitForAnimation(afterAction));            
        }

        private IEnumerator doWaitForAnimation (Action onCompleteAnimation) {
            if (waitForOtherAnimation) {
                while (targetAnimators.Any(a => a.GetCurrentAnimatorStateInfo(0).IsTag(animationName))) {               
                    yield return null;
                }                
            }
            
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