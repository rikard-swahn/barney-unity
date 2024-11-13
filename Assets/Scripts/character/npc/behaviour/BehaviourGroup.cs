using System;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.character.npc.behaviour {
    public class BehaviourGroup : MonoBehaviour {
        public CharacterBehaviour[] behaviours;
        public BehaviourGroupType behaviourGroupType;
        public bool stopOnLoop;
        public bool isAutoBehaviour;
        public bool isSubBehaviour;

        public Filter condition;
        public int CurrentBehaviour { get; private set;}
        
        private string characterKey;
        private static readonly int START_BEHAVIOUR = 0;
        private CharacterBehaviours characterBehaviours;
        private Action<bool> onCompletedGroup;

        private void Awake () {	
            GameObject character = gameObject.getAncestorWithTag (Tags.CHARACTER);
            characterKey = character.name;
            characterBehaviours = character.GetComponentInChildren<CharacterBehaviours> ();

            if (behaviours.Length == 0) {
                behaviours = GetComponents<CharacterBehaviour>();
            }            
        }

        public void init () {						
            setCurrentBehaviour(getCurrentBehaviourStartValue ());
        }

        public void restart() {			
            setCurrentBehaviour (START_BEHAVIOUR);
        }

        public void stop() {
            while (!behaviours[CurrentBehaviour].stop()) {                
            }
        }

        public void freezeUI() {		
            behaviours [CurrentBehaviour].freezeUI();
        }

        public void endOfBehaviour () {
            if (!isAutoBehaviour && !isSubBehaviour) {
                int currentBehaviorBeforeEnding = CurrentBehaviour;

                //Put every remaining behaviour to the end state
                for (int behaviour = CurrentBehaviour; behaviour < behaviours.Length; behaviour++) {										
                    setCurrentBehaviour (behaviour);
                    bool endingCurrentBehavior = (behaviour == currentBehaviorBeforeEnding);

                    if (!behaviours [behaviour].endOfBehaviour (endingCurrentBehavior)) {
                        break;
                    }
                }
            }				
        }

        public void startBehaviour(Action<bool> onCompletedGroup) {
            this.onCompletedGroup = onCompletedGroup;
            doBehaviour ();
        }

        public void resumeBehaviourBackground() {
            behaviours[CurrentBehaviour].resumeBehaviourBackground();
        }

        public void resumeBehaviour(Action<bool> onCompletedGroup) {
            this.onCompletedGroup = onCompletedGroup;
            resumeBehaviour ();
        }
        
        public CharacterBehaviour getCurrentBehaviour() {
            return behaviours[CurrentBehaviour];
        }

        private void doBehaviour() {			
            if (!characterBehaviours.isStarted()) {
                return;
            }				

            if (!behaviours [CurrentBehaviour].start (onCompletedBehaviour)) {
                stop ();
            }
        }        
        private void resumeBehaviour() {			
            if (!characterBehaviours.isStarted()) {
                return;
            }				

            if (!behaviours [CurrentBehaviour].resume (onCompletedBehaviour)) {
                stop ();
            }
        }        
			
        private void onCompletedBehaviour() {
            if (CurrentBehaviour < behaviours.Length - 1) {				
                setCurrentBehaviour (CurrentBehaviour + 1);
                doBehaviour ();
            } else {
                onCompletedGroup (stopOnLoop);
            }
        }

        private void setCurrentBehaviour (int behaviour) {			
            CurrentBehaviour = behaviour;
            GameState.Instance.StateData.setCharacterBehaviourIndex (characterKey, behaviour);
        }
			
        private int getCurrentBehaviourStartValue () {
            int? behaviour = GameState.Instance.StateData.getCharacterBehaviourIndex(characterKey);

            if (behaviour != null) {
                return (int)behaviour;
            } else {
                return START_BEHAVIOUR;
            }
        }
    }
}