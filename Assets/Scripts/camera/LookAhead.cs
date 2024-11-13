using System;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.player;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.camera {
    public class LookAhead : MonoBehaviour {

        public float lookAheadFactor = 1;
        
        //The width of the look ahead edge around the screen. If cursor is outside this edge, camera looks ahead.
        //The width is a fraction of the screen height. 
        public float lookAheadEdgeWidth = 0.1f;

        private SceneInputHandler sceneInputHandler;

        private void Awake() {
            sceneInputHandler = GameObject.Find(GameObjects.PLAYER).GetComponent<SceneInputHandler>();            
        }

        private void Update() {
            screenAspect = (float) Screen.width / (float) Screen.height;
        }

        public Vector3 getLookOffset () {
            if (!sceneInputHandler.isControlEnabled () || !MouseUtil.isHovering() || RaycastUtil.mouseRaycastHitsTag(Tags.MENU_AREA)) {				
                return Vector3.zero;
            }            
            
            float lookAheadEdgeWidthPixels = lookAheadEdgeWidth * Screen.height;
            
            float halfWidth = Screen.width * 0.5f;
            float halfHeight = Screen.height * 0.5f;

            //Mouse coordinates, centered on screen.
            float mx = Input.mousePosition.x - halfWidth;
            float my = Input.mousePosition.y - halfHeight;
            
            float lookAheadX = Mathf.Abs(mx) >= halfWidth - lookAheadEdgeWidthPixels ? Camera.main.orthographicSize * Mathf.Sign(mx) : 0f;
            float lookAheadY = Mathf.Abs(my) >= halfHeight - lookAheadEdgeWidthPixels ? Camera.main.orthographicSize * Mathf.Sign(my) : 0f;

            if (lookAheadX == 0f && lookAheadY == 0f) {				
                return Vector3.zero;
            }
            
            return lookAheadFactor * new Vector3(lookAheadX, lookAheadY, 0);
        }

        private float screenAspect;
        
        
#if UNITY_EDITOR	
        void OnDrawGizmosSelected (){
            float edge = 2 * Camera.main.orthographicSize * lookAheadEdgeWidth;
            float h = 2 * (Camera.main.orthographicSize - edge);
            float w = 2 * (Camera.main.orthographicSize * screenAspect - edge);
            Rect lookAheadBorder = new Rect(transform.position.x - w / 2, transform.position.y -h / 2, w, h);
            lookAheadBorder.drawGizmo(Color.green);
        }
#endif           
        
    }
}