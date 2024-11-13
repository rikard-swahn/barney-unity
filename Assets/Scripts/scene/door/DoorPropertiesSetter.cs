using UnityEngine;

namespace net.gubbi.goofy.scene.door {
    public class DoorPropertiesSetter : MonoBehaviour {
        
        private Door door;
        
        private void Awake() {
            door = GetComponentInParent<Door>();
        }

        public void setOpenComplete() {
            door.setOpenComplete(true);
        }
        
        public void clearOpenComplete() {
            door.setOpenComplete(false);
        }
        
    }
}