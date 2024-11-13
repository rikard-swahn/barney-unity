using System;
using net.gubbi.goofy.state;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class StateFlagBehavior : CharacterBehaviour {

        public string flag;

        protected override void doBehaviour (Action onCompleteBehaviour) {
            setFlag ();
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            setFlag ();
            return true;
        }

        private void setFlag ()	{
            GameState.Instance.StateData.setFlags (flag);
        }
    }
}