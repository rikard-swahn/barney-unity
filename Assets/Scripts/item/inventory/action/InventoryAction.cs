using net.gubbi.goofy.audio;
using net.gubbi.goofy.character;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.item.inventory.action {
	
    public class InventoryAction : MonoBehaviour {

        public string sayAfter;
        public ItemType item1;
        public ItemType item2;
        
        public DelayedAudioClip sfxBefore;

        protected Inventory inventory;
        private CharacterFacade playerFacade;
        private SfxPlayer sfxPlayer;

        protected virtual void Awake () {
            GameObject inventoryGo = GameObject.Find(GameObjects.INVENTORY);
            this.inventory = inventoryGo.GetComponent<Inventory>();

            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            playerFacade = playerGo.GetComponent<CharacterFacade>();
            
            sfxPlayer = GameObject.Find (GameObjects.SFX_PLAYER).GetComponent<SfxPlayer>();
        }

        public void doAction () {
            beforeAction ();
            mainAction ();
            afterAction ();
        }

        private void beforeAction() {
            sfxPlayer.play(sfxBefore);
        }

        protected virtual void mainAction() {            
        }

        protected virtual void afterAction() {
            if (sayAfter.Length > 0) {
                playerFacade.sayTextTowardsCamera (sayAfter);
            }			
        }


    }

}