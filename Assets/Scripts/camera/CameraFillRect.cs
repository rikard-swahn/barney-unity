using UnityEngine;

namespace net.gubbi.goofy.camera {
    public class CameraFillRect : MonoBehaviour {

        public new Camera camera;
        private RectTransform rt;
        
        private void Awake() { 
            rt = GetComponent<RectTransform> ();
        }

        private void Update() {
            float screenAspect = (float) Screen.width / (float) Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            float cameraWidth = cameraHeight * screenAspect;
            
            rt.sizeDelta = new Vector2(cameraWidth, cameraHeight);            
        }
    }
}