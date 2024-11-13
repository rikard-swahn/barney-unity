using net.gubbi.goofy.extensions;
using net.gubbi.goofy.item;
using net.gubbi.goofy.item.inventory;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class InventoryItemAction : SingleSceneItemAction {

        public ItemType[] addItems;
        public ItemType[] removeItems;
        private Inventory inventory;

        protected override void Awake () {
            base.Awake ();
            inventory = GameObject.Find(GameObjects.INVENTORY).GetComponent<Inventory>();
        }        

        protected override void doAction (ItemType selectedItem) {
            addItems.ForEach(i => inventory.addItem (i));
            removeItems.ForEach(i => inventory.removeItem(i));
            afterAction();
        }        
        
    }

}