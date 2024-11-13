using ui;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui {
    public class MouseTextRectClamper : HandheldEnable {
        
        public float xFlipOffset;
        
        private RectTransform rectTransform;
        private CanvasScaler canvasScaler;

        protected override void Awake() {
            base.Awake();
            rectTransform = GetComponent<RectTransform>();
            canvasScaler = GetComponentInParent<CanvasScaler>();
        }

        private void LateUpdate() {
            float rectWidth = rectTransform.rect.width;
            float scaledXFlipOffset = xFlipOffset;            
            
            float screenAspect = (float) Screen.width / (float) Screen.height;            
            float canvasHeight = canvasScaler.referenceResolution.y;
            float canvasWidth = canvasHeight * screenAspect * Camera.main.rect.width;
            
            if (rectTransform.anchoredPosition.x + rectWidth > canvasWidth) {
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + scaledXFlipOffset - rectWidth, rectTransform.anchoredPosition.y);
            }
        }

    }
}