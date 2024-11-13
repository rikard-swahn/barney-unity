using System;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class PlayerTurnBehaviour : CharacterBehaviour {

        public Vector2 direction;

        protected override void doBehaviour (Action onCompleteBehaviour) {            
            playerMove.setDirectionImmediate(direction);
            onCompleteBehaviour ();                                        
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            playerMove.setDirectionImmediate(direction);
            return true;
        }

    }
}