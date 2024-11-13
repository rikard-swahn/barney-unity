using System;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.util;

namespace Assets.Scripts.character.npc.behaviour {
    public class StartBehaviourBehaviour : CharacterBehaviour {

        public CharacterBehaviours behaviours;
        public BehaviourGroupType behaviour;

        protected override void doBehaviour(Action onCompleteBehaviour) {
            onCompleteBehaviour();
            behaviours.start(behaviour);
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            return false;
        }
    }
}