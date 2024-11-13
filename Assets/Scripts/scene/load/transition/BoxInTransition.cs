using System;
using UnityEngine;

namespace net.gubbi.goofy.scene.load.transition {
    
    [CreateAssetMenu(menuName = "Transitions/In/Box")]
    public class BoxInTransition : SceneTransition {
        
        public Color color = Color.black;
        private Texture2D texture;
             
        protected override void doOnGUI() {
            GUI.color = color;

            float x = (Screen.width / 2f) * (1 - progress);
            float y = (Screen.height / 2f) * (1 - progress);
            
            GUI.DrawTexture(new Rect(0, 0, Screen.width, y), texture);
            GUI.DrawTexture(new Rect(0, 0, x, Screen.height), texture);
            GUI.DrawTexture(new Rect(0, Screen.height - y, Screen.width, y), texture);
            GUI.DrawTexture(new Rect(Screen.width - x, 0, x, Screen.height), texture);
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
            BoxInTransition transition = CreateInstance<BoxInTransition>();
            if (color != null) {
                transition.color = (Color)color;
            }            
            if (transitionTime != null) {
                transition.transitionTime = (float)transitionTime;
            }     
            
            return transition;
        }

        protected override void completeTransition() {
            stop();
            base.completeTransition();
        }        

    }
}