using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.ui.menu.ingame;

namespace Assets.Scripts.scene.item.action {
    public class ShowLightboxAction : SingleSceneItemAction {

        public InGameLightbox lightbox;

        protected override void doAction (ItemType selectedItem) {
            lightbox.show();
            afterAction();
        }

    }
}