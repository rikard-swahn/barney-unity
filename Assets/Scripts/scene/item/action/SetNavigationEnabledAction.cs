using net.gubbi.goofy.item;
using net.gubbi.goofy.util;
using PolyNav;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class SetNavigationEnabledAction : SingleSceneItemAction {
        public new bool enabled;
        
        private PolyNavAgent polynavAgent;

        protected override void Awake() {
            base.Awake();
            
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            polynavAgent = playerGo.GetComponentInChildren<PolyNavAgent>();            
        }

        protected override void doAction (ItemType selectedItem) {
            polynavAgent.enabled = enabled;
            afterAction();
        }
    }
}
