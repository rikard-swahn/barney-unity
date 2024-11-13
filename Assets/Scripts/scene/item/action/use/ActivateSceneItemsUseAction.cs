using net.gubbi.goofy.item;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action.use {

    public class ActivateSceneItemsUseAction : UseAction {
        public string[] activateItems;
        public string[] deactivateItems;
        public GameObject[] activateObjects;
        public GameObject[] deactivateObjects;

        protected override void afterAction(ItemType selectedItem) {
            foreach(string id in activateItems) {
                SceneItemUtil.setItemActive (id, true);
            }                
            foreach(string id in deactivateItems) {
                SceneItemUtil.setItemActive (id, false);
            }

            foreach(GameObject go in activateObjects) {
                SceneItemUtil.setItemActive (go, true);
            }                
            foreach(GameObject go in deactivateObjects) {
                SceneItemUtil.setItemActive (go, false);
            }

            base.afterAction (selectedItem);
        }
    }

}