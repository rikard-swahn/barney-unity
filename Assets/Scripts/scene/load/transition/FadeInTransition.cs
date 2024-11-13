using System;
using UnityEngine;

namespace net.gubbi.goofy.scene.load.transition {
    
    [CreateAssetMenu(menuName = "Transitions/In/Fade")]
    public class FadeInTransition : SceneTransition {
        
        public Color color = Color.black;
        private Texture2D texture;

        protected override void doOnGUI() {
            color.a = (1f - progress);
            GUI.color = color;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        }
        
        public override bool start(Action onComplete) {                        
            if (!base.start(onComplete)) {
                return false;
            }            
            
            if (texture == null) {
                texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
            }
            
            return true;
        }        
        
        public static SceneTransition create(Color? color = null, float? transitionTime = null) {
            FadeInTransition transition = CreateInstance<FadeInTransition>();
            if (color != null) {
                transition.color = (Color)color;
            }
            if (transitionTime != null) {
                transition.transitionTime = (float)transitionTime;
            }              
            
            return transition;
        }     

        protected override void completeTransition() {
            base.completeTransition();
            stop();
        }

    }
}