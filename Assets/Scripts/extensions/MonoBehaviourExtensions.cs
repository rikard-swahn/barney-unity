using System;
using System.Collections;
using UnityEngine;

namespace net.gubbi.goofy.extensions {
    public static class MonoBehaviourExtensions {
        
        public static void delayedAction(this MonoBehaviour behaviour, Action action, float delay) {
            if (delay == 0f) {
                action();
                return;
            }
            
            behaviour.StartCoroutine(wait(action, delay));            
        }

        private static IEnumerator wait(Action action, float delay) {
            yield return new WaitForSeconds(delay);
            action ();
        }                     
    }
}