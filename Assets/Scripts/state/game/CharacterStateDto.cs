using System.Collections.Generic;
using net.gubbi.goofy.state.game;
using net.gubbi.goofy.util;
using state.game;
using UnityEngine;

namespace net.gubbi.goofy.state {

    public class CharacterStateDto {

        public Vector2Dto? position;
        public Vector2Dto? direction;
        public string scene;
        public BehaviourStateDto behavior;
        public bool? active;
        public bool? visible;
        public string bodyState;
        public AnimatorStateDto animatorState;

        public CharacterStateDto() { }

        public CharacterStateDto (CharacterState characterState) {
            scene = characterState.Scene;
            behavior = characterState.Behavior != null ? new BehaviourStateDto (characterState.Behavior) : null;

            Vector2? pos = characterState.getPosition ();
            position = pos != null ? new Vector2Dto(((Vector2)pos).x, ((Vector2)pos).y) : (Vector2Dto?)null;

            Vector2? dir = characterState.getDirection ();
            direction = dir != null ? new Vector2Dto(((Vector2)dir).x, ((Vector2)dir).y) : (Vector2Dto?)null;

            active = characterState.Active;
            visible = characterState.Visible;
            bodyState = characterState.BodyState;
            animatorState = new AnimatorStateDto(characterState.AnimatorState);
        }

        public class BehaviourStateDto {
            public BehaviourGroupType? behaviourGroupType;
            public int? index;
            public string state;
            public Dictionary<string, Property> properties;

            public BehaviourStateDto() { }

            public BehaviourStateDto (CharacterState.BehaviourState state) {
                behaviourGroupType = state.BehaviourGroupType;
                index = state.Index;
                this.state = state.State != null ? state.State.ToString() : null;
                properties = state.Properties;
            }			
        }

    }
}