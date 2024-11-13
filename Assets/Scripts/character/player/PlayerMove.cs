using System;
using net.gubbi.goofy.character;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.state;
using net.gubbi.goofy.state.settings;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.player {
    public class PlayerMove : CharacterMove {
        
        protected override void Awake() {
            base.Awake ();
            
            setCollisionDisabled(GameState.Instance.StateData.PlayerState.DisableCollision);

            if (startAtScenePos) {
                setPosition(getPosition());
                setDirectionImmediate(initialDir);
            }
            else {
                initPos();
                initDir();
            }

            EventManager.Instance.addListener<GameEvents.SceneChangeInitEvent>(handleSceneChange);
        }

        protected override void Update() {
            base.Update();
            EventManager.Instance.raise(new GameEvents.PlayerRenderPositionEvent(getRenderPosition()));    
        }

        public override void setPositionState (Vector2 position) {
            GameState.Instance.StateData.setPlayerPosition(position);
        }

        public override void setDirectionState (Vector2 direction) {
            GameState.Instance.StateData.setPlayerDirection(direction);
        }

        public void setCollisionDisabled (bool disabled) {
            GameState.Instance.StateData.PlayerState.DisableCollision = disabled;
            polyNavAgent.disableCollision = disabled;
        }

        public override BodyState? getBodyState () {
            return null;
        }

        public override void setBodyState (BodyState state) {
            throw new NotImplementedException ();
        }

        public override void sit () {
            throw new NotImplementedException ();
        }

        public override void stand () {
            throw new NotImplementedException ();
        }

        protected override float getGameSpeedFactor() {
            int gameSpeed = SettingsState.Instance.StateData.GameSpeed;
            return 1 + gameSpeed / 1.75f;
        }

        private void initDir() {
            Vector2? direction = GameState.Instance.StateData.PlayerState.getDirection();
            if (direction != null) {
                setDirectionImmediate((Vector2) direction);
            }
            else {
                setDirectionImmediate(initialDir);
            }
        }

        private void initPos() {
            Vector3? position = GameState.Instance.StateData.PlayerState.getPosition();
            if (position != null) {
                setPosition((Vector3) position);
            }
        }

        private void handleSceneChange(GameEvents.SceneChangeInitEvent e) {
            stop();

            if (e.ImmediateChange) {
                animators.ForEach(a => a.speed = 0f);
            }
            else {
                animators.ForEach(a => a.SetFloat(AnimationParams.SPEED, 0f));
            }            
            
        } 
       
    }
}