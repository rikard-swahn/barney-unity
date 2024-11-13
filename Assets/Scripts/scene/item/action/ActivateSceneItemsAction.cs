using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.scene.item.action;

namespace Assets.Scripts.scene.item.action {
    public class ActivateSceneItemsAction : SingleSceneItemAction {
        public string[] activateItems;

        protected override void doAction (ItemType selectedItem) {
            foreach(string id in activateItems) {
                SceneItemUtil.setItemActive (id, true);
            }
            afterAction();
        }
    }
}
