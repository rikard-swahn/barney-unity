using net.gubbi.goofy.extensions;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui  {
    public class RectTransScreenSpaceCamClamp : MonoBehaviour {
        
        private CanvasScaler canvasScaler;

        private void Awake() {
            canvasScaler = GetComponentInParent<CanvasScaler>();

            RectTransform rt = GetComponent<RectTransform>();
            rt.anchoredPosition = clampToScreen(rt);
        }        
        
        private Vector3 clampToScreen (RectTransform rectTrans) {
            float screenAspect = (float) Screen.width / (float) Screen.height;            
            float canvasHeight = canvasScaler.referenceResolution.y;
            float canvasWidth = canvasHeight * screenAspect * Camera.main.rect.width;
            Rect cameraRect = new Rect(-canvasWidth / 2, -canvasHeight / 2, canvasWidth, canvasHeight);
            
            float width = rectTrans.rect.width;
            float height = rectTrans.rect.height;

            float px = rectTrans.pivot.x;
            float py = rectTrans.pivot.y;
            
            Rect rect = new Rect(
                rectTrans.anchoredPosition.x - px * width,
                rectTrans.anchoredPosition.y - py * height,
                width,
                height
            );

            Rect clampedRect = rect.clampInsideResize (cameraRect);
            Vector3 newScreenPos = new Vector3 (clampedRect.x + px * width, clampedRect.y + py * height, 0);
                       
            return newScreenPos;            
        }        
    }
}