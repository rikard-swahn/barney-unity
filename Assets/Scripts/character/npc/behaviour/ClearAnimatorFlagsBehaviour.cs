﻿using System;
using net.gubbi.goofy.character.npc.behaviour;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.state;
using UnityEngine;

namespace Assets.Scripts.character.npc.behaviour {
    public class ClearAnimatorFlagsBehaviour : CharacterBehaviour {

        public string[] boolparams;
        private Animator[] animators;
        
        private new void Awake() {
            base.Awake ();
            animators = characterGo.GetComponentsInChildren<Animator>();
        }

        protected override void doBehaviour (Action onCompleteBehaviour) {
            GameState.Instance.StateData.clearCharacterAnimationBoolParams(characterKey, boolparams);
            boolparams.ForEach(p => {
                animators.ForEach(a => a.SetBool(p, false));
            });
            onCompleteBehaviour();
        }

        protected override bool doEndOfBehaviour (bool endingCurrentBehavior) {
            GameState.Instance.StateData.clearCharacterAnimationBoolParams(characterKey, boolparams);
            return true;
        }

    }
}