using System;
using net.gubbi.goofy.scene;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class PlayerGoToBehaviour : CharacterBehaviour {

        public Waypoint waypoint;
        private Vector2 goTo;

        protected override void Awake() {
            base.Awake();
            goTo = waypoint.gameObject.transform.position;
        }

        public override bool stop () {
            bool stopped = base.stop();
            playerMove.stop();
            return stopped;
        }

        protected override void doBehaviour (Action onCompleteBehaviour) {            
            Action<bool> onCompleteWrapper = delegate { onCompleteBehaviour(); };
            playerMove.SetDestination (goTo, onCompleteWrapper);
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            return true;
        }

    }
}