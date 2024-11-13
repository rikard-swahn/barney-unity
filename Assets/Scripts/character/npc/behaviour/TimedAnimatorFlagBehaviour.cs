using System;
using System.Collections;
using net.gubbi.goofy.player;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class TimedAnimatorFlagBehaviour : CharacterBehaviour {

        public string flag;
        public float time = Constants.DEFAULT_ACTION_TIME;
        public Vector2 direction;
        public bool waitForAnimation = true;
        public GameObject target;

        private TimedAnimatorFlag timedAnimatorFlag;
        private IEnumerator animatorRoutine;

        protected override void Awake() {
            base.Awake();

            if (target == null) {
                target = gameObject;                
            }
            
            timedAnimatorFlag = target.GetComponentInParent<TimedAnimatorFlag>();
            characterFacade = target.GetComponentInParent<CharacterFacade> ();            
        }

        protected override void doBehaviour(Action onCompleteBehaviour) {
            setDirection();
            
            if (waitForAnimation) {
                animatorRoutine = timedAnimatorFlag.setFlag (onCompleteBehaviour, flag, time);
            }
            else {
                animatorRoutine = timedAnimatorFlag.setFlag (null, flag, time);
                onCompleteBehaviour();
            }            
            
        }

        public override bool stop () {
            bool stopped = base.stop();
            timedAnimatorFlag.stop(animatorRoutine, flag);
            return stopped;
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            setDirection();
            return true;
        }

        private void setDirection() {
            if (!Vector2.zero.Equals(direction)) {
                characterFacade.setDirectionImmediate(direction);
            }
        }
    }
}