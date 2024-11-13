using UnityEngine;

namespace ui {
    public class ScreenSpaceMainCameraAssigner : MonoBehaviour {

        public string sortingLayerName;
        public int orderInLayer;
        
        private Canvas canvas;

        private void Awake() {
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = sortingLayerName;
            canvas.sortingOrder = orderInLayer;
        }
    }
}