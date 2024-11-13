using net.gubbi.goofy.character;
using net.gubbi.goofy.item;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class TeleportAction : SingleSceneItemAction {
                
        public GameObject destination;
        
        private CharacterMove characterMove;

        protected override void Awake() {
            base.Awake();
            
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            characterMove = playerGo.GetComponentInChildren<CharacterMove>();            
        }

        protected override void doAction (ItemType selectedItem) {            
            characterMove.setPosition(destination.transform.position);
            afterAction();
        }
    }
}
