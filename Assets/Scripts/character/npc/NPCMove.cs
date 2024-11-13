using System.Collections.Generic;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc {
    public class NPCMove : CharacterMove {

        public BodyState defaultBodyState = BodyState.STANDING;

        private BodyState bodyState;
        private string characterKey;

        protected override void Awake() {
            base.Awake ();
            characterKey = gameObject.name;            
        }

        public override void init() {

            if (startAtScenePos) {
                setPosition(getPosition());
                setDirectionImmediate(initialDir);                
            }
            else {
                initPos ();
                initDir ();                
            }

            initAnimatorParams();
            initBodyState ();
        }

        public override void setPositionState (Vector2 position) {
            GameState.Instance.StateData.setCharacterPosition (characterKey, position);
        }

        public override void setDirectionState (Vector2 direction) {
            GameState.Instance.StateData.setCharacterDirection (characterKey, direction);
        }

        public override BodyState? getBodyState () {
            return bodyState;
        }

        public override void setBodyState (BodyState bodyState) {
            this.bodyState = bodyState;
            GameState.Instance.StateData.setCharacterBodyState (characterKey, bodyState.getKey());
        }
			
        public override void sit () {
            setBodyState (BodyState.SITTING);
            animators.ForEach(a => a.SetBool (AnimationParams.SITTING, true));
        }
			
        public override void stand () {
            setBodyState (BodyState.STANDING);
            animators.ForEach(a => a.SetBool (AnimationParams.SITTING, false));
        }

        private void initPos () {
            Vector3? position = GameState.Instance.StateData.getCharacterPosition (characterKey);

            if (position != null) {
                setPosition ((Vector3)position);
            }
        }

        private void initDir () {
            Vector2? direction = GameState.Instance.StateData.getCharacterDirection (characterKey);

            if(direction != null) {
                setDirectionImmediate ((Vector2)direction);
            } else {
                setDirectionImmediate (initialDir);
            }				
        }	

        private void initBodyState () {
            BodyState? bodyState = getBodyStateState ();
            this.bodyState = bodyState ?? defaultBodyState;

            if (this.bodyState == BodyState.SITTING) {
                sit ();
            }
        }
        
        private void initAnimatorParams () {
            HashSet<string> boolParams = GameState.Instance.StateData.getCharacterAnimationBoolParams(characterKey);

            if (boolParams != null) {
                boolParams.ForEach(p => animators.ForEach(a => a.SetBool(p, true)));
            }
        }
        
        private BodyState? getBodyStateState () {
            string stateStr = GameState.Instance.StateData.getCharacterBodyState (characterKey);
            if (stateStr == null) {
                return null;
            }

            return BodyStates.fromKey (stateStr);
        }

    }
}