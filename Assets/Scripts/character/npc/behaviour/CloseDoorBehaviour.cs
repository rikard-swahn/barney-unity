using System;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.scene.door;
using UnityEngine;

namespace character.npc.behaviour {
    public class CloseDoorBehaviour : CharacterBehaviour {

        public GameObject doorGameObj;
        private Door door;

        private new void Awake() {
            base.Awake ();
            this.door = doorGameObj.GetComponent<Door> ();
        }

        protected override void doBehaviour (Action onCompleteBehaviour) {
            door.setOpen(false);
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            door.setOpenState(false);
            return true;
        }

    }
}