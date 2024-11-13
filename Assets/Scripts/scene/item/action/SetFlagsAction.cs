using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.state;

namespace Assets.Scripts.scene.item.action {

    public class SetFlagsAction : SingleSceneItemAction {
        public string[] flags;

        protected override void doAction (ItemType selectedItem) {
            GameState.Instance.StateData.setFlags(flags);
            afterAction();
        }
    }
}
