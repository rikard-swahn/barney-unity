using UnityEngine;

namespace net.gubbi.goofy.scene.slope {
    
    [CreateAssetMenu(menuName = "Slope")]
    public class SlopeData : ScriptableObject {        
        public float leftHeight;                
        public float rightHeight;
    }
}