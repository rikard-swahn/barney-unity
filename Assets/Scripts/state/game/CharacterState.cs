using System;
using System.Collections.Generic;
using net.gubbi.goofy.character.npc.behaviour.state;
using net.gubbi.goofy.state.game;
using net.gubbi.goofy.util;
using state.game;
using UnityEngine;

namespace net.gubbi.goofy.state {

    public class CharacterState  {
        public string Scene {get; set;}
        public BehaviourState Behavior {get; set;}
        public bool? Active {get; set;}
        public bool? Visible {get; set;}
        public string BodyState {get; set;}
        public AnimatorState AnimatorState {get; set;}
        private Vector2? _position;
        private Vector2? _direction;

        public CharacterState() {
            AnimatorState = new AnimatorState();
        }

        public CharacterState (CharacterStateDto dto) {
            Scene = dto.scene;
            Behavior = dto.behavior != null ? new BehaviourState (dto.behavior) : null;

            if (dto.position != null) {
                setPosition(new Vector2(((Vector2Dto)dto.position).x, ((Vector2Dto)dto.position).y));
            }
            if (dto.direction != null) {
                setDirection(new Vector2(((Vector2Dto)dto.direction).x, ((Vector2Dto)dto.direction).y));
            }
            Active = dto.active;
            Visible = dto.visible;
            BodyState = dto.bodyState;
            AnimatorState = new AnimatorState(dto.animatorState);
        }
			
        public void setPosition (Vector3 position) {
            this._position = new Vector2 (position.x, position.y);
        }

        public Vector2? getPosition() {
            if (_position != null) {
                Vector2 posNotNull = (Vector2)_position;
                return new Vector2 (posNotNull.x, posNotNull.y);
            }

            return null;
        }

        public void setDirection (Vector2 direction) {
            this._direction = direction;
        }

        public Vector2? getDirection () {
            if (_direction != null) {
                Vector2 dirNotNull = (Vector2)_direction;
                return new Vector2 (dirNotNull.x, dirNotNull.y);
            }

            return null;
        }			
			
        public override string ToString () {
            return string.Format ("[CharacterState: position={0}, Scene={1}, BehaviourGroupType={2}]", _position, Scene, this.Behavior);
        }

        public class BehaviourState {
            public int? Index {get; set;}
            public BehaviourGroupType? BehaviourGroupType {get; set;}
            public BehaviourRunStateType? State {get; set;} 

            public Dictionary<string, Property> Properties {get; set;}

            public BehaviourState(CharacterStateDto.BehaviourStateDto dto) {
                BehaviourGroupType = dto.behaviourGroupType;
                Index = dto.index;
                State = dto.state != null ? (BehaviourRunStateType)Enum.Parse(typeof(BehaviourRunStateType), dto.state) : (BehaviourRunStateType?)null;
                Properties = dto.properties;	
            }

            public BehaviourState () {				
                Properties = new Dictionary<string, Property> ();
            }			
				
            public override string ToString () {
                return string.Format ("[BehaviourState: Index={0}, Properties={1}]", Index, Properties.toDebugString());
            }			
        }
    }


}