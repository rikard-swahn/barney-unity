﻿using UnityEngine;

namespace net.gubbi.goofy.state {
	
    public class PlayerState {
        public string SceneActionTargetName { get; set;}
        public Vector2? SceneTarget { get; set;}
        public int? ItemActionIndex { get; set;}
        public int? ItemActionGroupIndex { get; set;}
        public int MapPosition { get; set;}
        public Vector2? LastWaypoint { get; set;}
        public Vector2? NextWaypoint { get; set;}
        public bool DisableCollision {get; set;}
        
        private Vector2? position;
        private Vector2? direction;

        public PlayerState () {}

        public PlayerState (PlayerStateDto dto) {
            MapPosition = dto.mapPosition;
            position = Vector2Dto.toVector2(dto.position);
            direction = Vector2Dto.toVector2(dto.direction);
            SceneActionTargetName = dto.sceneActionTargetName;
            ItemActionIndex = dto.itemActionIndex;
            ItemActionGroupIndex = dto.itemActionGroupIndex;
            SceneTarget = Vector2Dto.toVector2(dto.sceneTarget);
            LastWaypoint = Vector2Dto.toVector2(dto.lastWaypoint);
            NextWaypoint = Vector2Dto.toVector2(dto.nextWaypoint);
            DisableCollision = dto.disableCollision;
        }

        public override string ToString () {
            return string.Format ("[PlayerState: position={0}, direction={1}]", position, direction);
        }

        public void setPosition (Vector3 position) {
            this.position = new Vector2 (position.x, position.y);
        }

        public void setPosition (Vector2 position) {
            this.position = new Vector2 (position.x, position.y);
        }

        public void clearPosition () {
            this.position = null;
        }

        public Vector3? getPosition() {
            if (position != null) {
                Vector2 posNotNull = (Vector2)position;
                return new Vector3 (posNotNull.x, posNotNull.y, 0f);
            } else {
                return null;
            }
        }

        public void setDirection (Vector2 direction) {
            this.direction = direction;
        }

        public Vector2? getDirection () {
            if (direction != null) {
                Vector2 dirNotNull = (Vector2)direction;
                return new Vector2 (dirNotNull.x, dirNotNull.y);
            } else {
                return null;
            }
        }

        public void clearActionAndTarget() {
            SceneTarget = null;
            SceneActionTargetName = null;
            GameState.Instance.StateData.PlayerState.ItemActionIndex = null;
            ItemActionGroupIndex = null;
        }
    }
}