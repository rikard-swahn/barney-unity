using System;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class SetSceneBehaviour : CharacterBehaviour {
        public string scene;

        protected override void doBehaviour (Action onCompleteBehaviour) {
            characterSceneHandler.setCharacterScene (scene);
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            characterSceneHandler.setCharacterScene (scene);
            return true;
        }

    }
}