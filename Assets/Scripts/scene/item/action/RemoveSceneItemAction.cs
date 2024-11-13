using net.gubbi.goofy.item;

namespace net.gubbi.goofy.scene.item.action {
    public class RemoveSceneItemAction : SingleSceneItemAction {

        public SceneItem sceneItem;

        protected override void doAction (ItemType selectedItem) {
            sceneItem.remove();
            afterAction();
        }        
        
    }
}