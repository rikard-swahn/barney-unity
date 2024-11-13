using System;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.state;

namespace Assets.Scripts.character.npc.behaviour {
    public class ClearStateFlagsBehaviour : CharacterBehaviour {

        public string[] flags;

        protected override void doBehaviour (Action onCompleteBehaviour) {
            clearFlags ();
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            clearFlags ();
            return true;
        }

        private void clearFlags ()	{
            GameState.Instance.StateData.removeFlags(flags);
        }

    }
}