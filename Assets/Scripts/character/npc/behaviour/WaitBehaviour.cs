using System;
using System.Collections;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class WaitBehaviour : CharacterBehaviour {

        public float waitTime = 1.0f;
        private IEnumerator waitRoutine;
        public Vector2 direction;

        protected override void doBehaviour (Action onCompleteBehaviour) {
            setDirection();
            this.waitRoutine = wait (onCompleteBehaviour);
            StartCoroutine(this.waitRoutine);
        }

        public override bool stop () {
            bool stopped = base.stop();
            if (waitRoutine != null) {				
                StopCoroutine (waitRoutine);
            }
            return stopped;
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            setDirection();
            return true;
        }

        private IEnumerator wait (Action afterWait) {
            yield return new WaitForSeconds(waitTime);
            afterWait ();
        }

        private void setDirection() {
            if (!Vector2.zero.Equals(direction)) {
                characterFacade.setDirectionImmediate(direction);
            }
        }
    }
}