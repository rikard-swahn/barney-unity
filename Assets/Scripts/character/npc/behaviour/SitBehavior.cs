using System;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class SitBehavior : CharacterBehaviour {

        protected override void doBehaviour (Action onCompleteBehaviour) {
            characterMove.sit ();
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            characterMove.setBodyState (BodyState.SITTING);
            GameState.Instance.StateData.setCharacterAnimationBoolParams(characterKey, AnimationParams.SIT_COMPLETE);
            return true;
        }

    }
}