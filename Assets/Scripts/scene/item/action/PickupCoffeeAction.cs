using net.gubbi.goofy.item;

namespace net.gubbi.goofy.scene.item.action {

    public class PickupCoffeeAction : PickupAction {

        protected override void doPickupOfItem (SceneItem sceneItem) {
            inventory.addItem (new CoffeePotItem());

            if (removeSceneItem) {
                sceneItem.remove();
            }
        }
    }

}