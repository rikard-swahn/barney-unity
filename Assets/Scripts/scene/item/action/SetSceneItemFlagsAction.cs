using net.gubbi.goofy.extensions;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.state;

namespace Assets.Scripts.scene.item.action {

    public class SetSceneItemFlagsAction : SingleSceneItemAction {
        public string sceneItem;
        public string[] flags;        
        public bool set;       
        

        protected override void doAction (ItemType selectedItem) {            
            flags.ForEach(f => GameState.Instance.StateData.setSceneProperty(sceneItem, f, set));
            afterAction();
        }
    }
}
