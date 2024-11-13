using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;
using Range = net.gubbi.goofy.util.Range;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class RandomWaitBehaviour : CharacterBehaviour {
        
        public Range waitTime;
        
        private IEnumerator waitRoutine;
        private Random random = new();
        
        protected override void doBehaviour (Action onCompleteBehaviour) {
            float time = waitTime.min + (float) random.NextDouble() * (waitTime.max - waitTime.min);
            waitRoutine = wait(time, onCompleteBehaviour);
            StartCoroutine(waitRoutine);
        }

        public override bool stop () {
            bool stopped = base.stop();
            if (waitRoutine != null) {				
                StopCoroutine (waitRoutine);
            }
            return stopped;
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            return true;
        }

        private IEnumerator wait (float time, Action afterWait) {
            yield return new WaitForSeconds(time);
            afterWait ();
        }


    }
}