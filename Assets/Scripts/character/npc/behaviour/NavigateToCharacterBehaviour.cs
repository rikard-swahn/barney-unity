using System;
using System.Collections.Generic;
using net.gubbi.goofy.nav;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class NavigateToCharacterBehaviour : CharacterBehaviour {
        public GameObject character;

        private NavigateWaypoints navigateWaypoints;

        protected override void Awake() {
            base.Awake ();
            navigateWaypoints = GetComponentInParent<NavigateWaypoints> ();
        }

        protected override void doBehaviour (Action onCompleteBehaviour) {
            characterMove.start ();
            int? wpIndex = 0;
            IList<Vector2> path = getPathToCharacter(character);
            navigateWaypoints.start (path, wpIndex, onCompleteBehaviour, onMove);
        }

        public override bool stop () {
            bool stopped = base.stop();
            navigateWaypoints.stop ();
            characterMove.stopAndUpdateState ();
            return stopped;
        }

        public override void freezeUI () {
            base.freezeUI();
            navigateWaypoints.stop ();
            characterMove.stop ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            Vector2? lastPos = GameState.Instance.StateData.getCharacterPosition (characterKey);
            IList<Vector2> path = getPathToCharacter(character);
            navigateWaypoints.moveToEnd (path, onMove, null, lastPos, endingCurrentBehavior);
            return true;
        }

        private void onMove (Vector3 pos, Vector2? direction) {
            characterMove.setPositionState (pos);

            if (direction != null) {
                characterMove.setDirectionState ((Vector2)direction);
            }
        }

        private IList<Vector2> getPathToCharacter(GameObject character) {
            Vector2 visitPos = character.transform.Find(GameObjects.VISIT_POSITION).position;

            IList<Vector2> path = new List<Vector2>();
            path.Add(visitPos);
            return path;
        }
    }
}