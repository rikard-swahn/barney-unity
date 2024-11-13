using System;
using System.Collections;
using UnityEngine;

namespace net.gubbi.goofy.util {
    public class UnityUtil {
        
        public static IEnumerator endOfFrameCallback(MonoBehaviour mb, Action callback) {
            IEnumerator routine = waitEndOfFrameThenCallbackCoroutine(callback);
            mb.StartCoroutine (routine);
            return routine;
        }

        private static IEnumerator waitEndOfFrameThenCallbackCoroutine(Action callback) {
            yield return new WaitForEndOfFrame();
            callback ();
        }
        
    }
}