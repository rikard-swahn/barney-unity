using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.door;

namespace net.gubbi.goofy.scene.item.action {

    public class SetDoorOpenAction : SingleSceneItemAction {

        public bool open = true;
        private Door door;

        protected override void Awake () {
            door = GetComponentInParent<Door> ();
            if (target == null) {
                target = door.gameObject;
            }
            base.Awake ();                        
        }

        protected override void doAction (ItemType selectedItem) {
            door.setOpen(open);
            afterAction ();
        }
    }

}