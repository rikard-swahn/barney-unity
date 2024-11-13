using net.gubbi.goofy.events;
using UnityEngine;

namespace net.gubbi.goofy.camera {
    //TODO: Change this to a general MovementPublisher?
    public class CameraMovementPublisher : MonoBehaviour {

        private Vector2 lastPos;

        private void Update() {
            Vector2 movement = (Vector2)transform.position - lastPos;
            Vector3 absoluteMovement = transform.position;
            
            EventManager.Instance.raise(new GameEvents.CameraMovementEvent(absoluteMovement, movement));
            lastPos = transform.position;            
        }

    }
}