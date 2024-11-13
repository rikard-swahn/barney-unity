using System;
using System.Collections;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.nav;
using net.gubbi.goofy.say;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;
using Random = System.Random;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class NavigateBehaviour : CharacterBehaviour {
        public GameObject waypointGroup;
        public string sayBlocked;
        public float blockedLineDelay = 2f;
        
        protected NavigateWaypoints navigateWaypoints;
        private Say say;
        private IEnumerator sayDelayedRoutine;
        
        private static readonly string NEXT_WP_PROPERTY_KEY = "NEXT_WAYPOINT";
        private static readonly Random random = new Random();
        private float BLOCKED_SAY_DELAY_MIN = 2f;
        private float BLOCKED_SAY_DELAY_MAX = 5f;
        private bool blocked;


        private new void Awake() {
            base.Awake ();
            navigateWaypoints = GetComponentInParent<NavigateWaypoints> ();
            say = GameObject.Find(GameObjects.SPEECH_BUBBLE_CONTAINER).GetComponent<Say>();
        }

        private void Update() {
            if (started && blocked && sayDelayedRoutine == null && !sayBlocked.isNullOrEmpty()) {
                //Start say subroutine, which says after delay
                float delay = BLOCKED_SAY_DELAY_MIN + (float) random.NextDouble() * (BLOCKED_SAY_DELAY_MAX - BLOCKED_SAY_DELAY_MIN);
                sayDelayedRoutine = sayAfterDelay(delay);
                StartCoroutine(sayDelayedRoutine);
            }
        }

        protected override void doBehaviour (Action onCompleteBehaviour) {
            characterMove.start ();
            int? wpIndex = getStartingWaypoint ();
            navigateWaypoints.start (waypointGroup, wpIndex, onCompleteBehaviour, onMove, onNextWaypoint, onBlocked);
        }

        public override bool stop () {
            bool stopped = base.stop();
            navigateWaypoints.stop ();
            characterMove.stopAndUpdateState ();
            stopBlockSay();
            return stopped;
        }

        public override void freezeUI () {
            base.freezeUI();
            navigateWaypoints.stop ();
            characterMove.stop ();
            stopBlockSay();
        }

        private int? getStartingWaypoint () {
            if (GameState.Instance.StateData.hasCharacterBehaviourProperty (characterKey, NEXT_WP_PROPERTY_KEY)) {
                int wpIndex = GameState.Instance.StateData.getCharacterBehaviourState(characterKey, NEXT_WP_PROPERTY_KEY).getIntval();
                return wpIndex;
            }

            return null;
        }
						
        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            Vector2? lastPos = GameState.Instance.StateData.getCharacterPosition (characterKey);

            navigateWaypoints.moveToEnd (waypointGroup, onMove, onNextWaypoint, lastPos, endingCurrentBehavior);
            return true;
        }
			
        private void onMove (Vector3 pos, Vector2? direction) {
            characterMove.setPositionState (pos);

            if (direction != null) {
                characterMove.setDirectionState ((Vector2)direction);
            }
        }

        private void onNextWaypoint(int wpIndex) {
            int nextWpIndex = wpIndex + 1;
            GameState.Instance.StateData.setCharacterBehaviourState (characterKey, NEXT_WP_PROPERTY_KEY, nextWpIndex);
        }

        private void onBlocked(bool blocked) {
            this.blocked = blocked;
            if (!blocked) {
                stopBlockSay();
            }
        }

        private void stopBlockSay() {
            if (sayDelayedRoutine != null) {
                StopCoroutine(sayDelayedRoutine);
                sayDelayedRoutine = null;

                if (say.Sayer == characterGo) {
                    say.stopTalking(false);
                }
            }
        }

        private IEnumerator sayAfterDelay(float waitSecs) {
            yield return new WaitForSeconds(waitSecs);
            string option = I18nUtil.randomOption(sayBlocked);
            say.sayBackgroundRaw (characterGo, option, sayComplete, blockedLineDelay, true, sayComplete);
        }

        private void sayComplete() {
            sayDelayedRoutine = null;
        }
    }
}