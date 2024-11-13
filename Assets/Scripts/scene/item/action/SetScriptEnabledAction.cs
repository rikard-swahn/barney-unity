using net.gubbi.goofy.item;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {
    public class SetScriptEnabledAction : SingleSceneItemAction {

        public MonoBehaviour script;
        public bool enabled;
        
        protected override void doAction (ItemType selectedItem) {
            if (script != null) {
                script.enabled = enabled;    
            }
            
            afterAction();
        }        
			
    }
}