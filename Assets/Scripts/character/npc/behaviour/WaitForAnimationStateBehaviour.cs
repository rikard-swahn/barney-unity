﻿using System;
using System.Collections;
using System.Linq;
using net.gubbi.goofy.character.npc.behaviour;
using UnityEngine;

namespace Assets.Scripts.character.npc.behaviour {
    public class WaitForAnimationStateBehaviour : CharacterBehaviour {

        public string animationName;
        public GameObject target;

        private Animator[] targetAnimators;
        private IEnumerator waitRoutine;

        private new void Awake() {
            base.Awake ();
            targetAnimators = target.GetComponentsInChildren<Animator>();
        }

        protected override void doBehaviour (Action onCompleteBehaviour) {
            waitRoutine = doWaitForAnimation(onCompleteBehaviour);
            StartCoroutine(waitRoutine);            
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
            
            if (onCompleteAnimation != null) {
                onCompleteAnimation();
            }
        }
    }
}