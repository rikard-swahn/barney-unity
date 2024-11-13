using net.gubbi.goofy.extensions;
using net.gubbi.goofy.player;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.camera
{
    public class Camera2DFollow : MonoBehaviour
    {
        public Transform target;
        public float damping;
        public Rect visibleArea;
        public bool cropToVisibleArea;
        public bool lookAhead = true;
        public Vector3 offset;
        
        private float smallDamping;
        private float cameraZ;
        private Vector3 currentVelocity;
        private Vector3 lastPos;
        private SceneInputHandler sceneInputHandler;
        private Rect defaultCameraRect;
        private Camera cam;
        private LookAhead lookAheadService;

        private void Awake() {
            GameObject player = GameObject.Find(GameObjects.PLAYER);
            if (player != null) {
                sceneInputHandler = player.GetComponent<SceneInputHandler>();
            }

            if (target == null) {
                target = transform;
            }

            lookAheadService = GetComponent<LookAhead>();

            smallDamping = damping / 2f;
            transform.parent = null;
            cam = GetComponent<Camera> ();
            cameraZ = transform.position.z;
        }

        private void Start() {
            setCameraPos(
                getTargetPosition()
            );
        }

        private void LateUpdate() {
            setCameraPos(
                getSmoothFollowPos()
            );            
        }

        private void setCameraPos(Vector2 newCamPos) {
            float screenAspect = (float) Screen.width / (float) Screen.height;
            float cameraHeight = cam.orthographicSize * 2;
            float cameraWidth = cameraHeight * screenAspect;
            Rect cameraRect = new Rect(newCamPos.x - cameraWidth / 2, newCamPos.y - cameraHeight / 2, cameraWidth,
                cameraHeight);

            Rect clampedCamera = cameraRect.clampInsideResize(visibleArea);
            newCamPos = clampedCamera.center;
            transform.position = new Vector3(newCamPos.x, newCamPos.y, cameraZ + offset.z);

            if (cropToVisibleArea) {
                float w = clampedCamera.width / cameraRect.width;
                cam.rect = new Rect((1 - w) / 2, 0, w, 1);
            }
        }

        private Vector3 getSmoothFollowPos() {
            Vector3 targetPosWithLookOffset = getTargetPosition();

            if (lookAhead) {
                targetPosWithLookOffset += lookAheadService.getLookOffset();
            }

            if (sceneInputHandler == null) {
                return transform.position;
            }

            float followDamping = sceneInputHandler.isControlEnabled() ? damping : smallDamping;
            return Vector3.SmoothDamp(transform.position, targetPosWithLookOffset, ref currentVelocity, followDamping);            
        }

        private Vector3 getTargetPosition() {
            return target.position + offset;
        }


#if UNITY_EDITOR	
        void OnDrawGizmosSelected (){
            visibleArea.drawGizmo(Color.red);                    
        }
#endif   
        
    }
}