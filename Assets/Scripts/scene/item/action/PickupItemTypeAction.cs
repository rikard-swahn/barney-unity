using net.gubbi.goofy.item;

namespace net.gubbi.goofy.scene.item.action {

    public class PickupItemTypeAction : PickupAction {

        public ItemType pickupItem;

        /// <summary>
        /// Adds item to player and removes scene item.
        /// </summary>
        protected override void doPickupOfItem (SceneItem sceneItem) {
            inventory.addItem (pickupItem);

            if (removeSceneItem)
            {
                sceneItem.remove();
            }
        }
    }

}