using System;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class TurnTowardsBehaviour : CharacterBehaviour {

        public GameObject target;

        protected override void doBehaviour (Action onCompleteBehaviour) {
            characterFacade.turnTowards (target.transform.position);
            onCompleteBehaviour();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            characterFacade.turnTowardsState (target.transform.position);
            return true;
        }

    }
}