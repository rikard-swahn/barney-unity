using System;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.scene.load;
using net.gubbi.goofy.scene.load.transition;
using UnityEngine;

namespace Assets.Scripts.character.npc.behaviour {
    public class ChangePlayerSceneBehaviour : CharacterBehaviour {

        public string goToScene;
        public Vector2 entryPosition;
        public Vector2 entryDirection;
        public Transitions.Type outTransition;
        public Transitions.Type inTransition;     
        public bool setCollisionDisabled;
        
        protected override void doBehaviour (Action onCompleteBehaviour) {
            if (!Vector2.zero.Equals(entryPosition)) {
                playerMove.setPositionState(new Vector2(entryPosition.x, entryPosition.y));
            }
            if (!Vector2.zero.Equals(entryDirection)) {
                playerMove.setDirectionState(new Vector2(entryDirection.x, entryDirection.y));
            }

            if (setCollisionDisabled) {
                playerMove.setCollisionDisabled(true);
            }

            onCompleteBehaviour();
            SceneLoader.Instance.changeInGameScene (goToScene, Transitions.get(outTransition), Transitions.get(inTransition));
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            return true;
        }

    }
}