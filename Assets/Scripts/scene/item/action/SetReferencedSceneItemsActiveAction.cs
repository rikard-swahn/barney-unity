using System.Collections.Generic;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.scene.item.action;

namespace Assets.Scripts.scene.item.action {
    public class SetReferencedSceneItemsActiveAction : SingleSceneItemAction {

        public List<SceneItem> items;
        public bool active;

        protected override void doAction (ItemType selectedItem) {            
            items.ForEach(i => i.setActive(active));            
            afterAction ();
        }
    }
}