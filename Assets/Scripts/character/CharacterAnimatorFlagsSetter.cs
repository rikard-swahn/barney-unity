using net.gubbi.goofy.extensions;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace character {
    public class CharacterAnimatorFlagsSetter : MonoBehaviour {
        
        private Animator[] animators;
        private string characterKey;

        private void Awake() {
            GameObject characterGo = gameObject.getAncestorWithTag (Tags.CHARACTER);
            characterKey = characterGo.name;            
            animators = characterGo.GetComponentsInChildren<Animator>();
        }

        public void setCharacterFlag(string parameter) {
            GameState.Instance.StateData.setCharacterAnimationBoolParams(characterKey, parameter);            
            animators.ForEach(a => a.SetBool(parameter, true));
        }
        
        public void clearCharacterFlag(string parameter) {
            GameState.Instance.StateData.clearCharacterAnimationBoolParams(characterKey, parameter);
            animators.ForEach(a => a.SetBool(parameter, false));
        }        

    }
}