using net.gubbi.goofy.character;
using net.gubbi.goofy.item;
using net.gubbi.goofy.state;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class ChangeCharacterSceneAction : SingleSceneItemAction {

        public string characterKey;
        public string scene;

        protected override void doAction (ItemType selectedItem) {
            GameObject character = GameObject.Find (characterKey);
            CharacterSceneHandler characterSceneHandler = character != null ? character.GetComponentInChildren<CharacterSceneHandler>() : null; 
                        
            if (characterSceneHandler != null) {
                characterSceneHandler.setCharacterScene (scene);
            } else {
                GameState.Instance.StateData.changeCharacterScene (characterKey, scene);
            }

            afterAction ();
        }
    }

}