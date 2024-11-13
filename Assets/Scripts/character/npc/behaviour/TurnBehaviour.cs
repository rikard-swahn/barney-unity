using System;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class TurnBehaviour : CharacterBehaviour {

        public Vector2 direction;

        protected override void doBehaviour (Action onCompleteBehaviour) {
            characterMove.setDirectionImmediate (direction);
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            characterMove.setDirectionState (direction);
            return true;
        }
			
    }
}