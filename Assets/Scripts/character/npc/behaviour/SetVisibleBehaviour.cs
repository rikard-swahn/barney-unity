using System;
using net.gubbi.goofy.character.npc.behaviour;

namespace Assets.Scripts.character.npc.behaviour {
    public class SetVisibleBehaviour : CharacterBehaviour {

        public bool visible;

        protected override void doBehaviour (Action onCompleteBehaviour) {
            characterSceneHandler.setCharacterVisible(visible);
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            characterSceneHandler.setCharacterVisibleState(visible);
            return true;
        }

    }
}