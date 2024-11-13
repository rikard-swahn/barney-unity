using UnityEngine;

namespace net.gubbi.goofy.scene.load.transition {
    
    [CreateAssetMenu(menuName = "Transitions/In/Circle")]
    public class CircleInTransition : CircleTransition {
        
        protected override float getScale(float progress) {
            return progress;
        }            
        protected override void completeTransition() {
            stop();
            base.completeTransition();
        }        

    }
}