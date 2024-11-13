using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene {
    public class RenderOffset : MonoBehaviour {

        public Vector3 offset;
        
        private Vector3 defaultRenderPos;
        private Vector3 defaultRenderBaselinePos;
        private Vector2 offsetAreaLeft = Vector2.zero;
        private Vector2 offsetAreaRight = Vector2.zero;
        private Transform baselineTrans;
        
        protected virtual void Awake() {
            defaultRenderPos = transform.localPosition;

            baselineTrans = gameObject.findChildWithTag(Tags.BASE_LINE).transform;
            defaultRenderBaselinePos = baselineTrans.localPosition;
        }

        private void Update() {
            Vector3 areaOffset = getAreaRenderOffset();
            transform.localPosition = defaultRenderPos + areaOffset + offset;
            baselineTrans.localPosition = defaultRenderBaselinePos - areaOffset;            
        }

        public void setRenderOffsetArea(Vector2 offsetAreaLeft, Vector2 offsetAreaRight) {
            this.offsetAreaLeft = offsetAreaLeft;
            this.offsetAreaRight = offsetAreaRight;
        }

        public void clearRenderOffsetArea() {
            offsetAreaLeft = Vector2.zero;
            offsetAreaRight = Vector2.zero;
        }        
        
        private Vector2 getAreaRenderOffset() {
            if (offsetAreaLeft == Vector2.zero && offsetAreaRight == Vector2.zero) {
                return Vector2.zero;
            }

            float x = transform.position.x;
            float t = (Mathf.Clamp(x, offsetAreaLeft.x, offsetAreaRight.x) - offsetAreaLeft.x) / (offsetAreaRight.x - offsetAreaLeft.x);
            float renderYOffset = Mathf.Lerp(offsetAreaLeft.y, offsetAreaRight.y, t);
                
            return new Vector2(0, renderYOffset);
        }        
    }
}