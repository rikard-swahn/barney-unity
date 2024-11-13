using net.gubbi.goofy.scene;
using UnityEngine;

namespace net.gubbi.goofy.util {
    public class YAxisScaler : MonoBehaviour {
                
        public float minScale = 0f;
        public float scaleWhenBelowMin = 0f;
        public float scale = 1f;
        
        private Room room;

        private void Awake() {
            room = GameObject.FindWithTag (Tags.ROOM).GetComponent<Room>();
        }

        private void Update() {
            float newScaleFloat = scale * getDepthScale();

            float xSign = Mathf.Sign(transform.localScale.x);
            Vector3 newScale = new Vector3(xSign * newScaleFloat, newScaleFloat, transform.localScale.z);
            transform.localScale = newScale;
        }

        //Calculate scaling based on y pos (object is further away if higher y)
        private float getDepthScale () {
            float worldHeight = Camera.main.orthographicSize * 2;

            float k = -room.yScaling;
            float m = room.scaleAtReferenceY -k * room.scaleReferenceY / worldHeight;
            float screenRelativeY = transform.position.y / worldHeight;
            float scale = k * screenRelativeY + m;

            if (scale < minScale) {
                scale = scaleWhenBelowMin;
            }
            
            return scale;
        }        
    }
}