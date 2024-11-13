using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.util;
using UnityEngine;

namespace Assets.Scripts.scene.item.action {
    public class DeselectInventoryAction : SingleSceneItemAction {

        private Inventory inventory;

        protected override void Awake() {
            base.Awake();

            GameObject inventoryGo = GameObject.Find(GameObjects.INVENTORY);
            inventory = inventoryGo.GetComponent<Inventory>();
        }

        protected override void doAction(ItemType selectedItem) {
            inventory.deselectItem();
            afterAction();
        }
    }
}