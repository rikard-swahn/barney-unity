using UnityEngine;

namespace net.gubbi.goofy.util {
    public class Rigidbody2dVelocity : MonoBehaviour {

        public Vector2 StartVelocity;
        public bool relativeToScale;

        private Rigidbody2D _rigidBody;
                        
        private void Awake() {
            _rigidBody = GetComponent<Rigidbody2D>();
            _rigidBody.velocity = StartVelocity;            
        }

        private void Update() {
            if (relativeToScale) {
                _rigidBody.velocity = StartVelocity * transform.localScale.x;
            }
        }
    }
}