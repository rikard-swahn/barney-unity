using net.gubbi.goofy.character;
using net.gubbi.goofy.item;
using net.gubbi.goofy.state;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class DeactivateCharacterAction : SingleSceneItemAction {

        public string characterKey;

        private CharacterSceneHandler characterSceneHandler;

        protected override void Awake () {
            base.Awake ();

            GameObject character = GameObject.Find (characterKey);
            if (character != null) {
                this.characterSceneHandler = character.GetComponentInParent<CharacterSceneHandler>();
            }
        }

        protected override void doAction (ItemType selectedItem) {

            if (characterSceneHandler != null) {
                characterSceneHandler.setCharacterActive(false);
            }
            else {
                GameState.Instance.StateData.setCharacterActive(characterKey, false);
            }

            afterAction ();
        }
    }

}