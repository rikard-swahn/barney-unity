using net.gubbi.goofy.item;

namespace net.gubbi.goofy.scene.item.action {

    public class PickupCashAction : PickupAction {

        public int amount;

        /// <summary>
        /// Adds item to player and removes scene item.
        /// </summary>
        protected override void doPickupOfItem (SceneItem sceneItem) {
            Item cash = new CardItem (amount);

            inventory.addItem (cash);

            if (removeSceneItem) {
                sceneItem.remove();
            }
        }
    }

}