using System;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class TeleportBehaviour : CharacterBehaviour {

        public GameObject destination;

        protected override void doBehaviour (Action onCompleteBehaviour) {
            characterMove.setPosition (destination.transform.position);
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            characterMove.setPositionState (destination.transform.position);
            return true;
        }

    }
}