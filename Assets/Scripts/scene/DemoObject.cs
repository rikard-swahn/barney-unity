using UnityEngine;

namespace net.gubbi.goofy.scene {
    public class DemoObject : MonoBehaviour {
        
        private void Awake() {
#if !DEMO
            gameObject.SetActive(false);
            return;            
#endif
        }
        
    }
}