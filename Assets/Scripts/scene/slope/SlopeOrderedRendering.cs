using System;
using UnityEngine;

namespace net.gubbi.goofy.scene.slope {
    public class SlopeOrderedRendering : MonoBehaviour {
        public String triggerForTag;
        public SlopeData slopeData;

        private BoxCollider2D boxCollider;

        private void Awake() {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            //If tag matches and the action to trigger is not already running
            if (other.CompareTag (triggerForTag)) {
                RenderOffset renderOffset = other.GetComponentInChildren<RenderOffset>();
                Vector2 left = new Vector3(transform.position.x + boxCollider.offset.x - boxCollider.size.x / 2, slopeData.leftHeight);
                Vector2 right = new Vector3(transform.position.x + boxCollider.offset.x + boxCollider.size.x / 2, slopeData.rightHeight);
                
                renderOffset.setRenderOffsetArea(left, right);
            }            
        }

        private void OnTriggerExit2D(Collider2D other) {
            //If tag matches and the action to trigger is not already running
            if (other.CompareTag (triggerForTag)) {
                RenderOffset renderOffset = other.GetComponentInChildren<RenderOffset>();
                renderOffset.clearRenderOffsetArea();
            }              
        }              
      
    }
}