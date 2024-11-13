using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.door;

namespace net.gubbi.goofy.scene.item.action.use {

    public class CloseDoorUseAction : UseAction {
        
        private Door door;

        protected override void Awake () {
            door = GetComponentInParent<Door> ();
            if (target == null) {
                target = door.gameObject;
            }
            base.Awake ();
            skipOnResume = true;
        }

        protected override void doAction (ItemType selectedItem) {
            afterAction (selectedItem);            
            door.setOpen(false);
        }
    }

}