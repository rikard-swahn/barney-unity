using net.gubbi.goofy.events;
using UnityEngine;

namespace net.gubbi.goofy.scene.parallax {
    public class Parallax : MonoBehaviour {
        
        public float SpeedFactor = 1f;
        public bool horizontal;
        public bool vertical;
        public bool absolutePosition;
        
        private Vector2 mask;
        private Vector2 startPos;

        private void Awake() {        
            EventManager.Instance.addListener<GameEvents.CameraMovementEvent>(onCameraMovement);
            mask = new Vector2(horizontal ? 1 : 0, vertical ? 1 : 0);      
            startPos = transform.position;
        }

        private void onCameraMovement(GameEvents.CameraMovementEvent e) {
            if (absolutePosition) {
                transform.position = startPos - Vector2.Scale(e.AbsoluteMovement, mask) * SpeedFactor;;
            }
            else {
                transform.Translate(-Vector3.Scale(e.Movement, mask) * SpeedFactor);
            }
        }

    }
}