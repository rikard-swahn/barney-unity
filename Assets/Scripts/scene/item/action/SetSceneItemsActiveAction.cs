using System.Collections.Generic;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.scene.item.action;

namespace Assets.Scripts.scene.item.action {
    public class SetSceneItemsActiveAction : SingleSceneItemAction {

        public List<string> items;
        public bool active;

        protected override void doAction (ItemType selectedItem) {
            setItemActive (items, active);
            afterAction ();
        }

        private void setItemActive(List<string> items, bool active) {
            foreach(string id in items) {
                SceneItemUtil.setItemActive (id, active);
            }
        }
    }
}