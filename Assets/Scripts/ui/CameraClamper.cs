using net.gubbi.goofy.extensions;
using ui;
using UnityEngine;
using UnityEngine.UI;

namespace net.gubbi.goofy.ui {
    public class CameraClamper : HandheldEnable {

        public bool clampX = true;
        public bool clampY = true;
        
        private RectTransform rectTransform;
        private Rect borderRect;
        private Rect rect;
        private Rect clampedRect;
        private CanvasScaler canvasScaler;

        protected override void Awake() {
            base.Awake();
            rectTransform = GetComponent<RectTransform>();
            canvasScaler = GetComponentInParent<CanvasScaler>();
        }

        private void LateUpdate() {            
            rectTransform.anchoredPosition = clampToScreen(rectTransform.anchoredPosition, rectTransform);
        }
        
        private Vector2 clampToScreen (Vector3 pos, RectTransform rectTrans) {         
            float screenAspect = (float) Screen.width / (float) Screen.height;            
            float canvasHeight = canvasScaler.referenceResolution.y;                        
            float canvasWidth = canvasHeight * screenAspect * Camera.main.rect.width;                                                 
                                    
            borderRect = new Rect(0, 0, canvasWidth, canvasHeight);
            
            Rect localRect = rectTrans.rect;            
                            
            float height = localRect.height;
            float width = localRect.width;
            
            rect = new Rect(
                pos.x - rectTrans.pivot.x * width,
                pos.y - rectTrans.pivot.y * height,
                width,
                height
            );
            
            clampedRect = rect.clampInsideResize (borderRect);
            float x = clampX ? clampedRect.x : pos.x;
            float y = clampY ? clampedRect.y : pos.y;

            x += rectTrans.pivot.x * width;
            y += rectTrans.pivot.y * height;
            
            return new Vector2 (x, y);
        }        
 
        
    }
}