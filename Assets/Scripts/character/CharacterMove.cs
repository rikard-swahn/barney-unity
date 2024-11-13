using System;
using net.gubbi.goofy.events;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using PolyNav;
using unity;
using UnityEngine;

namespace net.gubbi.goofy.character {
    public abstract class CharacterMove : InitiableMonoBehaviour
    {
        public float normalSpeed; // The fastest the character can travel
        public float animationSpeed = 1f;
        public Vector2 initialDir;
        public bool startAtScenePos;
        public bool walkBackwards;
        public float speedFactor = 1f;
        public bool staticDirection;

        private bool mirrorOnVertical;
        public bool MirrorOnVertical {
            private get { return mirrorOnVertical; }
            set {
                mirrorOnVertical = value;

                if (!lastDirectionIsHorizontal()) {
                    setcharacterHorizontalDirection(mirrorOnVertical ? -1 : 1);
                }
            }
        }

        protected Animator[] animators;
        protected PolyNavAgent polyNavAgent;

        private Vector2 lastDirection = Vector2.zero;
        private bool stopped;
        private float currentTotalScale;
        private Transform renderingTrans;
        
        protected virtual void Awake() {
            polyNavAgent = GetComponentInChildren<PolyNavAgent>();
            animators = GetComponentsInChildren<Animator>();
            renderingTrans = gameObject.findChildWithTag(Tags.ORDERED_RENDERING).transform;
        }
        
        private void OnEnable() {
            EventManager.Instance.addListener<GameEvents.NavAgentUpdateEvent>(saveGameHandler);
        }

        private void OnDisable() {
            EventManager.Instance.removeListener<GameEvents.NavAgentUpdateEvent>(saveGameHandler);
        }            
        
        private void saveGameHandler(GameEvents.NavAgentUpdateEvent e) {
            //Update the character state to get a refreshed state from PolyNavAgent
            updateState();
        }        

        protected virtual void Update () {
            if (stopped) {
                return;
            }

            updateState ();
        }

        public void setPosition (Vector3 pos) {
            transform.position = pos;
            setPositionState (getPosition());
        }

        public void setDirectionImmediate(Vector2 direction) {
            setDirection (direction);
        }

        private void setDirection(Vector2 direction) {
            if (staticDirection) {
                return;
            }
            
            float dx = direction.x;
            float dy = direction.y;

            if (dx == 0 && dy == 0) {
                return;
            }

            lastDirection.Set(dx, dy);
            setDirectionState (lastDirection);

            if (lastDirectionIsHorizontal()) {
                animators.ForEach(a => a.SetInteger (AnimationParams.DIRECTION, 1));

                int backWardsFactor = walkBackwards ? -1 : 1;
                setcharacterHorizontalDirection (backWardsFactor * Math.Sign(dx));                
            } else {
                //If character has direction north or south, set dir x (right), to have the x scale as 1 as default
                setcharacterHorizontalDirection(mirrorOnVertical ? -1 : 1);
                
                if (!walkBackwards && dy < 0 || walkBackwards && dy >= 0) {
                    animators.ForEach(a => a.SetInteger (AnimationParams.DIRECTION, 2));
                }
                else {
                    animators.ForEach(a => a.SetInteger (AnimationParams.DIRECTION, 0));
                }                
            }
            
        }

        private bool lastDirectionIsHorizontal() {
            return Math.Abs(lastDirection.x) >= Math.Abs(lastDirection.y);
        }

        public void stop() {
            stopped = true;
            polyNavAgent.Stop ();
        }

        public void stopAndUpdateState() {
            stop ();
            updateState ();
        }

        public void start() {
            stopped = false;
        }

        public abstract void setPositionState (Vector2 position);
        public abstract void setDirectionState (Vector2 direction);

        private void setcharacterHorizontalDirection(int newXDir) {
            Vector3 newScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (Math.Sign(transform.localScale.x) != newXDir) {
                newScale.x *= -1;    
            }

            transform.localScale = newScale;            
        }

        private void updateState () {
            var totalSpeed = normalSpeed * speedFactor * getGameSpeedFactor();
            
            polyNavAgent.maxSpeed = Math.Abs(transform.localScale.x) * totalSpeed;
            setDirection (polyNavAgent.movingDirection);

            float speed = (polyNavAgent.currentSpeed / polyNavAgent.maxSpeed) * animationSpeed * totalSpeed;
            animators.ForEach(a => a.SetFloat(AnimationParams.SPEED, speed));

            setPositionState (getPosition ());
            setDirectionState (lastDirection);
        }
        
        public Vector2 LastDirection {
            get {
                return new Vector2(lastDirection.x, lastDirection.y);
            }
        }

        protected Vector3 getRenderPosition() {
            return renderingTrans.position;
        }

        public Vector3 getPosition () {
            return gameObject.transform.position;
        }
			
        public abstract BodyState? getBodyState ();
        public abstract void setBodyState (BodyState state);
        public abstract void sit ();
        public abstract void stand ();

        public void SetDestination(Vector2 dest, Action<bool> finishedCallback = null) {
            polyNavAgent.SetDestination(dest, finishedCallback);
        }

        public void abortNavigation() {
            polyNavAgent.Abort();
        }

        public bool hasNavigationFinishCallback() {
            return polyNavAgent.HasFinishCallback();
        }

        public float getSpeed() {
            return polyNavAgent.currentSpeed;
        }

        protected virtual float getGameSpeedFactor() {
            return 1;
        }
    }
}