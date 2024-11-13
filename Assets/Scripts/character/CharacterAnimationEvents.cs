using UnityEngine;

namespace net.gubbi.goofy.character {
    public class CharacterAnimationEvents : MonoBehaviour {

        public void setCharacterActive (string characterKey) {
            CharacterSceneHandler characterSceneHandler = GameObject.Find(characterKey).GetComponent<CharacterSceneHandler>();            
            characterSceneHandler.setCharacterActive(true);
        }

        public void setCharacterInactive (string characterKey) {
            CharacterSceneHandler characterSceneHandler = GameObject.Find(characterKey).GetComponent<CharacterSceneHandler>();            
            characterSceneHandler.setCharacterActive(false);            
        }

    }
}