using System;
using System.Linq;
using net.gubbi.goofy.extensions;
using UnityEngine;
using Random = System.Random;

namespace net.gubbi.goofy.util.animation {
    public class RandomAnimationBoolParam : MonoBehaviour {

        public string param;
        public string animationTag;
        public Range startStateTime;
        public Range animStateTime;
        
        private Animator[] animators;
        private Random random = new Random();
        private float waitTime;        
        
        protected void Awake() {                                
            animators = GetComponentsInChildren<Animator>();            
        }

        private void OnDisable() {
            clearBoolParam();
        }

        private void Update() {
            if (!isStateWithTag(animationTag)) {
                clearBoolParam();                
                return;
            }

            bool flag = animators.First().GetBool(param);

            if (waitTime == 0f) {
                if (flag) {
                    waitTime = animStateTime.min + (float) random.NextDouble() * (animStateTime.max - animStateTime.min);                    
                }
                else {
                    waitTime = startStateTime.min + (float) random.NextDouble() * (startStateTime.max - startStateTime.min);
                } 
            }

            waitTime -= Time.deltaTime;
            waitTime = Math.Max(waitTime, 0);

            if (waitTime == 0f) {
                animators.ForEach(a => a.SetBool(param, !flag));
            }
        }

        private void clearBoolParam() {
            waitTime = 0f;
            animators.ForEach(a => a.SetBool(param, false));
        }

        private bool isStateWithTag(string tag) {
            return animators.Any(a => a.GetCurrentAnimatorStateInfo(0).IsTag(tag));
        } 
        
    }
}