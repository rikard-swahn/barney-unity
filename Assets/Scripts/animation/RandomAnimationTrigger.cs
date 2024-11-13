using System;
using System.Linq;
using net.gubbi.goofy.extensions;
using UnityEngine;
using Random = System.Random;

namespace net.gubbi.goofy.util.animation {
    public class RandomAnimationTrigger : MonoBehaviour {

        public string param;
        public string animationTag;
        public Range startStateTime;
        
        private Animator[] animators;
        private Random random = new Random();
        private float waitTime;        
        
        protected void Awake() {                                
            animators = GetComponentsInChildren<Animator>();            
        }            
        
        private void Update() {
            if (!isStateWithTag(animationTag)) {
                waitTime = 0f;
                return;
            }

            if (waitTime == 0f) {
                waitTime = startStateTime.min + (float) random.NextDouble() * (startStateTime.max - startStateTime.min);
            }

            waitTime -= Time.deltaTime;
            waitTime = Math.Max(waitTime, 0);

            if (waitTime == 0f) {                                
                animators.ForEach(a => a.SetTrigger(param));
            }
        }

        private bool isStateWithTag(string tag) {
            return animators.Any(a => a.GetCurrentAnimatorStateInfo(0).IsTag(tag));
        } 
        
    }
}