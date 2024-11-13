using System;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class SetOtherCharacterActivateBehaviour : CharacterBehaviour {

        public CharacterSceneHandler otherCharacterSceneHandler;
        public bool active;

        protected override void doBehaviour (Action onCompleteBehaviour) {
            otherCharacterSceneHandler.setCharacterActive(active);
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            otherCharacterSceneHandler.setCharacterActiveState(active);                                    
            return true;
        }

    }
}