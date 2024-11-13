using System;
using net.gubbi.goofy.say;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class PlayerSayBehaviour : CharacterBehaviour {
        
        public bool sayTowardsCamera;
        public bool stopAndTurnCharacter = true;
        public string text;
        
        private Say say;

        private new void Awake() {
            base.Awake ();
            say = GameObject.Find(GameObjects.SPEECH_BUBBLE_CONTAINER).GetComponent<Say>();
        }

        protected override void doBehaviour (Action onCompleteBehaviour) {
            if (stopAndTurnCharacter) {
                characterFacade.stopAndTurnTowards (playerMove.getPosition ());
            }

            playerFacade.navStop();

            if (sayTowardsCamera) {
                playerFacade.turnTowardsCamera ();
            }

            playerFacade.sayText (
                text, 
                onCompleteBehaviour
            );
        }

        public override bool stop () {
            bool stopped = base.stop();
            say.stopTalking (false);
            return stopped;
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            return true;
        }

    }
}