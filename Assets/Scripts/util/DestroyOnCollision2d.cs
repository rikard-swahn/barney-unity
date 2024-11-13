using UnityEngine;

namespace net.gubbi.goofy.util {
    public class DestroyOnCollision2d : MonoBehaviour {

        public string Tag;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(Tag)) {                
                Destroy(other.gameObject);
            }            
        }        

    }
}