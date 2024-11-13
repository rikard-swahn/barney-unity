using System;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.state;

namespace Assets.Scripts.character.npc.behaviour {
    public class SetSceneItemFlagsBehaviour : CharacterBehaviour {
        public string itemKey;
        public string[] setItemFlags;

        protected override void doBehaviour (Action onCompleteBehaviour) {
            setFlags ();
            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            setFlags ();
            return true;
        }

        private void setFlags ()	{
            foreach(string flag in setItemFlags) {
                GameState.Instance.StateData.setSceneProperty (itemKey, flag, true);
            }
        }
    }
}