using System;
using System.Collections;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.say;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class SayBehaviour : CharacterBehaviour {

        public string text;
        public bool passive;
        public bool turnPlayer;
        public bool turnCharacter;
        public float sayDelay;
        public Vector2 direction;
        public bool waitForCompletion = true;
        public float sayDelayInitial;

        private Say say;
        private IEnumerator saySilentRoutine;
        private Action onCompleteBehaviour;
        private Animator[] animators;

        private new void Awake() {
            base.Awake ();
            say = GameObject.Find(GameObjects.SPEECH_BUBBLE_CONTAINER).GetComponent<Say>();
            animators = characterGo.GetComponentsInChildren<Animator>();
        }

        protected override void doBehaviour (Action onCompleteBehaviour) {
            this.onCompleteBehaviour = onCompleteBehaviour;
            
            if (turnPlayer) {
                playerFacade.stopAndTurnTowards (gameObject.transform.position);
            }
            if (turnCharacter) {
                characterFacade.stopAndTurnTowards (playerMove.getPosition ());
            }            

            setDirection();

            Action onComplete = waitForCompletion ? onCompleteBehaviour : null;
            
            if (passive) {                
                this.delayedAction(
                    () =>  characterFacade.sayBackground (text,onComplete,sayDelay,true,sayAbortedCallback),				
                    sayDelayInitial
                );                
            } else {                
                this.delayedAction(
                    () =>  characterFacade.sayText (text, onComplete),				
                    sayDelayInitial                    
                );                
            }

            if (!waitForCompletion) {
                onCompleteBehaviour();
            }
        }

        private void sayAbortedCallback() {
            float saySilentTime = text.split(I18nUtil.LINE_DELIM).Length * sayDelay;            
            saySilentRoutine = saySilent(saySilentTime);
            StartCoroutine(saySilentRoutine);
        }

        private IEnumerator saySilent(float time) {
            setAnimatorTalking(true);
            yield return new WaitForSeconds(time);
            setAnimatorTalking(false);
            onCompleteBehaviour();
        }
        
        private void setAnimatorTalking (bool talking) {
            if (animators != null) {
                animators.ForEach(a => a.SetBool(AnimationParams.TALKING, talking));
            }
        }        

        public override bool stop () {
            bool stopped = base.stop();
            
            if (saySilentRoutine != null) {
                setAnimatorTalking(false);
                StopCoroutine(saySilentRoutine);
                saySilentRoutine = null;
            }
            else {
                say.stopTalking(false);
            }

            return stopped;
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            setDirection();
            return true;
        }

        private void setDirection() {
            if (!Vector2.zero.Equals(direction)) {
                characterFacade.setDirectionImmediate(direction);
            }
        }

    }
}