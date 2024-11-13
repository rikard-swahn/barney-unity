using System;
using UnityEngine;

namespace net.gubbi.goofy.scene.load.transition {

    [CreateAssetMenu(menuName = "Transitions/Out/Fade")]
    public class FadeOutTransition : SceneTransition {
        
        public Color color = Color.black;
        private Texture2D texture;

        protected override void doOnGUI() {
            color.a = progress;
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
            FadeOutTransition transition = CreateInstance<FadeOutTransition>();
            if (color != null) {
                transition.color = (Color)color;
            }
            if (transitionTime != null) {
                transition.transitionTime = (float)transitionTime;
            }               
            
            return transition;
        }      
    }
}