using net.gubbi.goofy.item;
using net.gubbi.goofy.state;

namespace net.gubbi.goofy.scene.item.action.use {

    public class SetSceneItemFlagUseAction : UseAction {
        public string[] setItemFlags;
        public bool setToTrue = true;

        protected override void afterAction(ItemType selectedItem) {
            foreach(string flag in setItemFlags) {
                if (setToTrue) {
                    GameState.Instance.StateData.setSceneProperty(gameObject.name, flag, true);
                }
                else {
                    GameState.Instance.StateData.setSceneProperty(gameObject.name, flag, false);
                }
            }

            base.afterAction (selectedItem);
        }
    }

}