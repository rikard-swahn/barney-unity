using System;
using System.Text.RegularExpressions;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using ui.text;
using UnityEngine;
using UnityEngine.UI;

namespace VikingCrewTools {
    public class SpeechbubbleBehaviour : MonoBehaviour {
        
        public Image image;
        public Text text;        
        public int maxLineWidth = 25;
        public RectTransform arrow;
        public float cornerWidth;

        private Transform objectToFollow;
        private static readonly float EQUAL_THRESHOLD_SCALAR = 0.0001f;        
        private float bubbleScale;
        private Vector2 arrowDefaultPos;
        private Vector3 clampedPos;
        private string textString;
        private Rect borderRect;
        private Rect rect;
        private Rect clampedRect;
        private readonly string SPACE = " ";
        private readonly string NEWLINE = "\n";
        private RectTransform rectTransform;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            arrowDefaultPos = arrow.anchoredPosition;
        }

        private void LateUpdate() {
            bubbleScale = rectTransform.lossyScale.y;
            transform.rotation = Camera.main.transform.rotation;            
            breakLinesAutoAdjust();

            transform.position = clampedPos;
            float xDiff = (clampedPos - objectToFollow.position).x / bubbleScale;
            
            xDiff = Mathf.Min(Math.Abs(xDiff), image.rectTransform.rect.width/2 - cornerWidth) * Math.Sign(xDiff);
            arrow.anchoredPosition = new Vector2(arrowDefaultPos.x - xDiff, arrowDefaultPos.y);
        }

        public void Setup(Transform objectToFollow, Vector3 offset, string text, Color color) {
            this.objectToFollow = objectToFollow;
            image.color = color;
            gameObject.SetActive(true);
            this.textString = text;
            LayoutRebuilder.ForceRebuildLayoutImmediate(image.rectTransform);
        }                

        public void deactivate() {
            gameObject.SetActive(false);
        }

        /**
         * If bubble is forced down due to being over top of camera, the bubble
         * is made smaller vertically by allowing wider lines.
         */
        private void breakLinesAutoAdjust() {
            int maxLineWidth = this.maxLineWidth;

            //TODO: These numbers (10, 10), are tuned for my needs, not general.
            int lineWidthIncrease = 10;
            
            for (int i = 0; i <= 10; i++) {
                int curMaxLineWidth = maxLineWidth + i * lineWidthIncrease;
                text.text = LineBreakService.Instance.breakString(textString, curMaxLineWidth);
                reClampBubble();
                
                float clampDelta = clampedPos.y - objectToFollow.position.y;
                float bubbleWidth = image.rectTransform.rect.width * bubbleScale;
                
                if (bubbleWidth > getCameraWidth()) {
                    if (i > 0) {
                        text.text = LineBreakService.Instance.breakString(textString, maxLineWidth + (i - 1) * lineWidthIncrease);
                        reClampBubble();
                    }
                    
                    return;
                }                

                if (clampDelta.looseEquals(0f, EQUAL_THRESHOLD_SCALAR)
                    || clampDelta > 0) {
                    return;
                }
            }
        }

        private void reClampBubble() {
            LayoutRebuilder.ForceRebuildLayoutImmediate(image.rectTransform);
            clampedPos = clampToScreen(objectToFollow.position, image);
        }

        private Vector3 clampToScreen (Vector3 pos, Image image) {
            var cameraHeight = getCameraHeight();
            var cameraWidth = getCameraWidth();

            RectTransform imageRectTrans = image.rectTransform;
            Vector3 camPos = Camera.main.transform.position;
            
            borderRect = new Rect(camPos.x - cameraWidth / 2, camPos.y - cameraHeight / 2, cameraWidth, cameraHeight);
            
            Rect imageLocalRect = imageRectTrans.rect;            
                            
            float bubbleHeight = imageLocalRect.height * bubbleScale;
            float bubbleWidth = imageLocalRect.width * bubbleScale;
            
            rect = new Rect(
                pos.x - bubbleWidth / 2,
                pos.y,
                bubbleWidth,
                bubbleHeight
            );

            clampedRect = rect.clampInsideResize (borderRect);
            Vector3 newScreenPos = new Vector3 (clampedRect.x + imageRectTrans.pivot.x * bubbleWidth, clampedRect.y + imageRectTrans.pivot.y * bubbleHeight, 1);
                       
            return newScreenPos;            
        }

        private static float getCameraHeight() {
            return Camera.main.orthographicSize * 2;            
        }

        private static float getCameraWidth() {                        
            float screenAspect = (float) Screen.width / (float) Screen.height;
            float cameraWidth = getCameraHeight() * screenAspect * Camera.main.rect.width;
            return cameraWidth;
        }

#if UNITY_EDITOR	
        void OnDrawGizmosSelected () {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(objectToFollow.position, new Vector3(0.1f, 0.1f, 0.1f));
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(clampedPos, new Vector3(0.1f, 0.1f, 0.1f));
            
            borderRect.drawGizmo(Color.red);                        
            rect.drawGizmo(Color.green);                        
            clampedRect.drawGizmo(Color.yellow);                                    
        }
#endif           
    }

}