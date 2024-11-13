using System;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace character.npc.behaviour {
    public class SetBehaviourBehaviour : CharacterBehaviour {

        public string otherCharacterKey;
        public BehaviourGroupType behaviourGroup;
        
        protected override void doBehaviour (Action onCompleteBehaviour) {
            GameObject character = GameObject.Find (otherCharacterKey);
            CharacterBehaviours behaviours = character != null ? character.GetComponentInChildren<CharacterBehaviours>() : null;             
            
            if (behaviours != null) {
                behaviours.start(behaviourGroup);
            }
            else {
                GameState.Instance.StateData.setCharacterBehaviourType(otherCharacterKey, behaviourGroup);
            }

            onCompleteBehaviour ();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {            
            //TODO: Should this start behaviour and call doEndOfBehaviour on in?
            
            return true;
        }

    }
}