using System;
using net.gubbi.goofy.scene.door;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class EnterDoorBehaviour : CharacterBehaviour {

        public GameObject doorGameObj;
        
        private Door door;
        private Vector2 goTo;

        private new void Awake() {
            base.Awake ();
            door = doorGameObj.GetComponent<Door> ();
            goTo = door.insideWaypoint.gameObject.transform.position;
        }

        protected override void doBehaviour (Action onCompleteBehaviour) {
            Action<bool> onComplete = delegate {
                door.setOpen(false);
                onCompleteBehaviour();
            };
            characterMove.SetDestination (goTo, onComplete);
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            door.setOpen(false);
            return true;
        }

    }
}