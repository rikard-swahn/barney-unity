using net.gubbi.goofy.character;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using UnityEngine;

namespace Assets.Scripts.scene.item.action {
    public class ChangeCharacterSceneToCurrentAction : SingleSceneItemAction {

        public GameObject character;

        private CharacterSceneHandler sceneHandler;

        protected override void Awake () {
            base.Awake ();
            sceneHandler = character.GetComponent<CharacterSceneHandler> ();
        }

        protected override void doAction (ItemType selectedItem) {
            sceneHandler.setCharacterSceneToCurrent ();

            afterAction ();
        }
    }
}