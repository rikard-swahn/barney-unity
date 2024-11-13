using System;
using System.Collections;
using net.gubbi.goofy.extensions;
using UnityEngine;

namespace net.gubbi.goofy.player {
    public class TimedAnimatorFlag : MonoBehaviour {
        private Animator[] animators;

        private void Awake() {
            animators = GetComponentsInChildren<Animator>();
        }

        public IEnumerator setFlag(Action callback, string paramName, float time) {
            animators.ForEach(a => a.SetBool(paramName, true));
            IEnumerator waitRoutine = wait(callback, paramName, time);
            StartCoroutine(waitRoutine);
            return waitRoutine;
        }

        public void stop(IEnumerator routine, string paramName) {
            if (routine != null) {
                StopCoroutine(routine);
            }
            clearFlag(paramName);
        }

        IEnumerator wait (Action callback, string paramName, float time) {
            yield return new WaitForSeconds(time);
            clearFlag(paramName);
            if (callback != null) {
                callback ();
            }
        }

        private void clearFlag(string paramName) {            
            animators.ForEach(a => a.SetBool(paramName, false));
        }
    }
}