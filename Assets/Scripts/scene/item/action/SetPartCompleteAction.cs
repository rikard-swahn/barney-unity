using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.state.settings;

namespace Assets.Scripts.scene.item.action {

    public class SetPartCompleteAction : SingleSceneItemAction {

        protected override void doAction (ItemType selectedItem) {
            SettingsState.Instance.setPartComplete();
            afterAction ();
        }
    }
}
