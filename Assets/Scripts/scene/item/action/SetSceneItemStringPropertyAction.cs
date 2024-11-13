using net.gubbi.goofy.item;
using net.gubbi.goofy.state;

namespace net.gubbi.goofy.scene.item.action {
    public class SetSceneItemStringPropertyAction : SingleSceneItemAction {

        public string itemKey;
        public string property;
        public string value;

        protected override void doAction (ItemType selectedItem) {
            GameState.Instance.StateData.setSceneProperty(itemKey, property, value);
            afterAction();
        }

    }
}