using System;
using net.gubbi.goofy.util;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class StandBehavior : CharacterBehaviour {

        protected override void doBehaviour (Action onCompleteBehaviour) {
            characterMove.stand();
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            characterMove.setBodyState (BodyState.STANDING);
            return true;
        }


    }
}