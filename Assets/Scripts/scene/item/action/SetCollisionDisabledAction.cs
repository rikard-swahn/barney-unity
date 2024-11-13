using net.gubbi.goofy.item;
using net.gubbi.goofy.player;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.util;
using UnityEngine;

namespace scene.item.action {
    public class SetCollisionDisabledAction : SingleSceneItemAction {

        public bool disabled;
        
        private PlayerMove playerMove;

        protected override void Awake () {
            base.Awake ();
            
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerMove = playerGo.GetComponent<PlayerMove>();
        }

        protected override void doAction (ItemType selectedItem) {            
            playerMove.setCollisionDisabled(disabled);
            afterAction();
        }
    }

}