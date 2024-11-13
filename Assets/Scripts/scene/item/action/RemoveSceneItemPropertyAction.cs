using net.gubbi.goofy.item;
using net.gubbi.goofy.state;

namespace net.gubbi.goofy.scene.item.action {
    public class RemoveSceneItemPropertyAction : SingleSceneItemAction {

        public string itemKey;
        public string property;

        protected override void doAction (ItemType selectedItem) {
            GameState.Instance.StateData.removeSeneProperty(itemKey, property);
            afterAction();
        }

    }
}