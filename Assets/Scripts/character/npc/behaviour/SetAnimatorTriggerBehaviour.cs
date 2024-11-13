using System;
using System.Collections;
using System.Linq;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.extensions;
using UnityEngine;

namespace Assets.Scripts.character.npc.behaviour {
    public class SetAnimatorTriggerBehaviour : CharacterBehaviour {

        public string animationName;
        public GameObject target;
        public bool waitForAnimation;

        private Animator[] targetAnimators;
        private IEnumerator waitRoutine;

        private new void Awake() {
            base.Awake ();
            targetAnimators = target.GetComponentsInChildren<Animator>();
        }

        protected override void doBehaviour (Action onCompleteBehaviour) {            
            targetAnimators.ForEach(a => a.SetTrigger(animationName));

            if (waitForAnimation) {
                waitRoutine = doWaitForAnimation(onCompleteBehaviour);
                StartCoroutine(waitRoutine);
            }
            else {
                waitRoutine = doWaitForAnimation(null);
                StartCoroutine(waitRoutine);
                onCompleteBehaviour();
            }
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            return true;
        }

        public override bool stop () {
            bool stopped = base.stop();
            if (waitRoutine != null) {
                StopCoroutine(waitRoutine);
                waitRoutine = null;
            }
            return stopped;
        }

        private IEnumerator doWaitForAnimation (Action onCompleteAnimation) {
            while (!targetAnimators.Any(a => a.GetCurrentAnimatorStateInfo(0).IsTag(animationName))) {
                yield return null;
            }  
            
            while (targetAnimators.Any(a => a.GetCurrentAnimatorStateInfo(0).IsTag(animationName)
                                            && !a.IsInTransition(0)
                                            && a.GetCurrentAnimatorStateInfo(0).normalizedTime < 1
            )) {
                yield return null;
            }    

            if (onCompleteAnimation != null) {
                onCompleteAnimation();
            }
        }
    }
}