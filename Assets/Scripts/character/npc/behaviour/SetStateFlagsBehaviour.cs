using System;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.state;

namespace Assets.Scripts.character.npc.behaviour {
    public class SetStateFlagsBehaviour : CharacterBehaviour {

        public string[] flags;

        protected override void doBehaviour (Action onCompleteBehaviour) {
            setFlags ();
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            setFlags ();
            return true;
        }

        private void setFlags ()	{
            GameState.Instance.StateData.setFlags(flags);
        }

    }
}