using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.door;

namespace net.gubbi.goofy.scene.item.action.use {

    public class OpenDoorUseAction : UseAction {
        
        public bool actionAnimation = true;
        private Door door;

        protected override void Awake () {
            door = GetComponentInParent<Door> ();
            if (target == null) {
                target = door.gameObject;
            }
            base.Awake ();                        
            skipOnResume = false;
        }

        protected override void doAction (ItemType selectedItem) {
            if (actionAnimation && !door.isOpen()) {
                base.doAction(selectedItem);    
            }
            else {
                door.setOpen(true);
                afterAction (selectedItem);
            }
                        
        }

        protected override void afterAction(ItemType selectedItem) {
            door.setOpen(true);
            base.afterAction();
        }

    }

}