using System;
using net.gubbi.goofy.character;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.action {
    public class TriggerSceneActionHandler : SceneActionHandler  {

        private SceneItem sceneItem;
        private CharacterFacade characterFacade;
        private Inventory inventory;

        private void Awake () {
            sceneItem = GetComponent<SceneItem>();
            characterFacade = GameObject.Find(GameObjects.PLAYER).GetComponent<CharacterFacade>();
            inventory = GameObject.Find(GameObjects.INVENTORY).GetComponent<Inventory>();
        }

        public override void doAction (ItemType selectedItem) {
            GameState.Instance.StateData.PlayerState.SceneActionTargetName = gameObject.name;

            Action clearTarget = delegate {
                GameState.Instance.StateData.PlayerState.clearActionAndTarget();
                inventory.deselectItem();
            };

            characterFacade.navStop();
            sceneItem.doAction(selectedItem, clearTarget);
        }

        public override string getLabelPrefix(ItemType selectedItem) {
            throw new NotImplementedException();
        }
    }
}