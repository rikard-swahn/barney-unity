using UnityEngine;

namespace net.gubbi.goofy.scene.load.transition {
    
    [CreateAssetMenu(menuName = "Transitions/Out/Circle")]
    public class CircleOutTransition : CircleTransition {
        
        protected override float getScale(float progress) {
            return 1 - progress;
        }
    }
}