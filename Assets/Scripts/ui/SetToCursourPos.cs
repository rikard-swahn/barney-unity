using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class SetToCursourPos : HandheldEnable {

        public Vector2 cursorOffset;
        public bool offsetInInches;
        public Vector2 pivot;
        
        private RectTransform rectTransform;
        private CanvasScaler canvasScaler;

        protected override void Awake() {
            base.Awake();
            rectTransform = GetComponent<RectTransform>();
            canvasScaler = GetComponentInParent<CanvasScaler>();
        }        
        
        private void Update() {
            pivot = new Vector2(Mathf.Clamp01(pivot.x), Mathf.Clamp01(pivot.y));            
            Vector2 mPos = Input.mousePosition;            
            float screenAspect = (float) Screen.width / (float) Screen.height;
                         
            float canvasHeight = canvasScaler.referenceResolution.y;                        
            float canvasWidth = canvasHeight * screenAspect * Camera.main.rect.width;                                                 
                        
            float screenHeightInches = Screen.height / Screen.dpi;
            float canvasPixelsPerInch = canvasHeight / screenHeightInches;            
            Vector2 scaledCursorOffset = offsetInInches ? cursorOffset * canvasPixelsPerInch : cursorOffset;

            float cameraLeft = Screen.width * Camera.main.rect.xMin;
            float cameraWidth = Screen.width * Camera.main.rect.width;
            Vector2 canvasMPos = new Vector2(((mPos.x - cameraLeft) / cameraWidth) * canvasWidth, (mPos.y / Screen.height) * canvasHeight);

            rectTransform.anchoredPosition = new Vector2(
                canvasMPos.x + scaledCursorOffset.x - pivot.x * rectTransform.rect.width, 
                canvasMPos.y + scaledCursorOffset.y - pivot.y * rectTransform.rect.height
                );
        }
    }
}