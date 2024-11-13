namespace net.gubbi.goofy.item.inventory.action {

    public class CreateItemAction : InventoryAction {

        public ItemType createdItem;
        public bool removeItem1;
        public bool removeItem2;

        protected override void mainAction () {
            if(removeItem1) {
                inventory.removeItem (item1);
            }
            if(removeItem2) {
                inventory.removeItem (item2);
            }

            inventory.addItem (createdItem);			
        }

    }

}