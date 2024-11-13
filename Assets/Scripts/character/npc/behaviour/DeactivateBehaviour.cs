using System;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class DeactivateBehaviour : CharacterBehaviour {

        protected override void doBehaviour (Action onCompleteBehaviour) {
            characterSceneHandler.setCharacterActive (false);
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            characterSceneHandler.setCharacterActiveState (false);
            return true;
        }

    }
}