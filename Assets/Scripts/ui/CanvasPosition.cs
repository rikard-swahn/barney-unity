using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui {
    
    public class CanvasPosition : MonoBehaviour {
                
        private RectTransform rectTransform;
        private CanvasScaler canvasScaler;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            canvasScaler = GetComponentInParent<CanvasScaler>();
        }

        public void setCanvasPosFromWorldPos(Vector2 worldPos) {            
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            
            float screenAspect = (float) Screen.width / (float) Screen.height;
                         
            float canvasHeight = canvasScaler.referenceResolution.y;                        
            float canvasWidth = canvasHeight * screenAspect * Camera.main.rect.width;                                                 
                        
            float cameraLeft = Screen.width * Camera.main.rect.xMin;
            float cameraWidth = Screen.width * Camera.main.rect.width;
            rectTransform.anchoredPosition = new Vector2(((screenPos.x - cameraLeft) / cameraWidth) * canvasWidth, (screenPos.y / Screen.height) * canvasHeight);
        }        
    }
}